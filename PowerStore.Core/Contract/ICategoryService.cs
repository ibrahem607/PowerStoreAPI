using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerStore.Core.DTOs.CategoryDtos;
using PowerStore.Core.EntitiesSpecifications;

namespace PowerStore.Core.Contract
{
    public interface ICategoryService
    {
        Task<CategoryResponseDto> GetByIdAsync(int id);
        Task<CategoryWithProductsDto> GetByIdWithProductsAsync(int id);
        Task<IReadOnlyList<CategoryResponseDto>> GetAllAsync(CategorySearchParams searchParams);
        Task<CategoryResponseDto> CreateAsync(CreateCategoryDto createDto);
        Task<CategoryResponseDto> UpdateAsync(UpdateCategoryDto updateDto);
        Task<bool> SoftDeleteAsync(int id);
        Task<bool> HasProductsAsync(int id);
    }
}
