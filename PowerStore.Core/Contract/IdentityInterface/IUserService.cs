using Microsoft.AspNetCore.Identity;
using PowerStore.Core.Contract.Responses;
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
        Task<ApiResponse<ReturnUserDto>> GetByIdAsync(string id, UserType userType);
        Task<ApiResponse<IReadOnlyList<ReturnUserDto>>> GetAllAsync(UserSearchParams searchParams);
        Task<ApiResponse<IReadOnlyList<ReturnUserDto>>> GetByTypeAsync(UserType userType, UserSearchParams searchParams);
        Task<ApiResponse<ReturnUserDto>> CreateAsync(AddUserDto createDto);
        Task<ApiResponse<ReturnUserDto>> UpdateAsync(UpdateUserDto updateDto);
        Task<ApiResponse<bool>> DeleteAsync(string id, UserType userType);
        Task<ApiResponse<bool>> ChangePasswordAsync(ChangePasswordDto changePasswordDto);

        // Helper methods
        string GetRoleName(UserType userType);
        string GetUserTypeName(UserType userType);
    }
}
