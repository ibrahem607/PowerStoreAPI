using System.Collections.Generic;
using System.Threading.Tasks;
using PowerStore.Core.Contract.Responses;
using PowerStore.Core.DTOs.MainAreaDtos;
using PowerStore.Core.DTOs.SubAreaDtos;
using PowerStore.Core.EntitiesSpecifications;

namespace PowerStore.Core.Contract
{
    public interface ISubAreaService
    {
        Task<ApiResponse<SubAreaResponseDto>> GetByIdAsync(int id);

        Task<ApiResponse<IReadOnlyList<SubAreaResponseDto>>> GetAllAsync(SubAreaSearchParams searchParams);

        Task<ApiResponse<IReadOnlyList<SubAreaResponseDto>>> GetByMainAreaIdAsync(int mainAreaId, SubAreaSearchParams searchParams);

        Task<ApiResponse<SubAreaResponseDto>> CreateAsync(CreateSubAreaDto createDto);

        Task<ApiResponse<SubAreaResponseDto>> UpdateAsync(UpdateSubAreaDto updateDto);

        Task<ApiResponse<bool>> SoftDeleteAsync(int id);
    }
}
