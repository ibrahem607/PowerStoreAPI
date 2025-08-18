using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PowerStore.Core.Contract;
using PowerStore.Core.Contract.RideService_Contract;
using PowerStore.Core.Entities;
using PowerStore.Core.Specifications.DriverSpecifiactions;
using PowerStore.Infrastructer.Repositories.DriverRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerStore.Service._RideService
{
    public class RideService : IRideService
    {
        private readonly ILocationService _locationService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerFactory _loggerFactory;

        public RideService(IUnitOfWork unitOfWork, ILocationService locationService, ILoggerFactory loggerFactory)
        {
            _unitOfWork = unitOfWork;
            _locationService= locationService;
            _loggerFactory = loggerFactory;
        }


        public async Task<IReadOnlyList<Driver>> GetNearbyDrivers(double pickuplat, double pickuplong, double radiusKm)
        {
            var spec = new DriverWithApplicationUserSpecifiaction();
            var AllDrivers = await _unitOfWork.Repositoy<Driver>().GetAllWithSpecAsync(spec);
            return AllDrivers.Where(d =>
            {
            
                var lastRide = d.Rides.OrderByDescending(r => r.Id).FirstOrDefault();
                if (lastRide == null) return false; 

                // Calculate distance using Haversine formula
                return _locationService.HaversineDistance(pickuplat, lastRide.DestinationLocation.Latitude, pickuplong , lastRide.DestinationLocation.Longitude) <= radiusKm;
            }).ToList();
        }

        public RideRequests? GetActiveTripRequestForPassenger(string PassengerId)
        {
            // Conditions for active trip_request request: -
            // 1. driver is found, but trip_request not started yet!
            // 2. driver is not found yet, but trip_request request is last updated is less than one minute ago!

            try
            {
                var OneMinuteAgo = DateTime.Now.AddMinutes(-1);

                var RideReuest =  _unitOfWork.Repositoy<RideRequests>().GetBy(r => r.PassengerId == PassengerId &&
                (r.Status > RideRequestStatus.NO_DRIVER_FOUND && r.Status < RideRequestStatus.TRIP_STARTED) ||
                (r.Status == RideRequestStatus.NO_DRIVER_FOUND && r.LastModifiedAt < OneMinuteAgo));

                return RideReuest.FirstOrDefault();
            }
            catch (Exception ex)
            {
                var logger = _loggerFactory.CreateLogger<RideRequestRepository>();
                logger.LogError($"{nameof(GetActiveTripRequestForPassenger)} threw an exception: {ex}");
                throw;
            }
        }

        public  RideRequests? GetActiveTripRequestForDriver(string driverId)
        {
            try
            {
                var RideReuest =  _unitOfWork.Repositoy<RideRequests>().GetBy(r => r.DriverId == driverId
                && (r.Status > RideRequestStatus.NO_DRIVER_FOUND && r.Status < RideRequestStatus.TRIP_STARTED));

                return RideReuest.FirstOrDefault(); ;
            }
            catch (Exception ex)
            {
                var logger = _loggerFactory.CreateLogger<RideRequestRepository>();
                logger.LogError($"{nameof(GetActiveTripRequestForDriver)} threw an exception: {ex}");
                throw;
            }
        }
    }
}
