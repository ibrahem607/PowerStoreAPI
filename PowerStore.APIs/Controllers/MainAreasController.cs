using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PowerStore.Core.Contract;
using PowerStore.Core.Contract.Responses;
using PowerStore.Core.DTOs.MainAreaDtos;
using PowerStore.Core.EntitiesSpecifications;

namespace PowerStore.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainAreasController : ControllerBase
    {
        private readonly IMainAreaService _mainAreaService;

        public MainAreasController(IMainAreaService mainAreaService)
        {
            _mainAreaService = mainAreaService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<MainAreaResponseDto>>> GetById(int id)
        {
            var result = await _mainAreaService.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IReadOnlyList<MainAreaResponseDto>>>> GetAll([FromQuery] MainAreaSearchParams searchParams)
        {
            var result = await _mainAreaService.GetAllAsync(searchParams);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<MainAreaResponseDto>>> Create(CreateMainAreaDto createDto)
        {
            var result = await _mainAreaService.CreateAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut]
        public async Task<ActionResult<ApiResponse<MainAreaResponseDto>>> Update(UpdateMainAreaDto updateDto)
        {
            var result = await _mainAreaService.UpdateAsync(updateDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            var result = await _mainAreaService.SoftDeleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
