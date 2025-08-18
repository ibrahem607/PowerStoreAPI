using PowerStore.Core.Contract.Dtos.Passenger;
using PowerStore.Core.Contract.Dtos.Vehicle;
using PowerStore.Core.Entities;
using System.Linq.Expressions;

namespace PowerStore.Core.Contract.VehicleService_Contract
{
    public interface IVehicleService
    {
        List<VehicleDto> GetBy(Expression<Func<Vehicle, bool>> predicate);
        int Add(VehicleDto vehicleDto);
    }
}
