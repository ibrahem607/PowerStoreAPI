using Microsoft.EntityFrameworkCore;
using PowerStore.Core.Contract;
using PowerStore.Core.Entities;
using PowerStore.Core.Specifications;
using PowerStore.Infrastructer.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PowerStore.Infrastructer.Repositories
{
    public class GenaricRepository<T> : IGenaricRepositoy<T> where T : BaseEntity
    {
        private protected readonly ApplicationDbContext _context;

        public GenaricRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void  Add(T model)
            => _context.Set<T>().Add(model);

        public void Delete(T model)
            => _context.Set<T>().Remove(model);


        public async Task<List<T>> GetAll(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>().AsNoTracking();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync();
        }

        public List<T> GetBy(
    Expression<Func<T, bool>> query,
    params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> dbQuery = _context.Set<T>();

            // Apply includes if provided
            foreach (var include in includes)
            {
                dbQuery = dbQuery.Include(include);
            }

            return dbQuery.Where(query).ToList();
        }

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec)
        {
            return await ApplySpecification(spec).AsNoTracking().ToListAsync();
        }


        public async Task<T?> GetByIdAsync(int id)
           => await _context.Set<T>().FindAsync(id);

        public async Task<T?> GetByIdWithSpecAsync(ISpecifications<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        public async Task<T?> Find(Expression<Func<T,bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).FirstOrDefaultAsync();
        }

        public async Task<T?> GetDriverOrPassengerByIdAsync(string Id)
            => await _context.Set<T>().FindAsync(Id);


        public void Update(T model)
            => _context.Set<T>().Update(model);


        private IQueryable<T> ApplySpecification(ISpecifications<T> spec)
        {
            return SpecificationEvaluators<T>.GetQuery(_context.Set<T>(), spec);
        }


    }
}
