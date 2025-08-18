using PowerStore.Core.Entities;
using System.ComponentModel;

namespace PowerStore.Core.Contract.Dtos.Driver
{
    public class DriverDto
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

    }
    public class ReturnDriverDto
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
    }
}
