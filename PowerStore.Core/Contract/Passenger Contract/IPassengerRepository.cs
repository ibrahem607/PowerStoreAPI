﻿using PowerStore.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerStore.Core.Contract.Passenger_Contract
{
    public interface IPassengerRepository : IGenaricRepositoy<Passenger>
    {
        Task GetByIdAsync(string id);
        Task<Passenger?> GetByUserId(string userId);
   
     
    }
}
