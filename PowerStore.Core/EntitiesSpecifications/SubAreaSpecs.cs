using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerStore.Core.EntitiesSpecifications
{
    public class SubAreaSpecs : BaseSpecifications<SubArea>
    {
        // Get SubArea by ID
        public SubAreaSpecs(int id) : base(sa => sa.Id == id && !sa.IsDeleted)
        {
            Includes.Add(sa => sa.MainArea);
        }

        // Get all SubAreas for a specific MainArea
        public SubAreaSpecs(int mainAreaId, SubAreaSearchParams searchParams)
            : base(sa => !sa.IsDeleted &&
                        sa.MainAreaId == mainAreaId &&
                        (string.IsNullOrEmpty(searchParams.Search) ||
                         sa.Name.Contains(searchParams.Search)))
        {
            Includes.Add(sa => sa.MainArea);

            // Apply ordering
            if (!string.IsNullOrEmpty(searchParams.Sort))
            {
                switch (searchParams.Sort.ToLower())
                {
                    case "namedesc":
                        AddOrderByDesc(sa => sa.Name);
                        break;
                    case "name":
                    default:
                        AddOrderBy(sa => sa.Name);
                        break;
                }
            }
            else
            {
                AddOrderBy(sa => sa.Name);
            }

            ApplyPaging(searchParams.PageSize * (searchParams.PageIndex - 1), searchParams.PageSize);
        }

        // Get all SubAreas with optional filtering
        public SubAreaSpecs(SubAreaSearchParams searchParams)
            : base(sa => !sa.IsDeleted &&
                        (string.IsNullOrEmpty(searchParams.Search) ||
                         sa.Name.Contains(searchParams.Search)))
        {
            Includes.Add(sa => sa.MainArea);

            if (!string.IsNullOrEmpty(searchParams.Sort))
            {
                switch (searchParams.Sort.ToLower())
                {
                    case "namedesc":
                        AddOrderByDesc(sa => sa.Name);
                        break;
                    case "mainarea":
                        AddOrderBy(sa => sa.MainArea!.Name);
                        break;
                    case "mainareadesc":
                        AddOrderByDesc(sa => sa.MainArea!.Name);
                        break;
                    case "name":
                    default:
                        AddOrderBy(sa => sa.Name);
                        break;
                }
            }
            else
            {
                AddOrderBy(sa => sa.Name);
            }

            ApplyPaging(searchParams.PageSize * (searchParams.PageIndex - 1), searchParams.PageSize);
        }
    }

    public class SubAreaSearchParams
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
        public int? MainAreaId { get; set; } // Optional filter by MainArea
    }
}
