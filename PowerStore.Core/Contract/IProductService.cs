using System.Collections.Generic;
using System.Threading.Tasks;
using PowerStore.Core.Contract.Responses;
using PowerStore.Core.DTOs.ProductDtos;

namespace PowerStore.Core.Contract
{
    public interface IProductService
    {
        Task<ApiResponse<ProductResponseDto>> GetByIdAsync(int id);

        Task<ApiResponse<ProductDetailDto>> GetByIdWithDetailsAsync(int id);

        Task<ApiResponse<IReadOnlyList<ProductResponseDto>>> GetAllAsync(ProductSearchParams searchParams);

        Task<ApiResponse<IReadOnlyList<ProductResponseDto>>> GetByCategoryIdAsync(int categoryId, ProductSearchParams searchParams);

        Task<ApiResponse<ProductResponseDto>> CreateAsync(CreateProductDto createDto);

        Task<ApiResponse<ProductResponseDto>> UpdateAsync(UpdateProductDto updateDto);

        Task<ApiResponse<bool>> SoftDeleteAsync(int id);

        Task<decimal> CalculateProfitMarginAsync(int id);
    }
}
