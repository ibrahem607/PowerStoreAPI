using PowerStore.Core.Entities;
using PowerStore.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PowerStore.Core
{
    public interface IGenaricRepositoy<T> where T : BaseEntity
    {
        void Add(T model);

        void Update(T model);
        void Delete(T model);
        List<T> GetBy(
    Expression<Func<T, bool>> query,
    params Expression<Func<T, object>>[] includes);
        Task<List<T>> GetAll(params Expression<Func<T, object>>[] includes);
        Task<T?> GetByIdAsync(int id);
        Task<T?> Find(Expression<Func<T, bool>> predicate);
        Task<T?> GetDriverOrPassengerByIdAsync(string Id);
        Task<T?> GetByIdWithSpecAsync(ISpecifications<T> spec);

        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec);
    }
}
