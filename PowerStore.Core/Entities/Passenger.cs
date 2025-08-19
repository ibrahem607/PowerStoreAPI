using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerStore.Core.Entities
{
    public class Passenger : BaseEntity
    {
        public string Id { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public string? PreferredPaymentMethod { get; set; }
        public virtual ICollection<Product>? Rides { get; set; } 
        //public ICollection<RideRequests> RideRequests { get; set; }
        public virtual ICollection<Invoice>? PassengerRatings { get; private set; }
        
        public bool IsRiding { get; set; }
        public bool Online { get; set; }
    }
}
