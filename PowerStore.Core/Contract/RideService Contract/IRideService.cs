using PowerStore.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerStore.Core.Contract.RideService_Contract
{
    public interface IRideService
    {
         RideRequests? GetActiveTripRequestForPassenger(string PassengerId);
         RideRequests? GetActiveTripRequestForDriver(string driverId);
        Task<IReadOnlyList<Driver>> GetNearbyDrivers(double pickuplat , double pickuplong , double radiusKm);
    }
}
