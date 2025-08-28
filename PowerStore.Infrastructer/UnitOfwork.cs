﻿using PowerStore.Core;
using PowerStore.Core.Entities;
using PowerStore.Infrastructer.Data.Context;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerStore.Infrastructer
{
    public class UnitOfwork : IUnitOfWork 
    {
        private readonly Hashtable _Repository;
        private readonly ApplicationDbContext _context;



        public UnitOfwork(ApplicationDbContext context )
        {
            _context = context;
            _Repository = new Hashtable();
        }

        public IGenaricRepositoy<T> Repository<T>() where T : BaseEntity
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
