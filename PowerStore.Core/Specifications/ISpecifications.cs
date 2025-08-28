using PowerStore.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PowerStore.Core.Specifications
{
    public interface ISpecifications<T> where T : BaseEntity
    {
        int Skip { get; }
        int Take { get; }
        bool IsPagingEnabled { get; }
        public Expression<Func<T, bool>>? Criteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; set; }
        public Expression<Func<T,object>> OrderBy { get; set; }
        public Expression<Func<T,object>> OrderByDesc { get; set; }
    }
}
