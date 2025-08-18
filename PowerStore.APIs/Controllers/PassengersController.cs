using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PowerStore.APIs.Hubs;
using PowerStore.Core.Contract;
using PowerStore.Core.Contract.Dtos;
using PowerStore.Core.Contract.Dtos.Passenger;
using PowerStore.Core.Contract.Dtos.RideRequest;
using PowerStore.Core.Contract.Dtos.Rides;
using PowerStore.Core.Contract.Errors;
using PowerStore.Core.Contract.IdentityInterface;
using PowerStore.Core.Contract.Passenger_Contract;
using PowerStore.Core.Contract.RideService_Contract;
using PowerStore.Core.Entities;
using PowerStore.Core.Specifications.BidSpecifications;
using PowerStore.Core.Specifications.PassengerSpecifiactions;
using PowerStore.Infrastructer.Data.Context;
using PowerStore.Infrastructer.Document;
using PowerStore.Service.Identity;
using System.Security.Claims;
using DataResponse = PowerStore.Core.Contract.Dtos.ApiToReturnDtoResponse.DataResponse;

namespace PowerStore.APIs.Controllers
{

    public class PassengersController : BaseApiController
    {
        private const string passenger = "passenger";
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRideService _rideService;
        private readonly IPassengerService _passengerService;
        private readonly IHubContext<RideHub> _hubContext;

        public PassengersController(UserManager<ApplicationUser> userManager
                                    , SignInManager<ApplicationUser> signInManager
                                    , IMapper mapper
                                    , IUnitOfWork unitOfWork,
                                     IRideService rideService,
                                     IPassengerService passengerService,
                                     IHubContext<RideHub> hubContext)
        {

            _userManager = userManager;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _rideService = rideService;
            _passengerService = passengerService;
            _hubContext = hubContext;
        }


        [Authorize(Roles = passenger)]
        [HttpGet("profile")]
        public async Task<ActionResult<ApiToReturnDtoResponse>> GetSpecPassengers()
        {
            var UserPhoneNumber = User.FindFirstValue(ClaimTypes.MobilePhone);

            var GetUserByPhone = await _userManager.Users.FirstOrDefaultAsync(U => U.PhoneNumber == UserPhoneNumber);

            if (GetUserByPhone is null) return BadRequest(400);
            var result = _mapper.Map<ProfileDto>(GetUserByPhone);

            var response = new ApiToReturnDtoResponse
            {
                Data = new DataResponse
                {
                    Mas = "The Passenger Data",
                    StatusCode = StatusCodes.Status200OK,
                    Body = result

                }
            };

            return Ok(response);

        }
        [Authorize(Roles = passenger)]
        [HttpGet("Passenger_History_Ride")]
        public async Task<ActionResult<ApiToReturnDtoResponse>> GetPassengerHistoryRide()
        {
            var PhoneNumber = User.FindFirstValue(ClaimTypes.MobilePhone);
            var UserByPhoneNumber = await _userManager.Users.FirstOrDefaultAsync(_ => _.PhoneNumber == PhoneNumber);

            if (UserByPhoneNumber is null) return NotFound(new ApiResponse(404, "This User Not Found"));

            var passenger = _passengerService.GetByUserId(UserByPhoneNumber.Id);
            if (passenger is null) return NotFound(new ApiResponse(404, "This Passenger Not Found"));

            var spec = new PassengerWithRideAndRatingSpecifications(passenger.Id);
            var Ride = await _unitOfWork.Repositoy<Ride>().GetAllWithSpecAsync(spec);

            if (Ride.Count == 0) return NotFound(new ApiResponse(404, "Not Found any Ride For This Passenger"));

            var response = new ApiToReturnDtoResponse
            {
                Data = new DataResponse
                {
                    Mas = "The Passenger Data",
                    StatusCode = StatusCodes.Status200OK,
                    Body = Ride

                }
            };

            return Ok(response);

        }

        [Authorize(Roles = passenger)]
        [HttpPost("Update_for_Passenger")]
        public async Task<ActionResult<ApiToReturnDtoResponse>> UpdateSpecPassengers([FromForm] updateUserDto model)
        {
            var UserPhoneNumber = User.FindFirstValue(ClaimTypes.MobilePhone);

            var GetUserByPhone = await _userManager.Users.FirstOrDefaultAsync(U => U.PhoneNumber == UserPhoneNumber);

            if (GetUserByPhone is null) return BadRequest(400);

            GetUserByPhone.FullName = model.FullName;
            GetUserByPhone.Email = model.Email;
            GetUserByPhone.Gender = model.Gender;
            GetUserByPhone.DateOfBirth = model.DataOfBirth;
            GetUserByPhone.ProfilePictureUrl = DocumentSettings.UploadFile(model.UploadFile, "ProfilePicture");


            var result = await _userManager.UpdateAsync(GetUserByPhone);
            if (!result.Succeeded)
                return Ok(new ApiValidationResponse() { Errors = result.Errors.Select(E => E.Description) });

            var mappedResult = _mapper.Map<ProfileDto>(GetUserByPhone);

            var response = new ApiToReturnDtoResponse
            {
                Data = new DataResponse
                {
                    Mas = "Update Passenger Data Succ",
                    StatusCode = StatusCodes.Status200OK,
                    Body = mappedResult

                }
            };

            return Ok(response);

        }



        [Authorize(Roles = passenger)]
        [HttpPost("Cancel_Ride_Request")]
        public async Task<ActionResult<ApiToReturnDtoResponse>> CancelRideRequest(CancelRideRequestDto model)
        {
            var userPhoneNumber = User.FindFirstValue(ClaimTypes.MobilePhone);
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == userPhoneNumber);

            var passenger =  _passengerService.GetByUserId(user.Id);
            if (passenger is null) return BadRequest(new ApiResponse(400, "The Passenger is not Exist."));


            // 2- check passenger has ongoing trip request 
            var RideRequest = await _unitOfWork.Repositoy<RideRequests>().GetByIdAsync(model.RideRequestsId);
            if (RideRequest is null)
                return NotFound(new ApiResponse(404, "The Ride Request is not found."));

            var rideRequest = _rideService.GetActiveTripRequestForPassenger(passenger.Id);
            if (rideRequest is null) return BadRequest(new ApiResponse(400, "Customer Not Exist in This request trip."));


            if (RideRequest.Status == RideRequestStatus.TRIP_STARTED || RideRequest.Status == RideRequestStatus.CUSTOMER_CANCELED)
                return NotFound(new ApiResponse(404, "Cannot cancel This trip that has already started OR Canceled."));

            if (rideRequest.Id != model.RideRequestsId)
                return BadRequest(new ApiResponse(400, "The Ride Request Not Match"));

            RideRequest.Status = RideRequestStatus.CUSTOMER_CANCELED;
            RideRequest.LastModifiedAt = DateTime.Now;

            _unitOfWork.Repositoy<RideRequests>().Update(RideRequest);

            var count = await _unitOfWork.CompleteAsync();
            if (count <= 0) return BadRequest(new ApiResponse(400));

            var response = new ApiToReturnDtoResponse
            {
                Data = new DataResponse
                {
                    Mas = "Cancel TripRequest Succussed",
                    StatusCode = StatusCodes.Status200OK,

                }
            };

            return Ok(response);

        }
        [Authorize(Roles = passenger)]
        [HttpPut("Reject_Offer_By_Passenger")]
        public async Task<ActionResult<ApiToReturnDtoResponse>> RejectOfferByPassenger(RejectBidRequestDto rejectBidDto)
        {
            //1. Get User and Check is Exist or not 
            var UserPhoneNumber = User.FindFirstValue(ClaimTypes.MobilePhone);
            var user = await _userManager.Users.FirstOrDefaultAsync(U => U.PhoneNumber == UserPhoneNumber);

            var passenger =  _passengerService.GetByUserId(user.Id);
            if (passenger is null)
                return NotFound(new ApiResponse(404, "The Passenger Not Found"));

            //2. Check passenger has ongoing Ride Request 
            var requestedTrip = _rideService.GetActiveTripRequestForPassenger(passenger.Id);
            if (requestedTrip is null)
                return BadRequest(new ApiResponse(400, "Customer has no pending requested trip."));

            //3. Get Bid
            var spec = new BidWithRideRequestSpecifications(rejectBidDto.BidId);
            var bid = await _unitOfWork.Repositoy<Bid>().GetByIdWithSpecAsync(spec);
            if (bid is null)
                return BadRequest(new ApiResponse(400, "The Select Bid Is Not Found "));

            // trip request is invalid/expired if trip request is older than 1 minute TODO
            var onminutesAgo = DateTime.Now.AddMinutes(-1);
            if (bid.Ride.LastModifiedAt < onminutesAgo) return BadRequest(new ApiResponse(400, "Ride Reuest is expired."));

            //Check BidStatus
            if (bid.BidStatus == BidStatus.Rejected)
                return BadRequest(new ApiResponse(400, "This Bid Is Rejected"));

            // Update
            bid.BidStatus = BidStatus.Rejected;

            _unitOfWork.Repositoy<Bid>().Update(bid);
            var count = await _unitOfWork.CompleteAsync();
            if (count <= 0) return BadRequest(new ApiResponse(400, "The error when save change in database"));

            //Notification This Driver 
            // step 7: Notify the passenger 
            var DriverId = bid.DriverId;

            await _hubContext.Clients.User(DriverId).SendAsync("RejectedBid", new
            {
                Message = "This Offer Rejected",
            });

            return Ok(new ApiToReturnDtoResponse
            {
                Data = new DataResponse
                {
                    Mas = "The Bid Rejected and Notification the Driver",
                    StatusCode = StatusCodes.Status200OK,
                }
            });

        }

        [Authorize(Roles = passenger)]
        [HttpPut("Cancel_Ride_By_Passenger")]
        public async Task<ActionResult<ApiToReturnDtoResponse>> CancelRideByPassenger()
        {

            var phoneNumber = User.FindFirstValue(ClaimTypes.MobilePhone);
            if (string.IsNullOrEmpty(phoneNumber))
                return Unauthorized(new ApiResponse(401, "Phone number is missing or invalid"));

            var UserByPhoneNumber = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
            if (UserByPhoneNumber is null)
                return Unauthorized(new ApiResponse(401, "Unauthorized: User not found"));

            var Passenger =  _passengerService.GetByUserId(UserByPhoneNumber.Id);
            if (Passenger == null) return NotFound(new ApiResponse(404, "Passenger Not Found"));

            var ride = await _unitOfWork.RideRepository.GetActiveTripForPassenger(Passenger.Id);
            if (ride == null) return NotFound(new ApiResponse(404, "Not found Ongoing trips"));

            if (ride.Status != RideStatus.CanceledByDriver
                && ride.Status != RideStatus.Completed
                && ride.Status != RideStatus.WAITING_FOR_PAYMENT) return BadRequest(new ApiResponse(400, "Can Not Canceled Ride "));


            ride.Status = RideStatus.CanceledByPassenger;
            ride.LastModifiedAt = DateTime.Now;

            _unitOfWork.Repositoy<Ride>().Update(ride);

            var count = await _unitOfWork.CompleteAsync();
            if (count <= 0)
                return BadRequest(new ApiResponse(400, "That error when update entity in database."));

            //Notification This Driver 
            // step 7: Notify the passenger 
            var DriverId = ride.DriverId;

            await _hubContext.Clients.User(DriverId).SendAsync("CanceledRideByPassenger", new
            {
                Message = "This Ride Canceled",
            });

            return Ok(new ApiToReturnDtoResponse
            {
                Data = new DataResponse
                {
                    Mas = "The Ride Canceled and Notification the Driver",
                    StatusCode = StatusCodes.Status200OK,
                }
            });
        }

        [Authorize(Roles = passenger)]
        [HttpPost("RatingRideByPassenger")]
        public async Task<ActionResult<ApiToReturnDtoResponse>> RatingRideByPassenger(RatingPassgengerDto model)
        {
            var phoneNumber = User.FindFirstValue(ClaimTypes.MobilePhone);
            var GetUserByPhone = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);

            var passenger =  _passengerService.GetByUserId(GetUserByPhone.Id);

            if (passenger == null) return NotFound(new ApiResponse(404, "Passenger Not Found"));

            var ride = await _unitOfWork.RideRepository.GetActiveTripForPassenger(passenger.Id);

            if (ride is null) return NotFound(new ApiResponse(404, "The passenger Is Not Going Ride"));

            if (ride.Status != RideStatus.Completed)
                return BadRequest(new ApiResponse(400, "ride Not Completed Yet"));

            if (!(0 <= model.Score && model.Score <= 5))
                return BadRequest(new ApiResponse(400, "Score Out Of Range"));

            var passengerRating = new PassengerRating()
            {
                RideId = ride.Id,
                PassengerId = passenger.Id,
                Score = model.Score,
                Review = model.Review,
            };
            _unitOfWork.Repositoy<PassengerRating>().Add(passengerRating);

            var count = await _unitOfWork.CompleteAsync();
            if (count <= 0)
                return BadRequest(new ApiResponse(400, "That error when update entity in database."));

            return Ok(new ApiToReturnDtoResponse
            {
                Data = new DataResponse
                {
                    Mas = "The Ride Rating By Passenger ",
                    StatusCode = StatusCodes.Status200OK,
                }
            });
        }
        // GET: api/Passenger
        [HttpGet("GetAllPassengers")]
        public async Task<ActionResult<ApiToReturnDtoResponse>> GetAllPassengers()
        {
            List<PassengerDto> passengerDtoList = await _passengerService.GetAllWithUser();
            var response = new ApiToReturnDtoResponse
            {
                Data = new DataResponse
                {
                    Mas = "Retrieved all passengers successfully.",
                    StatusCode = StatusCodes.Status200OK,
                    Body = passengerDtoList
                }
            };
            return Ok(response);
        }
    }
}
