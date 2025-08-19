using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable enable
namespace PowerStore.Core.Entities
{
    public class StoreKeeper : BaseEntity
    {
        public string Name { get; set; }              // اسم امين المخزن
        public string Username { get; set; }          // يوزرنيم
        public string Password { get; set; }

    }
}
