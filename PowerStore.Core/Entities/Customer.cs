using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerStore.Core.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }                      // اسم العميل
        public string MobileNumber { get; set; }              // رقم الموبايل
        public string OccupationOrResidence { get; set; }     // العمل/الاقامه
        public DateTime SaleDate { get; set; }                // تاريخ البيع
        public DateTime FirstInvoiceDate { get; set; }        // تاريخ اول فاتوره
        public string NationalId { get; set; }                // الرقم القومى

        public int MainAreaId { get; set; }
        public MainArea MainArea { get; set; }

        public int SubAreaId { get; set; }
        public SubArea SubArea { get; set; }

        public string RepresentativeOrCollector { get; set; } // المندوب\المحصل

        public string ProductType { get; set; }               // نوع المنتج
        public string ProductName { get; set; }               // المنتج
        public int Quantity { get; set; }                     // الكميه
        public decimal Price { get; set; }
    }
}
