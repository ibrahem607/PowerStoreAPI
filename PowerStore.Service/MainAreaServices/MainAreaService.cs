using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using PowerStore.Core;
using PowerStore.Core.Contract;
using PowerStore.Core.Contract.Responses;
using PowerStore.Core.DTOs.MainAreaDtos;
using PowerStore.Core.Entities;
using PowerStore.Core.EntitiesSpecifications;

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

        public async Task<ApiResponse<MainAreaResponseDto>> GetByIdAsync(int id)
        {
            var spec = new MainAreaSpecs(id);
            var mainArea = await _unitOfWork.Repository<MainArea>().GetByIdWithSpecAsync(spec);

            if (mainArea == null)
                return ApiResponse<MainAreaResponseDto>.ErrorResponse($"MainArea with Id {id} not found.");

            var mainAreaDto = _mapper.Map<MainAreaResponseDto>(mainArea);
            return ApiResponse<MainAreaResponseDto>.SuccessResponse(mainAreaDto, "Main area retrieved successfully.");
        }

        public async Task<ApiResponse<IReadOnlyList<MainAreaResponseDto>>> GetAllAsync(MainAreaSearchParams searchParams)
        {
            var spec = new MainAreaSpecs(searchParams);
            var mainAreas = await _unitOfWork.Repository<MainArea>().GetAllWithSpecAsync(spec);

            var mainAreasDto = mainAreas.Select(ma => _mapper.Map<MainAreaResponseDto>(ma)).ToList();
            return ApiResponse<IReadOnlyList<MainAreaResponseDto>>.SuccessResponse(mainAreasDto, "Main areas retrieved successfully.");
        }

        public async Task<ApiResponse<MainAreaResponseDto>> CreateAsync(CreateMainAreaDto createDto)
        {
            var mainArea = _mapper.Map<MainArea>(createDto);
            _unitOfWork.Repository<MainArea>().Add(mainArea);
            await _unitOfWork.CompleteAsync();

            var mainAreaDto = _mapper.Map<MainAreaResponseDto>(mainArea);
            return ApiResponse<MainAreaResponseDto>.SuccessResponse(mainAreaDto, "Main area created successfully.");
        }

        public async Task<ApiResponse<MainAreaResponseDto>> UpdateAsync(UpdateMainAreaDto updateDto)
        {
            var mainArea = await _unitOfWork.Repository<MainArea>().GetByIdAsync(updateDto.Id);
            if (mainArea == null)
                return ApiResponse<MainAreaResponseDto>.ErrorResponse($"MainArea with Id {updateDto.Id} not found.");

            _mapper.Map(updateDto, mainArea);
            mainArea.UpdatedDate = DateTime.UtcNow;

            _unitOfWork.Repository<MainArea>().Update(mainArea);
            await _unitOfWork.CompleteAsync();

            var mainAreaDto = _mapper.Map<MainAreaResponseDto>(mainArea);
            return ApiResponse<MainAreaResponseDto>.SuccessResponse(mainAreaDto, "Main area updated successfully.");
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(int id)
        {
            var mainArea = await _unitOfWork.Repository<MainArea>().GetByIdAsync(id);
            if (mainArea == null)
                return ApiResponse<bool>.ErrorResponse($"MainArea with Id {id} not found.");

            _unitOfWork.Repository<MainArea>().Delete(mainArea); // Soft delete
            await _unitOfWork.CompleteAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Main area deleted successfully.");
        }
    }
}
