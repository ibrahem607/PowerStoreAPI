using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerStore.Core.Entities
{
    public class Representative : BaseEntity
    {
        public string Name { get; set; }              // اسم المندوب
        public string Username { get; set; }          // رقم المستخدم
        public string Password { get; set; }          // 
    }
}