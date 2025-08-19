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

        public string ProductType { get; set; }               // نوع المنتج
        public string ProductName { get; set; }               // المنتج
        public int Quantity { get; set; }                     // الكميه
        public decimal UnitPrice { get; set; }
    }

}
