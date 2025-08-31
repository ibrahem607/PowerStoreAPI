using Microsoft.AspNetCore.Identity;
using PowerStore.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace PowerStore.Core.Entities
{
   
    public class ApplicationUser : IdentityUser
    {
          public string? FullName { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public DateTime? DateOfBirth { get; set; } = DateTime.Now;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
        public DateTime DeletionDate { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
        public UserType? UserType { get; set; }

        public virtual List<RefreshToken>? RefreshTokens { get; set; }

    }
}
