using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PowerStore.Core.Contract;
using PowerStore.Core.DTOs.ProductDtos;
using PowerStore.Core.Entities;
using PowerStore.Core.EntitiesSpecifications;
using PowerStore.Core;

namespace PowerStore.Service.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ProductResponseDto> GetByIdAsync(int id)
        {
            var spec = new ProductSpecs(id);
            var product = await _unitOfWork.Repository<Product>().GetByIdWithSpecAsync(spec);

            if (product == null) throw new KeyNotFoundException($"Product with Id {id} not found.");

            var response = _mapper.Map<ProductResponseDto>(product);
            response.ProfitMargin = await CalculateProfitMarginAsync(id);
            return response;
        }

        public async Task<ProductDetailDto> GetByIdWithDetailsAsync(int id)
        {
            var spec = new ProductSpecs(id);
            var product = await _unitOfWork.Repository<Product>().GetByIdWithSpecAsync(spec);

            if (product == null) throw new KeyNotFoundException($"Product with Id {id} not found.");

            var response = _mapper.Map<ProductDetailDto>(product);
            response.ProfitMargin = await CalculateProfitMarginAsync(id);
            return response;
        }

        public async Task<IReadOnlyList<ProductResponseDto>> GetAllAsync(ProductSearchParams searchParams)
        {
            var spec = new ProductSpecs(searchParams);
            var products = await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(spec);

            var response = _mapper.Map<IReadOnlyList<ProductResponseDto>>(products);

            // Calculate profit margin for each product
            foreach (var productDto in response)
            {
                productDto.ProfitMargin = await CalculateProfitMarginAsync(productDto.Id);
            }

            return response;
        }

        public async Task<IReadOnlyList<ProductResponseDto>> GetByCategoryIdAsync(int categoryId, ProductSearchParams searchParams)
        {
            // Verify category exists
            var category = await _unitOfWork.Repository<Category>().GetByIdAsync(categoryId);
            if (category == null) throw new KeyNotFoundException($"Category with Id {categoryId} not found.");

            var spec = new ProductSpecs(categoryId, searchParams);
            var products = await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(spec);

            return _mapper.Map<IReadOnlyList<ProductResponseDto>>(products);
        }

        public async Task<ProductResponseDto> CreateAsync(CreateProductDto createDto)
        {
            // Verify category exists
            var category = await _unitOfWork.Repository<Category>().GetByIdAsync(createDto.CategoryId);
            if (category == null) throw new KeyNotFoundException($"Category with Id {createDto.CategoryId} not found.");

            // Validate sale price > purchase price
            if (createDto.SalePrice <= createDto.PurchasePrice)
            {
                throw new InvalidOperationException("Sale price must be greater than purchase price.");
            }

            var product = _mapper.Map<Product>(createDto);
            _unitOfWork.Repository<Product>().Add(product);
            await _unitOfWork.CompleteAsync();

            var response = _mapper.Map<ProductResponseDto>(product);
            response.CategoryName = category.Name;
            response.ProfitMargin = await CalculateProfitMarginAsync(product.Id);
            return response;
        }

        public async Task<ProductResponseDto> UpdateAsync(UpdateProductDto updateDto)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(updateDto.Id);
            if (product == null) throw new KeyNotFoundException($"Product with Id {updateDto.Id} not found.");

            // Verify category exists if changing category
            if (product.CategoryId != updateDto.CategoryId)
            {
                var category = await _unitOfWork.Repository<Category>().GetByIdAsync(updateDto.CategoryId);
                if (category == null) throw new KeyNotFoundException($"Category with Id {updateDto.CategoryId} not found.");
            }

            // Validate sale price > purchase price
            if (updateDto.SalePrice <= updateDto.PurchasePrice)
            {
                throw new InvalidOperationException("Sale price must be greater than purchase price.");
            }

            _mapper.Map(updateDto, product);
            product.UpdatedDate = DateTime.UtcNow;

            _unitOfWork.Repository<Product>().Update(product);
            await _unitOfWork.CompleteAsync();

            // Get updated entity with category included
            var updatedProduct = await _unitOfWork.Repository<Product>().GetByIdWithSpecAsync(new ProductSpecs(product.Id));
            var response = _mapper.Map<ProductResponseDto>(updatedProduct);
            response.ProfitMargin = await CalculateProfitMarginAsync(product.Id);
            return response;
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
            if (product == null) throw new KeyNotFoundException($"Product with Id {id} not found.");

            _unitOfWork.Repository<Product>().Delete(product);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<decimal> CalculateProfitMarginAsync(int id)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
            if (product == null) return 0;

            var profit = product.SalePrice - product.PurchasePrice;
            var profitMargin = (profit / product.SalePrice) * 100;
            return Math.Round(profitMargin, 2);
        }
    }
}
