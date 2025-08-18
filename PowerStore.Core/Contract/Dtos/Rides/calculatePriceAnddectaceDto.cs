using System.ComponentModel.DataAnnotations;

namespace PowerStore.Core.Contract.Dtos.Rides
{
    public class calculatePriceAnddectaceDto
    {
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public double PickUpLat { get; set; }
        [Required]
        public double PickUpLon { get; set; }
        [Required]
        public double DroppOffLat { get; set; }
        [Required]
        public double DroppOffLon { get; set; }
    }
}
