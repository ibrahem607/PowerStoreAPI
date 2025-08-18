﻿using Microsoft.AspNetCore.Http;
using PowerStore.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace PowerStore.Core.Contract.Dtos.Passenger
{
    public class updateUserDto
    {
 
        [MaxLength(100, ErrorMessage = "The max length is 100 char")]
        [MinLength(5, ErrorMessage = "The min length is 5 char")]
        public string FullName { get; set; }

        public DateTime? DataOfBirth { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public IFormFile? UploadFile { get; set; }

        public Gender Gender { get; set; }
    }
}
