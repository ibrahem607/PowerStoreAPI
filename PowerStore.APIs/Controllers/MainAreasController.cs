using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PowerStore.Core.Contract;
using PowerStore.Core.DTOs.MainAreaDtos;
using PowerStore.Core.EntitiesSpecifications;

namespace PowerStore.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainAreasController : ControllerBase
    {
        private readonly IMainAreaService _mainAreaService;
        public MainAreasController(IMainAreaService mainAreaService) => _mainAreaService = mainAreaService;

        [HttpGet("{id}")]
        public async Task<ActionResult<MainAreaResponseDto>> GetById(int id)
        {
            var mainArea = await _mainAreaService.GetByIdAsync(id);
            return Ok(mainArea);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<MainAreaResponseDto>>> GetAll([FromQuery] MainAreaSearchParams searchParams)
        {
            var mainAreas = await _mainAreaService.GetAllAsync(searchParams);
            return Ok(mainAreas);
        }

        [HttpPost]
        public async Task<ActionResult<MainAreaResponseDto>> Create(CreateMainAreaDto createDto)
        {
            var createdMainArea = await _mainAreaService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = createdMainArea.Id }, createdMainArea);
        }

        [HttpPut]
        public async Task<ActionResult<MainAreaResponseDto>> Update(UpdateMainAreaDto updateDto)
        {
            var updatedMainArea = await _mainAreaService.UpdateAsync(updateDto);
            return Ok(updatedMainArea);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mainAreaService.SoftDeleteAsync(id);
            return NoContent(); // 204 Success, no content to return
        }
    }
}
