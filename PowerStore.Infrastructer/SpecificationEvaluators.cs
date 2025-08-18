﻿using Microsoft.EntityFrameworkCore;
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
        public static IQueryable<T> GetQuery(IQueryable<T> Inputquery , ISpecifications<T> spec)
        {
            var query = Inputquery; // _context.set<T>()

            if(spec.Criteria is not null)
                query = query.Where(spec.Criteria);   // _context.set<driver>.where(d => d.id == id)
            
            if(spec.OrderBy is not null)
                query = query.OrderBy(spec.OrderBy);
            
            if(spec.OrderByDesc is not null)
                query = query.OrderByDescending(spec.OrderByDesc);

            query = spec.Includes.Aggregate(query, (currentqeury, icludeEpressin) => currentqeury.Include(icludeEpressin));

            return query;
        }
    }
}
