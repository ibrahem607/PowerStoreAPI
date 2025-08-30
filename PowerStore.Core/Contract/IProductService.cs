using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerStore.Core.DTOs.ProductDtos;

namespace PowerStore.Core.Contract
{
    public interface IProductService
    {
        Task<ProductResponseDto> GetByIdAsync(int id);
        Task<ProductDetailDto> GetByIdWithDetailsAsync(int id);
        Task<IReadOnlyList<ProductResponseDto>> GetAllAsync(ProductSearchParams searchParams);
        Task<IReadOnlyList<ProductResponseDto>> GetByCategoryIdAsync(int categoryId, ProductSearchParams searchParams);
        Task<ProductResponseDto> CreateAsync(CreateProductDto createDto);
        Task<ProductResponseDto> UpdateAsync(UpdateProductDto updateDto);
        Task<bool> SoftDeleteAsync(int id);
        Task<decimal> CalculateProfitMarginAsync(int id);
    }
}
