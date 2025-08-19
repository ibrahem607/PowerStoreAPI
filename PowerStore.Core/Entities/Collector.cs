using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerStore.Core.Entities
{
    public class Collector : BaseEntity
    {
        public int Id { get; set; }                   // الرقم التسلسلى
        public string Name { get; set; }
    }
}
