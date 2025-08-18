﻿using PowerStore.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerStore.Core.Contract.RideService_Contract
{
    public interface IRideRepository
    {
        Task<Ride?> GetActiveTripForPassenger(string PassengerId);
        Task<Ride?> GetActiveTripForDriver(string driverId);
        Task<Ride?> GetTripForPassengerWithPendingPayment(int RideId, string PassengerId);
        Task<int> GetRidesCountForPassenger(string PassengerId);
        Task<int> GetRidesCountForDriver(string driverId);
   
      
    }
}
