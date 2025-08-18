using PowerStore.Core.Contract.RideService_Contract;
using PowerStore.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerStore.Core.Contract
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IGenaricRepositoy<T> Repositoy<T>() where T : BaseEntity;

        Task<int> CompleteAsync();
        int Complete();
        public IRideRepository RideRepository { get;  }


    }
}
