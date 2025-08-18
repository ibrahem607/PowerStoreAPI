﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable enable
namespace PowerStore.Core.Entities
{
    public class Driver : BaseEntity
    {
        public string Id { get; set; }

        public string NationalIdFront { get; set; }
        public string NationalIdBack { get; set; }
        public DateTime NationalIdExpiringDate { get; set; } = DateTime.Now;
        public string DrivingLicenseIdFront { get; set; }
        public string DrivingLicenseIdBack { get; set; }
        public DateTime DrivingLicenseExpiringDate { get; set; } = DateTime.Now;
        public DateTime LastActiveTime { get; set; }
        public DriverStatusWork StatusWork { get; set; } = DriverStatusWork.Pending; // Defualt
        public DriverStatus Status { get; set; } 

        public string UserId { get; set; }
        public virtual ApplicationUser? User { get; set; }
        public virtual ICollection<Vehicle>? Vehicles { get; set; }
        public virtual ICollection<DriverRating> DriverRatings { get; private set; }
        public virtual ICollection<Ride>? Rides { get; set; }
        [DefaultValue(true)]
        public bool Online { get; set; }

    }

    public enum DriverStatusWork
    {
        Pending,
        Approved,
        Rejected
    }

    public enum DriverStatus
    {
        Avaiable,
        InRide,
        Offline
    }

}
