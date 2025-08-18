﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PowerStore.Core.Contract.RideService_Contract;
using PowerStore.Core.Entities;
using PowerStore.Infrastructer.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerStore.Infrastructer.Repositories.DriverRepository
{
    public class RideRequestRepository : GenaricRepository<RideRequests>
    {
        private readonly ILoggerFactory _loggerFactory;

        public RideRequestRepository(ApplicationDbContext dbContext , ILoggerFactory loggerFactory)
            :base(dbContext)
        {
            _loggerFactory = loggerFactory;
        }

        public async Task<RideRequests?> GetActiveTripRequestForPassenger(string PassengerId)
        {
            // Conditions for active trip_request request: -
            // 1. driver is found, but trip_request not started yet!
            // 2. driver is not found yet, but trip_request request is last updated is less than one minute ago!

            try
            {
                var OneMinuteAgo = DateTime.Now.AddMinutes(-1);

                var RideReuest =  await _context.RideRequests.Where(r => r.PassengerId == PassengerId &&
                (r.Status > RideRequestStatus.NO_DRIVER_FOUND && r.Status < RideRequestStatus.TRIP_STARTED) ||
                (r.Status == RideRequestStatus.NO_DRIVER_FOUND && r.LastModifiedAt < OneMinuteAgo )).SingleOrDefaultAsync();

                return RideReuest;
            }
            catch (Exception ex)
            {
                var logger = _loggerFactory.CreateLogger<RideRequestRepository>();
                logger.LogError($"{nameof(GetActiveTripRequestForPassenger)} threw an exception: {ex}");
                throw;
            }
        }

        public async Task<RideRequests?> GetActiveTripRequestForDriver(string driverId)
        {
            try
            {
                var RideReuest = await _context.Set<RideRequests>().Where(r => r.DriverId == driverId
                && (r.Status > RideRequestStatus.NO_DRIVER_FOUND && r.Status < RideRequestStatus.TRIP_STARTED)).SingleOrDefaultAsync();

                return RideReuest;
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
