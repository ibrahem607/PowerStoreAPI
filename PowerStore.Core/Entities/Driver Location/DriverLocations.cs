﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerStore.Core.Entities.Driver_Location
{
    public class DriverLocations
    {
        public string DriverId { get; set; } = null!;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string VehicleType { get; set; }
        //public DateTime Timestamp { get; set; }
    }
}
