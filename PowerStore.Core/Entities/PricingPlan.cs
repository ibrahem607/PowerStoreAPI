using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerStore.Core.Entities
{
    public class PricingPlan:BaseEntity
    {
        public decimal BasePricePerKilometer { get; set; }
        public ICollection<AreaPricingRange> AreaPricingRanges { get; set; }
    }
}
