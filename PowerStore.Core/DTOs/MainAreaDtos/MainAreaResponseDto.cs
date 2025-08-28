﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerStore.Core.DTOs.MainAreaDtos
{
    public class MainAreaResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int StartNumbering { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
