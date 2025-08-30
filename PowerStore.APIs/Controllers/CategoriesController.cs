using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PowerStore.Core.Contract;
using PowerStore.Core.DTOs.CategoryDtos;
using PowerStore.Core.EntitiesSpecifications;

namespace PowerStore.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService) => _categoryService = categoryService;

        // GET: api/categories
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<CategoryResponseDto>>> GetAll(
            [FromQuery] CategorySearchParams searchParams)
        {
            var categories = await _categoryService.GetAllAsync(searchParams);
            return Ok(categories);
        }

        // GET: api/categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryResponseDto>> GetById(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            return Ok(category);
        }

        // GET: api/categories/5/with-products
        [HttpGet("{id}/with-products")]
        public async Task<ActionResult<CategoryWithProductsDto>> GetByIdWithProducts(int id)
        {
            var category = await _categoryService.GetByIdWithProductsAsync(id);
            return Ok(category);
        }

        // POST: api/categories
        [HttpPost]
        public async Task<ActionResult<CategoryResponseDto>> Create(CreateCategoryDto createDto)
        {
            var createdCategory = await _categoryService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = createdCategory.Id }, createdCategory);
        }

        // PUT: api/categories
        [HttpPut]
        public async Task<ActionResult<CategoryResponseDto>> Update(UpdateCategoryDto updateDto)
        {
            var updatedCategory = await _categoryService.UpdateAsync(updateDto);
            return Ok(updatedCategory);
        }

        // DELETE: api/categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoryService.SoftDeleteAsync(id);
            return NoContent();
        }
    }
}
