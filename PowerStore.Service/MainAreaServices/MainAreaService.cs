using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PowerStore.Core.Contract;
using PowerStore.Core.DTOs.MainAreaDtos;
using PowerStore.Core.Entities;
using PowerStore.Core.EntitiesSpecifications;
using PowerStore.Core;

namespace PowerStore.Service.MainAreaServices
{
    public class MainAreaService : IMainAreaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MainAreaService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<MainAreaResponseDto> GetByIdAsync(int id)
        {
            var spec = new MainAreaSpecs(id);
            var mainArea = await _unitOfWork.Repository<MainArea>().GetByIdWithSpecAsync(spec);
            if (mainArea == null) throw new KeyNotFoundException($"MainArea with Id {id} not found.");
            return _mapper.Map<MainAreaResponseDto>(mainArea);
        }

        public async Task<IReadOnlyList<MainAreaResponseDto>> GetAllAsync(MainAreaSearchParams searchParams)
        {
            var spec = new MainAreaSpecs(searchParams);
            var mainAreas = await _unitOfWork.Repository<MainArea>().GetAllWithSpecAsync(spec);
            return _mapper.Map<IReadOnlyList<MainAreaResponseDto>>(mainAreas);
        }

        public async Task<MainAreaResponseDto> CreateAsync(CreateMainAreaDto createDto)
        {
            var mainArea = _mapper.Map<MainArea>(createDto);
            _unitOfWork.Repository<MainArea>().Add(mainArea);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<MainAreaResponseDto>(mainArea);
        }

        public async Task<MainAreaResponseDto> UpdateAsync(UpdateMainAreaDto updateDto)
        {
            var mainArea = await _unitOfWork.Repository<MainArea>().GetByIdAsync(updateDto.Id);
            if (mainArea == null) throw new KeyNotFoundException($"MainArea with Id {updateDto.Id} not found.");

            _mapper.Map(updateDto, mainArea); // Map updates from DTO to the fetched entity
            mainArea.UpdatedDate = DateTime.UtcNow;

            _unitOfWork.Repository<MainArea>().Update(mainArea);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<MainAreaResponseDto>(mainArea);
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            var mainArea = await _unitOfWork.Repository<MainArea>().GetByIdAsync(id);
            if (mainArea == null) throw new KeyNotFoundException($"MainArea with Id {id} not found.");

            _unitOfWork.Repository<MainArea>().Delete(mainArea); // This performs the soft delete
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}
