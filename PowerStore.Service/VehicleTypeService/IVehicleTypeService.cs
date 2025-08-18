using PowerStore.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerStore.Service.VehicleTypeService
{
    public interface IVehicleTypeService
    {
        Task<IEnumerable<VehicleType>> GetVehicleTypesByCategoryIdAsync(int categoryId);
    }
}
