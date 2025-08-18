using AutoMapper;
using PowerStore.Core.Contract.Dtos.Identity;
using PowerStore.Core.Contract.Dtos.Passenger;
using PowerStore.Core.Contract.Dtos.Rides;
using PowerStore.Core.Contract.Dtos.Vehicle;
using PowerStore.Core.Entities;
using PowerStore.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerStore.Service.Mappers
{
    public class DriverMapper : MapperBase<Driver, DriverDto>
    {
        private readonly IMapper _mapper;

        public DriverMapper()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Driver, DriverDto>()
                .ReverseMap();
                cfg.CreateMap<Ride, RideDto>()
                .ReverseMap();
                cfg.CreateMap<Vehicle, VehicleDto>()
                .ReverseMap();
                cfg.CreateMap<RideRequests, RideRequestDto>()
                .ReverseMap();
                cfg.CreateMap<Locations, LocationsDto>()
                .ReverseMap();

            });
            _mapper = config.CreateMapper();
        }
        public override Driver MapFromDestinationToSource(DriverDto destinationEntity)
        {
            return _mapper.Map<Driver>(destinationEntity);

        }
        public override DriverDto MapFromSourceToDestination(Driver sourceEntity)
        {
            return _mapper.Map<DriverDto>(sourceEntity);
        }

    }
}
