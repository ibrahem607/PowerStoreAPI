using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PowerStore.Core.Contract;
using PowerStore.Core.DTOs.ProductDtos;

namespace PowerStore.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService) => _productService = productService;

        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductResponseDto>>> GetAll(
            [FromQuery] ProductSearchParams searchParams)
        {
            var products = await _productService.GetAllAsync(searchParams);
            return Ok(products);
        }

        // GET: api/products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponseDto>> GetById(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            return Ok(product);
        }

        // GET: api/products/5/details
        [HttpGet("{id}/details")]
        public async Task<ActionResult<ProductDetailDto>> GetByIdWithDetails(int id)
        {
            var product = await _productService.GetByIdWithDetailsAsync(id);
            return Ok(product);
        }

        // GET: api/categories/5/products
        [HttpGet("~/api/categories/{categoryId}/products")]
        public async Task<ActionResult<IReadOnlyList<ProductResponseDto>>> GetByCategoryId(
            int categoryId, [FromQuery] ProductSearchParams searchParams)
        {
            var products = await _productService.GetByCategoryIdAsync(categoryId, searchParams);
            return Ok(products);
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<ProductResponseDto>> Create(CreateProductDto createDto)
        {
            var createdProduct = await _productService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
        }

        // PUT: api/products
        [HttpPut]
        public async Task<ActionResult<ProductResponseDto>> Update(UpdateProductDto updateDto)
        {
            var updatedProduct = await _productService.UpdateAsync(updateDto);
            return Ok(updatedProduct);
        }

        // DELETE: api/products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.SoftDeleteAsync(id);
            return NoContent();
        }

        // GET: api/products/5/profit-margin
        [HttpGet("{id}/profit-margin")]
        public async Task<ActionResult<decimal>> GetProfitMargin(int id)
        {
            var profitMargin = await _productService.CalculateProfitMarginAsync(id);
            return Ok(profitMargin);
        }
    }
}
