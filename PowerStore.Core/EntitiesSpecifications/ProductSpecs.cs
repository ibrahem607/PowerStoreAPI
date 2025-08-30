using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerStore.Core.DTOs.ProductDtos;
using PowerStore.Core.Entities;
using PowerStore.Core.Specifications;

namespace PowerStore.Core.EntitiesSpecifications
{
    public class ProductSpecs : BaseSpecifications<Product>
    {
        // Get Product by ID
        public ProductSpecs(int id) : base(p => p.Id == id && !p.IsDeleted)
        {
            Includes.Add(p => p.Category);
        }

        // Get all Products with filtering
        public ProductSpecs(ProductSearchParams searchParams)
            : base(p => !p.IsDeleted &&
                       (string.IsNullOrEmpty(searchParams.Search) ||
                        p.ProductName.Contains(searchParams.Search)) &&
                       (!searchParams.CategoryId.HasValue || p.CategoryId == searchParams.CategoryId) &&
                       (!searchParams.MinPrice.HasValue || p.SalePrice >= searchParams.MinPrice) &&
                       (!searchParams.MaxPrice.HasValue || p.SalePrice <= searchParams.MaxPrice))
        {
            if (searchParams.IncludeCategory)
            {
                Includes.Add(p => p.Category);
            }

            // Apply ordering
            if (!string.IsNullOrEmpty(searchParams.Sort))
            {
                switch (searchParams.Sort.ToLower())
                {
                    case "namedesc":
                        AddOrderByDesc(p => p.ProductName);
                        break;
                    case "price":
                        AddOrderBy(p => p.SalePrice);
                        break;
                    case "pricedesc":
                        AddOrderByDesc(p => p.SalePrice);
                        break;
                    case "profit":
                        AddOrderBy(p => p.SalePrice - p.PurchasePrice);
                        break;
                    case "profitdesc":
                        AddOrderByDesc(p => p.SalePrice - p.PurchasePrice);
                        break;
                    case "category":
                        AddOrderBy(p => p.Category.Name);
                        break;
                    case "name":
                    default:
                        AddOrderBy(p => p.ProductName);
                        break;
                }
            }
            else
            {
                AddOrderBy(p => p.ProductName);
            }

            ApplyPaging(searchParams.PageSize * (searchParams.PageIndex - 1), searchParams.PageSize);
        }

        // Get products by category
        public ProductSpecs(int categoryId, ProductSearchParams searchParams)
            : base(p => !p.IsDeleted &&
                       p.CategoryId == categoryId &&
                       (string.IsNullOrEmpty(searchParams.Search) ||
                        p.ProductName.Contains(searchParams.Search)))
        {
            Includes.Add(p => p.Category);

            if (!string.IsNullOrEmpty(searchParams.Sort))
            {
                switch (searchParams.Sort.ToLower())
                {
                    case "namedesc":
                        AddOrderByDesc(p => p.ProductName);
                        break;
                    case "price":
                        AddOrderBy(p => p.SalePrice);
                        break;
                    case "pricedesc":
                        AddOrderByDesc(p => p.SalePrice);
                        break;
                    case "name":
                    default:
                        AddOrderBy(p => p.ProductName);
                        break;
                }
            }
            else
            {
                AddOrderBy(p => p.ProductName);
            }

            ApplyPaging(searchParams.PageSize * (searchParams.PageIndex - 1), searchParams.PageSize);
        }
    }
}
