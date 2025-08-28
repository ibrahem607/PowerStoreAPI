using Microsoft.EntityFrameworkCore;
using PowerStore.Core.Entities;
using PowerStore.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerStore.Infrastructer
{
    public static class SpecificationEvaluators<T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecifications<T> spec)
        {
            var query = inputQuery;

            // Modify the query based on the spec
            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }

            // Includes (eager loading of related data)
            query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

            // Ordering
            if (spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }
            else if (spec.OrderByDesc != null)
            {
                query = query.OrderByDescending(spec.OrderByDesc);
            }

            // Pagination (NEW CODE)
            if (spec.IsPagingEnabled)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }

            return query;
        }
    }
}
