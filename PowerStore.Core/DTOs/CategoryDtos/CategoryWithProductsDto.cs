using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerStore.Core.DTOs.CategoryDtos
{
    public class CategoryWithProductsDto : CategoryResponseDto
    {
        public IReadOnlyList<CategoryProductDto> Products { get; set; } = new List<CategoryProductDto>();
    }

    public class CategoryProductDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal SalePrice { get; set; }
    }

}
