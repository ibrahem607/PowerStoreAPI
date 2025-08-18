using AutoMapper;
using PowerStore.Core.Contract.Dtos.Passenger;
using PowerStore.Core.Contract.Dtos.Rides;
using PowerStore.Core.Entities;
using PowerStore.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerStore.Service.Mappers
{
    public class PassengerMapper : MapperBase<Passenger, PassengerDto>
    {
        private readonly IMapper _mapper;

        public PassengerMapper()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Passenger, PassengerDto>()
                .ReverseMap();
                cfg.CreateMap<Ride, RideDto>()
                .ReverseMap();
                cfg.CreateMap<PassengerRating, PassengerRatingDto>()
                .ReverseMap();
                cfg.CreateMap<RideRequests, RideRequestDto>()
                .ReverseMap();
                cfg.CreateMap<Locations, LocationsDto>()
                .ReverseMap();

            });
            _mapper = config.CreateMapper();
        }
        public override Passenger MapFromDestinationToSource(PassengerDto destinationEntity)
        {
            return _mapper.Map<Passenger>(destinationEntity);

        }

        public override PassengerDto MapFromSourceToDestination(Passenger sourceEntity)
        {
            return _mapper.Map<PassengerDto>(sourceEntity);
        }
    }
}
