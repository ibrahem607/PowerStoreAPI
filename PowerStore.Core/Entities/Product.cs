using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
#nullable enable
namespace PowerStore.Core.Entities
{
    public class Product : BaseEntity
    {
        public string ProductName { get; set; }
        public decimal CommissionRate { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SalePrice { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}
