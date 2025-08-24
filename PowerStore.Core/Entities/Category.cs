using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerStore.Core.Entities
{
    public class Category:BaseEntity
    {
        public required string Name { get; set; }


        // Navigation property for Items
        public ICollection<Product>? Products { get; set; }
    }
}
