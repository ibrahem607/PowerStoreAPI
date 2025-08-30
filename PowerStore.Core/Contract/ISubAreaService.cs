using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerStore.Core.DTOs.MainAreaDtos;
using PowerStore.Core.DTOs.SubAreaDtos;
using PowerStore.Core.EntitiesSpecifications;

namespace PowerStore.Core.Contract
{
    public interface ISubAreaService
    {
        Task<SubAreaResponseDto> GetByIdAsync(int id);
        Task<IReadOnlyList<SubAreaResponseDto>> GetAllAsync(SubAreaSearchParams searchParams);
        Task<IReadOnlyList<SubAreaResponseDto>> GetByMainAreaIdAsync(int mainAreaId, SubAreaSearchParams searchParams);
        Task<SubAreaResponseDto> CreateAsync(CreateSubAreaDto createDto);
        Task<SubAreaResponseDto> UpdateAsync(UpdateSubAreaDto updateDto);
        Task<bool> SoftDeleteAsync(int id);
    }
}
