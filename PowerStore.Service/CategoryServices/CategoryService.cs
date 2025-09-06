using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using PowerStore.Core;
using PowerStore.Core.Contract;
using PowerStore.Core.Contract.Responses;
using PowerStore.Core.DTOs.CategoryDtos;
using PowerStore.Core.Entities;
using PowerStore.Core.EntitiesSpecifications;
using PowerStore.Core.Specifications;

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

        public async Task<ApiResponse<CategoryResponseDto>> GetByIdAsync(int id)
        {
            var spec = new CategorySpecs(id);
            var category = await _unitOfWork.Repository<Category>().GetByIdWithSpecAsync(spec);

            if (category == null)
                return ApiResponse<CategoryResponseDto>.ErrorResponse($"Category with Id {id} not found.");

            var response = _mapper.Map<CategoryResponseDto>(category);
            response.ProductsCount = category.Products?.Count ?? 0;
            return ApiResponse<CategoryResponseDto>.SuccessResponse(response, "Category retrieved successfully.");
        }

        public async Task<ApiResponse<CategoryWithProductsDto>> GetByIdWithProductsAsync(int id)
        {
            var spec = new CategorySpecs(id);
            var category = await _unitOfWork.Repository<Category>().GetByIdWithSpecAsync(spec);

            if (category == null)
                return ApiResponse<CategoryWithProductsDto>.ErrorResponse($"Category with Id {id} not found.");

            var response = _mapper.Map<CategoryWithProductsDto>(category);
            return ApiResponse<CategoryWithProductsDto>.SuccessResponse(response, "Category with products retrieved successfully.");
        }

        public async Task<ApiResponse<IReadOnlyList<CategoryResponseDto>>> GetAllAsync(CategorySearchParams searchParams)
        {
            var spec = new CategorySpecs(searchParams);
            var categories = await _unitOfWork.Repository<Category>().GetAllWithSpecAsync(spec);

            var response = categories.Select(c =>
            {
                var dto = _mapper.Map<CategoryResponseDto>(c);
                dto.ProductsCount = c.Products?.Count ?? 0;
                return dto;
            }).ToList();

            return ApiResponse<IReadOnlyList<CategoryResponseDto>>.SuccessResponse(response, "Categories retrieved successfully.");
        }

        public async Task<ApiResponse<CategoryResponseDto>> CreateAsync(CreateCategoryDto createDto)
        {
            var category = _mapper.Map<Category>(createDto);
            _unitOfWork.Repository<Category>().Add(category);
            await _unitOfWork.CompleteAsync();

            var response = _mapper.Map<CategoryResponseDto>(category);
            return ApiResponse<CategoryResponseDto>.SuccessResponse(response, "Category created successfully.");
        }

        public async Task<ApiResponse<CategoryResponseDto>> UpdateAsync(UpdateCategoryDto updateDto)
        {
            var category = await _unitOfWork.Repository<Category>().GetByIdAsync(updateDto.Id);
            if (category == null)
                return ApiResponse<CategoryResponseDto>.ErrorResponse($"Category with Id {updateDto.Id} not found.");

            _mapper.Map(updateDto, category);
            category.UpdatedDate = DateTime.UtcNow;

            _unitOfWork.Repository<Category>().Update(category);
            await _unitOfWork.CompleteAsync();

            var response = _mapper.Map<CategoryResponseDto>(category);
            return ApiResponse<CategoryResponseDto>.SuccessResponse(response, "Category updated successfully.");
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(int id)
        {
            var category = await _unitOfWork.Repository<Category>().GetByIdAsync(id);
            if (category == null)
                return ApiResponse<bool>.ErrorResponse($"Category with Id {id} not found.");

            if (await HasProductsAsync(id))
                return ApiResponse<bool>.ErrorResponse("Cannot delete category that has associated products.");

            _unitOfWork.Repository<Category>().Delete(category);
            await _unitOfWork.CompleteAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Category deleted successfully.");
        }

        public async Task<bool> HasProductsAsync(int id)
        {
            var category = await _unitOfWork.Repository<Category>().GetByIdAsync(id);
            if (category == null)
                return false;

            var spec = new BaseSpecifications<Product>(p => p.CategoryId == id && !p.IsDeleted);
            var products = await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(spec);

            return products.Any();
        }
    }
}
