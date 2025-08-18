using AutoMapper;
using PowerStore.Core.Contract.Dtos.Identity;
using PowerStore.Core.Contract.Dtos.Passenger;
using PowerStore.Core.Contract.Dtos.Rides;
using PowerStore.Core.Contract.Dtos.Vehicle;
using PowerStore.Core.Contract.Dtos.VehicleModel;
using PowerStore.Core.Entities;
using PowerStore.Core.Specifications;

namespace PowerStore.Service.Mappers
{
    public class VehicleMapper : MapperBase<Vehicle, VehicleDto>
    {
        private readonly IMapper _mapper;
        public VehicleMapper()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Vehicle, VehicleDto>()
                .ReverseMap();
                cfg.CreateMap<Driver, DriverDto>()
                .ReverseMap();
                cfg.CreateMap<VehicleModel, VehicleModelDTO>()
                .ReverseMap();

            });
            _mapper = config.CreateMapper();
        }
        public override Vehicle MapFromDestinationToSource(VehicleDto destinationEntity)
        {
            return _mapper.Map<Vehicle>(destinationEntity);

        }

        public override VehicleDto MapFromSourceToDestination(Vehicle sourceEntity)
        {
            return _mapper.Map<VehicleDto>(sourceEntity);
        }
    }
}
