﻿using PowerStore.Core.Entities;

namespace PowerStore.Core.Contract.Dtos.Passenger
{
    public class ProfileDto
    {
        public string FullName { get; set; }
        public string ProfilePictureUrl { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }
    }
}
