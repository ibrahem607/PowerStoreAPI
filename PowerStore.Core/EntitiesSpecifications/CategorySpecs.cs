using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerStore.Core.Entities;
using PowerStore.Core.Specifications;

namespace PowerStore.Core.EntitiesSpecifications
{
    public class CategorySpecs : BaseSpecifications<Category>
    {
        // Get Category by ID
        public CategorySpecs(int id) : base(c => c.Id == id && !c.IsDeleted)
        {
            Includes.Add(c => c.Products);
        }

        // Get all Categories with optional search & pagination
        public CategorySpecs(CategorySearchParams searchParams)
            : base(c => !c.IsDeleted &&
                       (string.IsNullOrEmpty(searchParams.Search) ||
                        c.Name.Contains(searchParams.Search)))
        {
            if (searchParams.IncludeProducts)
            {
                Includes.Add(c => c.Products);
            }

            // Apply ordering
            if (!string.IsNullOrEmpty(searchParams.Sort))
            {
                switch (searchParams.Sort.ToLower())
                {
                    case "namedesc":
                        AddOrderByDesc(c => c.Name);
                        break;
                    case "productcount":
                        AddOrderBy(c => c.Products.Count);
                        break;
                    case "productcountdesc":
                        AddOrderByDesc(c => c.Products.Count);
                        break;
                    case "name":
                    default:
                        AddOrderBy(c => c.Name);
                        break;
                }
            }
            else
            {
                AddOrderBy(c => c.Name);
            }

            ApplyPaging(searchParams.PageSize * (searchParams.PageIndex - 1), searchParams.PageSize);
        }
    }

    public class CategorySearchParams
    {
        private const int MaxPageSize = 50;
        public int PageIndex { get; set; } = 1;
        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
        public string? Sort { get; set; }
        public string? Search { get; set; }
        public bool IncludeProducts { get; set; } = false;
    }
}
