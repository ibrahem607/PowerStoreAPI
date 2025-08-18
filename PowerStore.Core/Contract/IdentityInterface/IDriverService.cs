using PowerStore.Core.Contract.Dtos.Identity;
using PowerStore.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PowerStore.Core.Contract.IdentityInterface
{
    public interface IDriverService
    {
        List<DriverDto> GetBy(Expression<Func<Driver, bool>> predicate);
        Task<List<DriverDto>> GetAllWithUser();
        int Add(DriverDto driver);
        Task<int> Update(DriverDto driver);
    }
}
