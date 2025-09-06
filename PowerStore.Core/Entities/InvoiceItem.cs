using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerStore.Core.Entities
{
    public class InvoiceItem : BaseEntity
    {
        public int Id { get; set; }

        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; }

        public string ProductType { get; set; }              
        public string ProductName { get; set; }                
        public int Quantity { get; set; }                     
        public decimal UnitPrice { get; set; }
    }

}
