using PowerStore.Core.Contract.Dtos.Rides;
using PowerStore.Core.Entities;
namespace PowerStore.Core.Contract.Dtos.Passenger
{
    public class PassengerDto
    {
        public string Id { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public string? PreferredPaymentMethod { get; set; }
        public virtual ICollection<RideDto>? Rides { get; set; }
        //public ICollection<RideRequests> RideRequests { get; set; }
        public virtual ICollection<PassengerRatingDto>? PassengerRatings { get; set; }

        public bool IsRiding { get; set; }
    }
}
