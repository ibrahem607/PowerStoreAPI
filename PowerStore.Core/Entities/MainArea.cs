using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerStore.Core.Entities
{
    public class MainArea:BaseEntity
    {
        public string Name { get; set; }              // اسم المنطقه الرئيسيه
        public int StartNumbering { get; set; }
    }
}
