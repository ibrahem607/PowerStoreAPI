using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerStore.Core.Enums;

namespace PowerStore.Core.Entities
{
    public class Invoice : BaseEntity
    {
        public int Id { get; set; }                          
        public DateTime InvoiceDate { get; set; }               
        public decimal TotalAmount { get; set; }                
        public decimal? DueAmount { get; set; }                
        public string Notes { get; set; }
        public InvoiceType invoiceType { get; set; }
        public int SupplierId { get; set; }
        public ApplicationUser Supplier { get; set; }

        public ICollection<InvoiceItem> Items { get; set; }
    }
}
