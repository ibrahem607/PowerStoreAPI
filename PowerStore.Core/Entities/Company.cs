using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerStore.Core.Entities
{
    public class Company : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
      
        public string City { get; set; }
        public string State { get; set; }
        public string CountryCode { get; set; }
        public string ZipCode { get; set; }
        public string Address { get; set; }
        public string? LogoUrl { get; set; }

        public ICollection<Branch> Branches { get; set; } = new List<Branch>();
        //public ICollection<Supplier> Sites { get; set; } = new List<Supplier>();
    }
}
