using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerStore.Core.DTOs.MainAreaDtos;
using PowerStore.Core.EntitiesSpecifications;

namespace PowerStore.Core.Contract
{
    public interface IMainAreaService
    {
        Task<MainAreaResponseDto> GetByIdAsync(int id);
        Task<IReadOnlyList<MainAreaResponseDto>> GetAllAsync(MainAreaSearchParams searchParams);
        Task<MainAreaResponseDto> CreateAsync(CreateMainAreaDto createDto);
        Task<MainAreaResponseDto> UpdateAsync(UpdateMainAreaDto updateDto);
        Task<bool> SoftDeleteAsync(int id);
    }
}
