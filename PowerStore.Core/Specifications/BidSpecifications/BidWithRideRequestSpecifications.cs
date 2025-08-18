using PowerStore.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerStore.Core.Specifications.BidSpecifications
{
    public class BidWithRideRequestSpecifications : BaseSpecifications<Bid>
    {
        public BidWithRideRequestSpecifications(int bidId)
            :base(b => b.Id == bidId)
        {
            Includes.Add(b => b.Ride);
        }
    }
}
