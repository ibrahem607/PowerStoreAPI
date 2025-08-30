using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerStore.Core.Entities
{
    public class MainArea:BaseEntity
    {
       
        public required string Name { get; set; }          
        public int StartNumbering { get; set; }
        public ICollection<SubArea>? SubAreas { get; set; }
    }
}
