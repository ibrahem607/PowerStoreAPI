using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerStore.Core.Entities;
using PowerStore.Core.Specifications;

namespace PowerStore.Core.EntitiesSpecifications
{
    public class MainAreaSpecs : BaseSpecifications<MainArea>
    {
        // Specification for getting a MainArea by ID (includes soft delete check)
        public MainAreaSpecs(int id) : base(ma => ma.Id == id && !ma.IsDeleted)
        {
        }

        // Specification for getting all non-deleted MainAreas with optional search & pagination
        public MainAreaSpecs(MainAreaSearchParams searchParams)
            : base(ma => !ma.IsDeleted &&
                        (string.IsNullOrEmpty(searchParams.Search) ||
                         ma.Name.Contains(searchParams.Search)))
        {
            // Apply ordering
            if (!string.IsNullOrEmpty(searchParams.Sort))
            {
                switch (searchParams.Sort.ToLower())
                {
                    case "namedesc":
                        AddOrderByDesc(ma => ma.Name);
                        break;
                    case "name":
                    default:
                        AddOrderBy(ma => ma.Name);
                        break;
                }
            }
            else
            {
                AddOrderBy(ma => ma.Name); // Default order
            }

            // Apply pagination
            ApplyPaging(searchParams.PageSize * (searchParams.PageIndex - 1), searchParams.PageSize);
        }
    }
    public class MainAreaSearchParams
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
    }
}
