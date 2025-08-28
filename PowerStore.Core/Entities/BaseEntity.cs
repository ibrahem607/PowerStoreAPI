using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerStore.Core.Entities
{
    
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
         public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
         public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
         public DateTime DeletionDate { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
        public override string ToString()
        {
            return $"Id: {Id}";
        }
    }
}
