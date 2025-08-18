using PowerStore.Core.Contract;
using PowerStore.Core.Contract.RideService_Contract;
using PowerStore.Core.Entities;
using PowerStore.Infrastructer.Data.Context;
using PowerStore.Infrastructer.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerStore.Infrastructer.Data
{
    public class UnitOfwork : IUnitOfWork 
    {
        private readonly Hashtable _Repository;
        private readonly ApplicationDbContext _context;


        public IRideRepository RideRepository { get;  }

        public UnitOfwork(ApplicationDbContext context  , IRideRepository rideRepository)
        {
            _context = context;
            _Repository = new Hashtable();
            RideRepository = rideRepository;    
        }

        public IGenaricRepositoy<T> Repositoy<T>() where T : BaseEntity
        {
            var key = typeof(T).Name;  // driver
            if(!_Repository.ContainsKey(key))
            {
                var repo = new GenaricRepository<T>(_context);
                _Repository.Add(key, repo);
            }

            return _Repository[key] as IGenaricRepositoy<T>;
        }

        public async Task<int> CompleteAsync()
            => await _context.SaveChangesAsync();

        public int Complete()
            =>  _context.SaveChanges();

        public async ValueTask DisposeAsync()
            => await _context.DisposeAsync();

      
    }
}
