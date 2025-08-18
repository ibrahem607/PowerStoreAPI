﻿using PowerStore.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerStore.Core.Specifications.DriverSpecifiactions
{
    public class DriverWithRideAndRatingSpecifications : BaseSpecifications<Ride>
    {
        public DriverWithRideAndRatingSpecifications(string driverId) :base(d=> d.DriverId == driverId)
        {
            AddOrderByDesc(r => r.CompletedAt);
        }
    }
}
