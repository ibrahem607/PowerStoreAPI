using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerStore.Core.DTOs.MainAreaDtos
{
    public class CreateMainAreaDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        [Range(1, int.MaxValue)]
        public int StartNumbering { get; set; }
    }
    public class UpdateMainAreaDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        [Range(1, int.MaxValue)]
        public int StartNumbering { get; set; }
    }
}
