using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerStore.Core.Contract.Responses;
using PowerStore.Core.DTOs.MainAreaDtos;
using PowerStore.Core.EntitiesSpecifications;

namespace PowerStore.Core.Contract
{
    public interface IMainAreaService
    {
        Task<ApiResponse<MainAreaResponseDto>> GetByIdAsync(int id);
        
        Task<ApiResponse<IReadOnlyList<MainAreaResponseDto>>> GetAllAsync(MainAreaSearchParams searchParams);

        Task<ApiResponse<MainAreaResponseDto>> CreateAsync(CreateMainAreaDto createDto);

        Task<ApiResponse<MainAreaResponseDto>> UpdateAsync(UpdateMainAreaDto updateDto);

        Task<ApiResponse<bool>> SoftDeleteAsync(int id);
    }
}
