using PowerStore.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerStore.Core.Specifications.DriverSpecifiactions
{
    public class DriverWithVehicleSpecifications : BaseSpecifications<Vehicle>
    {
        public DriverWithVehicleSpecifications(string driverId)
            :base(v => v.DriverId == driverId)
        {
            Includes.Add(v => v.Driver);
        }
    }
}
