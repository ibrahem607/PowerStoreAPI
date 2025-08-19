using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace PowerStore.Core.Entities
{
    public class Branch:BaseEntity
    {

        public string Name { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Address { get; set; }
        public int Status { get; set; }
        public int CompanyId { get; set; }


        // Navigation property to Company
        public virtual Company Company { get; set; }


        //public virtual ApplicationUser? BranchManager { get; set; }
        public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
        public ICollection<InvoiceItem>? invoiceItems { get; set; }
    }

}
