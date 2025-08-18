using Microsoft.AspNetCore.Identity;
using PowerStore.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerStore.Core.Contract.IdentityInterface
{
    public interface ITokenService
    {
        Task<string> CreateTokenAsync(ApplicationUser user);

        RefreshToken GenerateRefreshtoken();
        
    }
}
