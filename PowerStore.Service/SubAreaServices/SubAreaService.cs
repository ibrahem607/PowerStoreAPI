using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using PowerStore.Core;
using PowerStore.Core.Contract;
using PowerStore.Core.Contract.Responses;
using PowerStore.Core.DTOs.MainAreaDtos;
using PowerStore.Core.DTOs.SubAreaDtos;
using PowerStore.Core.Entities;
using PowerStore.Core.EntitiesSpecifications;

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

        public async Task<ApiResponse<SubAreaResponseDto>> GetByIdAsync(int id)
        {
            var spec = new SubAreaSpecs(id);
            var subArea = await _unitOfWork.Repository<SubArea>().GetByIdWithSpecAsync(spec);

            if (subArea == null)
                return ApiResponse<SubAreaResponseDto>.ErrorResponse($"SubArea with Id {id} not found.");

            var subAreaDto = _mapper.Map<SubAreaResponseDto>(subArea);
            return ApiResponse<SubAreaResponseDto>.SuccessResponse(subAreaDto, "SubArea retrieved successfully.");
        }

        public async Task<ApiResponse<IReadOnlyList<SubAreaResponseDto>>> GetAllAsync(SubAreaSearchParams searchParams)
        {
            var spec = new SubAreaSpecs(searchParams);
            var subAreas = await _unitOfWork.Repository<SubArea>().GetAllWithSpecAsync(spec);

            var subAreasDto = subAreas.Select(sa => _mapper.Map<SubAreaResponseDto>(sa)).ToList();
            return ApiResponse<IReadOnlyList<SubAreaResponseDto>>.SuccessResponse(subAreasDto, "SubAreas retrieved successfully.");
        }

        public async Task<ApiResponse<IReadOnlyList<SubAreaResponseDto>>> GetByMainAreaIdAsync(int mainAreaId, SubAreaSearchParams searchParams)
        {
            var spec = new SubAreaSpecs(mainAreaId, searchParams);
            var subAreas = await _unitOfWork.Repository<SubArea>().GetAllWithSpecAsync(spec);

            var subAreasDto = subAreas.Select(sa => _mapper.Map<SubAreaResponseDto>(sa)).ToList();
            return ApiResponse<IReadOnlyList<SubAreaResponseDto>>.SuccessResponse(subAreasDto, "SubAreas retrieved successfully for the given MainArea.");
        }

        public async Task<ApiResponse<SubAreaResponseDto>> CreateAsync(CreateSubAreaDto createDto)
        {
            var mainArea = await _unitOfWork.Repository<MainArea>().GetByIdAsync(createDto.MainAreaId);
            if (mainArea == null)
                return ApiResponse<SubAreaResponseDto>.ErrorResponse($"MainArea with Id {createDto.MainAreaId} not found.");

            var subArea = _mapper.Map<SubArea>(createDto);
            _unitOfWork.Repository<SubArea>().Add(subArea);
            await _unitOfWork.CompleteAsync();

            var subAreaDto = _mapper.Map<SubAreaResponseDto>(subArea);
            subAreaDto.MainAreaName = mainArea.Name;

            return ApiResponse<SubAreaResponseDto>.SuccessResponse(subAreaDto, "SubArea created successfully.");
        }

        public async Task<ApiResponse<SubAreaResponseDto>> UpdateAsync(UpdateSubAreaDto updateDto)
        {
            var subArea = await _unitOfWork.Repository<SubArea>().GetByIdAsync(updateDto.Id);
            if (subArea == null)
                return ApiResponse<SubAreaResponseDto>.ErrorResponse($"SubArea with Id {updateDto.Id} not found.");

            if (subArea.MainAreaId != updateDto.MainAreaId)
            {
                var mainArea = await _unitOfWork.Repository<MainArea>().GetByIdAsync(updateDto.MainAreaId);
                if (mainArea == null)
                    return ApiResponse<SubAreaResponseDto>.ErrorResponse($"MainArea with Id {updateDto.MainAreaId} not found.");
            }

            _mapper.Map(updateDto, subArea);
            subArea.UpdatedDate = DateTime.UtcNow;

            _unitOfWork.Repository<SubArea>().Update(subArea);
            await _unitOfWork.CompleteAsync();

            var updatedSubArea = await _unitOfWork.Repository<SubArea>().GetByIdWithSpecAsync(new SubAreaSpecs(subArea.Id));
            var subAreaDto = _mapper.Map<SubAreaResponseDto>(updatedSubArea);

            return ApiResponse<SubAreaResponseDto>.SuccessResponse(subAreaDto, "SubArea updated successfully.");
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(int id)
        {
            var subArea = await _unitOfWork.Repository<SubArea>().GetByIdAsync(id);
            if (subArea == null)
                return ApiResponse<bool>.ErrorResponse($"SubArea with Id {id} not found.");

            _unitOfWork.Repository<SubArea>().Delete(subArea);
            await _unitOfWork.CompleteAsync();

            return ApiResponse<bool>.SuccessResponse(true, "SubArea deleted successfully.");
        }
    }
}
