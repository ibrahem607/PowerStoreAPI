using PowerStore.Core.Contract;
using PowerStore.Core.Contract.Dtos.Vehicle;
using PowerStore.Core.Contract.VehicleService_Contract;
using PowerStore.Core.Entities;
using PowerStore.Service.Mappers;
using System.Linq.Expressions;

namespace PowerStore.Service.VehicleService
{
    public class VehicleService : IVehicleService
    {
        private readonly VehicleMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public VehicleService(IUnitOfWork unitOfWork, VehicleMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public List<VehicleDto> GetBy(Expression<Func<Vehicle, bool>> predicate)
        {
            return _mapper.MapFromSourceToDestination(_unitOfWork.Repositoy<Vehicle>().GetBy(predicate));
        }

        public int Add(VehicleDto vehicleDto)
        {
            _unitOfWork.Repositoy<Vehicle>().Add(_mapper.MapFromDestinationToSource(vehicleDto));
            return _unitOfWork.Complete();
        }
    }
}
