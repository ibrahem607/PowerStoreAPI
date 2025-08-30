using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerStore.Core.DTOs.MainAreaDtos
{
    public class SubAreaResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int MainAreaId { get; set; }
        public string MainAreaName { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
