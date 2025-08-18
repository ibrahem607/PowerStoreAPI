using System.ComponentModel.DataAnnotations;

namespace PowerStore.Core.Contract.Dtos.Driver
{
    public class DriverLocationToReturnDto
    {
        public string DriverId { get; set; }
        public double Latitude { get; set; }
    
        public double Longitude { get; set; }
    }
}
