using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerStore.Core.Entities
{
    public class Invoice : BaseEntity
    {
        public int Id { get; set; }                           // رقم الفاتوره
        public DateTime InvoiceDate { get; set; }             // تاريخ الفاتوره
        public decimal TotalAmount { get; set; }              // المبلغ الاجمالى
        public string Notes { get; set; }                     // ملاحظات

        public int SupplierId { get; set; }
        public ApplicationUser Supplier { get; set; }

        public ICollection<InvoiceItem> Items { get; set; }
    }
}
