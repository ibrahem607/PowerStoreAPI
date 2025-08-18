using System.ComponentModel.DataAnnotations;

namespace PowerStore.Core.Contract.Dtos.Passenger
{
    public class FindDriverDto
    {
        [Required]
        public string PickupLocation { get; set; }
        [Required]
        public string DropoffLocation { get; set; }
        [Required]
        public string  Type { get; set; }
        [Required]
        public decimal Fare { get; set; }
    }
}
