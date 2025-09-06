using System.Collections.Generic;
using System.Threading.Tasks;
using PowerStore.Core.Contract.Responses;
using PowerStore.Core.DTOs.CategoryDtos;
using PowerStore.Core.EntitiesSpecifications;

namespace PowerStore.Core.Contract
{
    public interface ICategoryService
    {
        Task<ApiResponse<CategoryResponseDto>> GetByIdAsync(int id);

        Task<ApiResponse<CategoryWithProductsDto>> GetByIdWithProductsAsync(int id);

        Task<ApiResponse<IReadOnlyList<CategoryResponseDto>>> GetAllAsync(CategorySearchParams searchParams);

        Task<ApiResponse<CategoryResponseDto>> CreateAsync(CreateCategoryDto createDto);

        Task<ApiResponse<CategoryResponseDto>> UpdateAsync(UpdateCategoryDto updateDto);

        Task<ApiResponse<bool>> SoftDeleteAsync(int id);

        Task<bool> HasProductsAsync(int id);
    }
}
