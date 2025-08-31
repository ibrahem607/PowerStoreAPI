using Microsoft.AspNetCore.Identity;
using PowerStore.Core.DTOs.UsersDtos;
using PowerStore.Core.Entities;
using PowerStore.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PowerStore.Core.Contract.IdentityInterface
{
    public interface IUserService
    {
        Task<ReturnUserDto> GetByIdAsync(string id, UserType userType);
        Task<IReadOnlyList<ReturnUserDto>> GetAllAsync(UserSearchParams searchParams);
        Task<IReadOnlyList<ReturnUserDto>> GetByTypeAsync(UserType userType, UserSearchParams searchParams);
        Task<ReturnUserDto> CreateAsync(AddUserDto createDto);
        Task<ReturnUserDto> UpdateAsync(UpdateUserDto updateDto);
        Task<bool> DeleteAsync(string id, UserType userType);
        Task<bool> ChangePasswordAsync(ChangePasswordDto changePasswordDto);

        // Helper methods
        string GetRoleName(UserType userType);
        string GetUserTypeName(UserType userType);
    }
}
