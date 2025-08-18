﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PowerStore.APIs.Hubs;
using PowerStore.Core.Contract;
using PowerStore.Core.Contract.Dtos;
using PowerStore.Core.Contract.Dtos.Rides;
using PowerStore.Core.Contract.Errors;
using PowerStore.Core.Contract.IdentityInterface;
using PowerStore.Core.Entities;
using System.Security.Claims;
using DataResponse = PowerStore.Core.Contract.Dtos.ApiToReturnDtoResponse.DataResponse;

namespace PowerStore.APIs.Controllers
{

    public class RideController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHubContext<RideHub> _hubContext;
        private readonly ILocationService _locationService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDriverService _driverService;
        private const string Driver = "Driver";
        public RideController(IUnitOfWork unitOfWork
            , IMapper mapper,
            IHubContext<RideHub> hubContext,
            UserManager<ApplicationUser> userManager
            , IDriverService driverRepository, ILocationService locationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hubContext = hubContext;
            _userManager = userManager;
            _driverService = driverRepository;
            _locationService = locationService;
        }

        [Authorize(Roles = Driver)]
        [HttpPut("{tripRequestId}/start-trip")]
        public async Task<ActionResult> StartTrip(int tripRequestId)
        {
            // get driver      enhansement
            var phoneNumber = User.FindFirstValue(ClaimTypes.MobilePhone);
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
            if (user is null) return BadRequest(new ApiResponse(401));


            // Step 1: check valid ride request exists
            var rideReqeust = await _unitOfWork.Repositoy<RideRequests>().GetByIdAsync(tripRequestId);
            if (rideReqeust is null) return BadRequest(new ApiResponse(400, "Ride Request is not exist."));

            // Step 2: check driver exists
            var DriverData = _driverService.GetBy(x => x.UserId == user.Id);
            var driver = DriverData.FirstOrDefault();
            if (driver is null) return BadRequest(new ApiResponse(400, "Driver is not exist."));

            // Step 3: check driver has ongoing trips
            var rides = await _unitOfWork.RideRepository.GetActiveTripForDriver(driver.Id);
            if (rides is not null) return BadRequest(new ApiResponse(400, "Driver has ongoing trips"));

            // ** Security check !
            if (rideReqeust.Id != tripRequestId)
                return BadRequest(new ApiResponse(400, "Active trip request for driver does not match !!"));

            // Step 4: prepare entity  TODO

            rideReqeust.Status = RideRequestStatus.TRIP_STARTED;
            // Step 5: create trip entity  TODO using mapper
            var RideEntity = new Ride
            {

            };

            // Step 6: perform database operations
            rideReqeust.LastModifiedAt = DateTime.Now;
            _unitOfWork.Repositoy<RideRequests>().Update(rideReqeust);

            _unitOfWork.Repositoy<Ride>().Add(RideEntity);

            var count = await _unitOfWork.CompleteAsync();
            if (count <= 0) return BadRequest(new ApiResponse(400, "That error when commeted changed in database"));

            return Ok();

        }

        [Authorize(Roles = Driver)]
        [HttpPut("{rideId}/end-ride")]
        public async Task<ActionResult<ApiToReturnDtoResponse>> EndTrip(int rideId)
        {
            // step 1: get user --> driver 
            var phoneNumber = User.FindFirstValue(ClaimTypes.MobilePhone);
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
            if (user is null) return BadRequest(new ApiResponse(401));

            // step 2 : get driver 
            var DriverData =  _driverService.GetBy(x => x.UserId == user.Id);
            var driver = DriverData.FirstOrDefault();
            if (driver is null) return BadRequest(new ApiResponse(400, "The Driver not found"));

            // check driver has ongoing trips 
            var rides = await _unitOfWork.RideRepository.GetActiveTripForDriver(driver.Id);
            if (rides is null) return BadRequest(new ApiResponse(400, "Not found Ongoing trips"));

            if (rideId != rides.Id)
                return BadRequest(new ApiResponse(400, "Rides dosn't match."));

            // check status validations TODO 

            // update sattus for rides
            rides.Status = RideStatus.Completed;
            rides.LastModifiedAt = DateTime.Now;

            _unitOfWork.Repositoy<Ride>().Update(rides);

            var count = await _unitOfWork.CompleteAsync();
            if (count <= 0)
                return BadRequest(new ApiResponse(400, "That error when update entity in database."));

            return Ok();

        }

        [HttpPost("calc_price_time_destance")]
        public async Task<ActionResult<ApiToReturnDtoResponse>> CalcPriceAndTimeAndDestance(calculatePriceAnddectaceDto CalcDto)
        {
            var result = await _locationService.CalculateDestanceAndTimeAndPrice(CalcDto.PickUpLat, CalcDto.PickUpLon, CalcDto.DroppOffLat, CalcDto.DroppOffLon, CalcDto.CategoryId);

            return (new ApiToReturnDtoResponse
            {
                Data = new DataResponse
                {
                    Mas = $"The Price and Time Calculated by Distance and Category [{result.category}]",
                    StatusCode = StatusCodes.Status200OK,
                    Body = new ReturnCalcDto()
                    {
                        Price = result.price,
                        Distance = result.distance,
                        Time = result.estimatedTime
                    }
                }
            });
        }


    }
}
