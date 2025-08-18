using PowerStore.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerStore.Core.Contract.RideService_Contract
{
    public interface IRideRequestRepository : IGenaricRepositoy<RideRequests>
    {
        public Task<RideRequests?> GetActiveTripRequestForPassenger(string PassengerId);
        public Task<RideRequests?> GetActiveTripRequestForDriver(string driverId);
    }
}
