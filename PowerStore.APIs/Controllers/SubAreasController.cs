using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PowerStore.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubAreasController : ControllerBase
    {
        private readonly ISubAreaService _subAreaService;
        public SubAreasController(ISubAreaService subAreaService) => _subAreaService = subAreaService;

        // GET: api/subareas
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<SubAreaResponseDto>>> GetAll(
            [FromQuery] SubAreaSearchParams searchParams)
        {
            var subAreas = await _subAreaService.GetAllAsync(searchParams);
            return Ok(subAreas);
        }

        // GET: api/subareas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SubAreaResponseDto>> GetById(int id)
        {
            var subArea = await _subAreaService.GetByIdAsync(id);
            return Ok(subArea);
        }

        // GET: api/mainareas/5/subareas
        [HttpGet("~/api/mainareas/{mainAreaId}/subareas")]
        public async Task<ActionResult<IReadOnlyList<SubAreaResponseDto>>> GetByMainAreaId(
            int mainAreaId, [FromQuery] SubAreaSearchParams searchParams)
        {
            var subAreas = await _subAreaService.GetByMainAreaIdAsync(mainAreaId, searchParams);
            return Ok(subAreas);
        }

        // POST: api/subareas
        [HttpPost]
        public async Task<ActionResult<SubAreaResponseDto>> Create(CreateSubAreaDto createDto)
        {
            var createdSubArea = await _subAreaService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = createdSubArea.Id }, createdSubArea);
        }

        // PUT: api/subareas
        [HttpPut]
        public async Task<ActionResult<SubAreaResponseDto>> Update(UpdateSubAreaDto updateDto)
        {
            var updatedSubArea = await _subAreaService.UpdateAsync(updateDto);
            return Ok(updatedSubArea);
        }

        // DELETE: api/subareas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _subAreaService.SoftDeleteAsync(id);
            return NoContent();
        }
    }
