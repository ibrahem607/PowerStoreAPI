using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PowerStore.Core.Contract;
using PowerStore.Core.DTOs.CategoryDtos;
using PowerStore.Core.Entities;
using PowerStore.Core.EntitiesSpecifications;
using PowerStore.Core.Specifications;
using PowerStore.Core;

namespace PowerStore.Service.CategoryServices
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CategoryResponseDto> GetByIdAsync(int id)
        {
            var spec = new CategorySpecs(id);
            var category = await _unitOfWork.Repository<Category>().GetByIdWithSpecAsync(spec);

            if (category == null) throw new KeyNotFoundException($"Category with Id {id} not found.");

            var response = _mapper.Map<CategoryResponseDto>(category);
            response.ProductsCount = category.Products?.Count ?? 0;
            return response;
        }

        public async Task<CategoryWithProductsDto> GetByIdWithProductsAsync(int id)
        {
            var spec = new CategorySpecs(id);
            var category = await _unitOfWork.Repository<Category>().GetByIdWithSpecAsync(spec);

            if (category == null) throw new KeyNotFoundException($"Category with Id {id} not found.");

            return _mapper.Map<CategoryWithProductsDto>(category);
        }

        public async Task<IReadOnlyList<CategoryResponseDto>> GetAllAsync(CategorySearchParams searchParams)
        {
            var spec = new CategorySpecs(searchParams);
            var categories = await _unitOfWork.Repository<Category>().GetAllWithSpecAsync(spec);

            return categories.Select(c =>
            {
                var dto = _mapper.Map<CategoryResponseDto>(c);
                dto.ProductsCount = c.Products?.Count ?? 0;
                return dto;
            }).ToList();
        }

        public async Task<CategoryResponseDto> CreateAsync(CreateCategoryDto createDto)
        {
            var category = _mapper.Map<Category>(createDto);
            _unitOfWork.Repository<Category>().Add(category);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<CategoryResponseDto>(category);
        }

        public async Task<CategoryResponseDto> UpdateAsync(UpdateCategoryDto updateDto)
        {
            var category = await _unitOfWork.Repository<Category>().GetByIdAsync(updateDto.Id);
            if (category == null) throw new KeyNotFoundException($"Category with Id {updateDto.Id} not found.");

            _mapper.Map(updateDto, category);
            category.UpdatedDate = DateTime.UtcNow;

            _unitOfWork.Repository<Category>().Update(category);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<CategoryResponseDto>(category);
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            var category = await _unitOfWork.Repository<Category>().GetByIdAsync(id);
            if (category == null) throw new KeyNotFoundException($"Category with Id {id} not found.");

            // Check if category has products
            if (await HasProductsAsync(id))
            {
                throw new InvalidOperationException("Cannot delete category that has associated products.");
            }

            _unitOfWork.Repository<Category>().Delete(category);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> HasProductsAsync(int id)
        {
            var category = await _unitOfWork.Repository<Category>().GetByIdAsync(id);
            if (category == null) return false;

            // Check if category has any non-deleted products
            var spec = new BaseSpecifications<Product>(p => p.CategoryId == id && !p.IsDeleted);
            var products = await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(spec);

            return products.Any();
        }
    }
}