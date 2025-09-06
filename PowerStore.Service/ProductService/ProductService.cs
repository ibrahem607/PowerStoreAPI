using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using PowerStore.Core;
using PowerStore.Core.Contract;
using PowerStore.Core.Contract.Responses;
using PowerStore.Core.DTOs.ProductDtos;
using PowerStore.Core.Entities;
using PowerStore.Core.EntitiesSpecifications;

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

        public async Task<ApiResponse<ProductResponseDto>> GetByIdAsync(int id)
        {
            var spec = new ProductSpecs(id);
            var product = await _unitOfWork.Repository<Product>().GetByIdWithSpecAsync(spec);

            if (product == null)
                return ApiResponse<ProductResponseDto>.ErrorResponse($"Product with Id {id} not found.");

            var response = _mapper.Map<ProductResponseDto>(product);
            response.ProfitMargin = await CalculateProfitMarginAsync(id);
            return ApiResponse<ProductResponseDto>.SuccessResponse(response, "Product retrieved successfully.");
        }

        public async Task<ApiResponse<ProductDetailDto>> GetByIdWithDetailsAsync(int id)
        {
            var spec = new ProductSpecs(id);
            var product = await _unitOfWork.Repository<Product>().GetByIdWithSpecAsync(spec);

            if (product == null)
                return ApiResponse<ProductDetailDto>.ErrorResponse($"Product with Id {id} not found.");

            var response = _mapper.Map<ProductDetailDto>(product);
            response.ProfitMargin = await CalculateProfitMarginAsync(id);
            return ApiResponse<ProductDetailDto>.SuccessResponse(response, "Product with details retrieved successfully.");
        }

        public async Task<ApiResponse<IReadOnlyList<ProductResponseDto>>> GetAllAsync(ProductSearchParams searchParams)
        {
            var spec = new ProductSpecs(searchParams);
            var products = await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(spec);

            var response = _mapper.Map<IReadOnlyList<ProductResponseDto>>(products);

            foreach (var productDto in response)
            {
                productDto.ProfitMargin = await CalculateProfitMarginAsync(productDto.Id);
            }

            return ApiResponse<IReadOnlyList<ProductResponseDto>>.SuccessResponse(response, "Products retrieved successfully.");
        }

        public async Task<ApiResponse<IReadOnlyList<ProductResponseDto>>> GetByCategoryIdAsync(int categoryId, ProductSearchParams searchParams)
        {
            var category = await _unitOfWork.Repository<Category>().GetByIdAsync(categoryId);
            if (category == null)
                return ApiResponse<IReadOnlyList<ProductResponseDto>>.ErrorResponse($"Category with Id {categoryId} not found.");

            var spec = new ProductSpecs(categoryId, searchParams);
            var products = await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(spec);

            var response = _mapper.Map<IReadOnlyList<ProductResponseDto>>(products);
            return ApiResponse<IReadOnlyList<ProductResponseDto>>.SuccessResponse(response, "Products retrieved successfully for the category.");
        }

        public async Task<ApiResponse<ProductResponseDto>> CreateAsync(CreateProductDto createDto)
        {
            var category = await _unitOfWork.Repository<Category>().GetByIdAsync(createDto.CategoryId);
            if (category == null)
                return ApiResponse<ProductResponseDto>.ErrorResponse($"Category with Id {createDto.CategoryId} not found.");

            if (createDto.SalePrice <= createDto.PurchasePrice)
                return ApiResponse<ProductResponseDto>.ErrorResponse("Sale price must be greater than purchase price.");

            var product = _mapper.Map<Product>(createDto);
            _unitOfWork.Repository<Product>().Add(product);
            await _unitOfWork.CompleteAsync();

            var response = _mapper.Map<ProductResponseDto>(product);
            response.CategoryName = category.Name;
            response.ProfitMargin = await CalculateProfitMarginAsync(product.Id);

            return ApiResponse<ProductResponseDto>.SuccessResponse(response, "Product created successfully.");
        }

        public async Task<ApiResponse<ProductResponseDto>> UpdateAsync(UpdateProductDto updateDto)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(updateDto.Id);
            if (product == null)
                return ApiResponse<ProductResponseDto>.ErrorResponse($"Product with Id {updateDto.Id} not found.");

            if (product.CategoryId != updateDto.CategoryId)
            {
                var category = await _unitOfWork.Repository<Category>().GetByIdAsync(updateDto.CategoryId);
                if (category == null)
                    return ApiResponse<ProductResponseDto>.ErrorResponse($"Category with Id {updateDto.CategoryId} not found.");
            }

            if (updateDto.SalePrice <= updateDto.PurchasePrice)
                return ApiResponse<ProductResponseDto>.ErrorResponse("Sale price must be greater than purchase price.");

            _mapper.Map(updateDto, product);
            product.UpdatedDate = DateTime.UtcNow;

            _unitOfWork.Repository<Product>().Update(product);
            await _unitOfWork.CompleteAsync();

            var updatedProduct = await _unitOfWork.Repository<Product>().GetByIdWithSpecAsync(new ProductSpecs(product.Id));
            var response = _mapper.Map<ProductResponseDto>(updatedProduct);
            response.ProfitMargin = await CalculateProfitMarginAsync(product.Id);

            return ApiResponse<ProductResponseDto>.SuccessResponse(response, "Product updated successfully.");
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(int id)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
            if (product == null)
                return ApiResponse<bool>.ErrorResponse($"Product with Id {id} not found.");

            _unitOfWork.Repository<Product>().Delete(product);
            await _unitOfWork.CompleteAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Product deleted successfully.");
        }

        public async Task<decimal> CalculateProfitMarginAsync(int id)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
            if (product == null)
                return 0;

            var profit = product.SalePrice - product.PurchasePrice;
            var profitMargin = (profit / product.SalePrice) * 100;
            return Math.Round(profitMargin, 2);
        }
    }
}
