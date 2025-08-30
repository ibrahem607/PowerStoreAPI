using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerStore.Service.SubAreaServices
{
    public class SubAreaService : ISubAreaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SubAreaService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SubAreaResponseDto> GetByIdAsync(int id)
        {
            var spec = new SubAreaSpecs(id);
            var subArea = await _unitOfWork.Repository<SubArea>().GetByIdWithSpecAsync(spec);
            if (subArea == null) throw new KeyNotFoundException($"SubArea with Id {id} not found.");
            return _mapper.Map<SubAreaResponseDto>(subArea);
        }

        public async Task<IReadOnlyList<SubAreaResponseDto>> GetAllAsync(SubAreaSearchParams searchParams)
        {
            var spec = new SubAreaSpecs(searchParams);
            var subAreas = await _unitOfWork.Repository<SubArea>().GetAllWithSpecAsync(spec);
            return _mapper.Map<IReadOnlyList<SubAreaResponseDto>>(subAreas);
        }

        public async Task<IReadOnlyList<SubAreaResponseDto>> GetByMainAreaIdAsync(int mainAreaId, SubAreaSearchParams searchParams)
        {
            var spec = new SubAreaSpecs(mainAreaId, searchParams);
            var subAreas = await _unitOfWork.Repository<SubArea>().GetAllWithSpecAsync(spec);
            return _mapper.Map<IReadOnlyList<SubAreaResponseDto>>(subAreas);
        }

        public async Task<SubAreaResponseDto> CreateAsync(CreateSubAreaDto createDto)
        {
            // Verify MainArea exists
            var mainArea = await _unitOfWork.Repository<MainArea>().GetByIdAsync(createDto.MainAreaId);
            if (mainArea == null) throw new KeyNotFoundException($"MainArea with Id {createDto.MainAreaId} not found.");

            var subArea = _mapper.Map<SubArea>(createDto);
            _unitOfWork.Repository<SubArea>().Add(subArea);
            await _unitOfWork.CompleteAsync();

            // Return the created SubArea with MainArea name
            var response = _mapper.Map<SubAreaResponseDto>(subArea);
            response.MainAreaName = mainArea.Name;
            return response;
        }

        public async Task<SubAreaResponseDto> UpdateAsync(UpdateSubAreaDto updateDto)
        {
            var subArea = await _unitOfWork.Repository<SubArea>().GetByIdAsync(updateDto.Id);
            if (subArea == null) throw new KeyNotFoundException($"SubArea with Id {updateDto.Id} not found.");

            // Verify MainArea exists if changing MainArea
            if (subArea.MainAreaId != updateDto.MainAreaId)
            {
                var mainArea = await _unitOfWork.Repository<MainArea>().GetByIdAsync(updateDto.MainAreaId);
                if (mainArea == null) throw new KeyNotFoundException($"MainArea with Id {updateDto.MainAreaId} not found.");
            }

            _mapper.Map(updateDto, subArea);
            subArea.UpdatedDate = DateTime.UtcNow;

            _unitOfWork.Repository<SubArea>().Update(subArea);
            await _unitOfWork.CompleteAsync();

            // Get updated entity with MainArea included
            var updatedSubArea = await _unitOfWork.Repository<SubArea>().GetByIdWithSpecAsync(new SubAreaSpecs(subArea.Id));
            return _mapper.Map<SubAreaResponseDto>(updatedSubArea);
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            var subArea = await _unitOfWork.Repository<SubArea>().GetByIdAsync(id);
            if (subArea == null) throw new KeyNotFoundException($"SubArea with Id {id} not found.");

            _unitOfWork.Repository<SubArea>().Delete(subArea);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}
