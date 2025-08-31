using AutoMapper;
using Azure;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PowerStore.Core.Contract.Errors;
using PowerStore.Core.Contract.IdentityInterface;
using PowerStore.Core.Contract.Responses;
using PowerStore.Core.DTOs.UsersDtos;
using PowerStore.Core.Entities;
using PowerStore.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ValidationException = PowerStore.Core.Contract.Errors.ValidationException;

namespace PowerStore.Service.Identity
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;
       public UserService(UserManager<ApplicationUser> userManager,
            ILogger<UserService> logger,IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApiResponse< ReturnUserDto>> GetByIdAsync(string id, UserType userType)
        {
            try { 
            var user = await _userManager.FindByIdAsync(id);
            if (user == null || user.UserType != userType)
                throw new KeyNotFoundException($"{GetUserTypeName(userType)} with Id {id} not found.");

            var userDto = _mapper.Map<ReturnUserDto>(user);
            return ApiResponse<ReturnUserDto>.SuccessResponse(userDto, $"{GetUserTypeName(userType)} retrieved successfully");
                 }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "User not found: {Id}, Type: {UserType}", id, userType);
                return ApiResponse<ReturnUserDto>.ErrorResponse(ex.Message, statusCode: 404);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user: {Id}, Type: {UserType}", id, userType);
                return ApiResponse<ReturnUserDto>.ErrorResponse("An error occurred while retrieving the user", statusCode: 500);
            }
        }
            

        public async Task<ApiResponse< IReadOnlyList<ReturnUserDto>>> GetAllAsync(UserSearchParams searchParams)
        {
            try { 
            var query = _userManager.Users.AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(searchParams.Search))
            {
                query = query.Where(u =>
                    u.FullName.Contains(searchParams.Search) ||
                    u.UserName.Contains(searchParams.Search) ||
                    (u.Email != null && u.Email.Contains(searchParams.Search)));
            }

            

            if (searchParams.UserType.HasValue)
            {
                query = query.Where(u => u.UserType == searchParams.UserType.Value);
            }

            // Apply ordering and pagination
            query = query.OrderBy(u => u.FullName)
                        .Skip(searchParams.PageSize * (searchParams.PageIndex - 1))
                        .Take(searchParams.PageSize);

            var users = await query.ToListAsync();
                var userDtos = _mapper.Map<IReadOnlyList<ReturnUserDto>>(users);

                return ApiResponse<IReadOnlyList<ReturnUserDto>>.SuccessResponse(userDtos, "Users retrieved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving users");
                return ApiResponse<IReadOnlyList<ReturnUserDto>>.ErrorResponse("An error occurred while retrieving users", statusCode: 500);
            }
        }

        public async Task<ApiResponse<IReadOnlyList<ReturnUserDto>>> GetByTypeAsync(UserType userType, UserSearchParams searchParams)
        {
            try
            {
                var query = _userManager.Users.Where(u => u.UserType == userType);

                // Apply filters
                if (!string.IsNullOrEmpty(searchParams.Search))
                {
                    query = query.Where(u =>
                        u.FullName.Contains(searchParams.Search) ||
                        u.UserName.Contains(searchParams.Search) ||
                        (u.Email != null && u.Email.Contains(searchParams.Search)));
                }



                // Apply ordering and pagination
                query = query.OrderBy(u => u.FullName)
                            .Skip(searchParams.PageSize * (searchParams.PageIndex - 1))
                            .Take(searchParams.PageSize);

                var users = await query.ToListAsync();
                var userDtos = _mapper.Map<IReadOnlyList<ReturnUserDto>>(users);
                return ApiResponse<IReadOnlyList<ReturnUserDto>>.SuccessResponse(userDtos, $"{GetUserTypeName(userType)}s retrieved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving users of type: {UserType}", userType);
                return ApiResponse<IReadOnlyList<ReturnUserDto>>.ErrorResponse("An error occurred while retrieving users", statusCode: 500);
            }
        }
        public async Task<ApiResponse<ReturnUserDto>> CreateAsync(AddUserDto createDto)
        {
            try {
                // Generate username from full name if not provided
                var userName = createDto.UserName;
                if (string.IsNullOrEmpty(userName))
                {
                    userName = GenerateUserNameFromFullName(createDto.FullName);
                }

                // Generate password if not provided
                var password = createDto.Password;
                if (string.IsNullOrEmpty(password))
                {
                    password = "P@ssw0rd";
                }

                // Check if username already exists
                var existingUser = await _userManager.FindByNameAsync(userName);
                if (existingUser != null)
                    throw new BusinessRuleException($"Username '{userName}' is already taken.");

                // Check if email already exists (only if email is provided)
                if (!string.IsNullOrEmpty(createDto.Email))
                {
                    existingUser = await _userManager.FindByEmailAsync(createDto.Email);
                    if (existingUser != null)
                        throw new BusinessRuleException($"Email '{createDto.Email}' is already registered.");

                    // Validate email format
                    var emailValidator = new EmailAddressAttribute();
                    if (!emailValidator.IsValid(createDto.Email))
                        throw new ValidationException($"Email '{createDto.Email}' is not valid.");
                }

                var user = _mapper.Map<ApplicationUser>(createDto);
                user.UserName = userName; // Use generated/provided username
                user.CreatedDate = DateTime.UtcNow;
                user.UpdatedDate = DateTime.UtcNow;

                var result = await _userManager.CreateAsync(user, password);

                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new IdentityOperationException($"Failed to create {GetUserTypeName(createDto.UserType)}: {errors}");
                }

                // Add to appropriate role
                var roleName = GetRoleName(createDto.UserType);
                var roleResult = await _userManager.AddToRoleAsync(user, roleName);

                if (!roleResult.Succeeded)
                {
                    await _userManager.DeleteAsync(user);
                    var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                    throw new IdentityOperationException($"Failed to assign role: {errors}");
                }

                var response = _mapper.Map<ReturnUserDto>(user);
                response.Password = password;
                return ApiResponse<ReturnUserDto>.SuccessResponse(response, $"{GetUserTypeName(createDto.UserType)} created successfully", 201);
            }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "Validation error creating user");
            return ApiResponse<ReturnUserDto>.ErrorResponse(ex.Message,ex.Errors, 400);
        }
        catch (BusinessRuleException ex)
        {
            _logger.LogWarning(ex, "Business rule violation creating user");
            return ApiResponse<ReturnUserDto>.ErrorResponse(ex.Message, statusCode: 400);
        }
        catch (IdentityOperationException ex)
        {
            _logger.LogError(ex, "Identity operation failed creating user");
return ApiResponse<ReturnUserDto>.ErrorResponse(ex.Message, ex.Errors, 400);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user");
return ApiResponse<ReturnUserDto>.ErrorResponse("An error occurred while creating the user", statusCode: 500);
        }
    }

        // Helper methods for generating username and password
        private string GenerateUserNameFromFullName(string fullName)
        {
            // Remove spaces and special characters, convert to lowercase
            var userName = fullName.ToLower()
                .Replace(" ", ".")
                .Replace("'", "")
                .Replace("-", ".");

            // Add random numbers to ensure uniqueness
            var random = new Random();
            return $"{userName}{random.Next(100, 999)}";
        }

        //private string GenerateRandomPassword()
        //{
        //    const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*";
        //    var random = new Random();
        //    var password = new char[12];

        //    for (int i = 0; i < password.Length; i++)
        //    {
        //        password[i] = validChars[random.Next(validChars.Length)];
        //    }

        //    return new string(password);
        //}

        public async Task<ApiResponse<ReturnUserDto>> UpdateAsync(UpdateUserDto updateDto)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(updateDto.Id);
                if (user == null || user.UserType != updateDto.UserType)
                    throw new BusinessRuleException($"{GetUserTypeName(updateDto.UserType)} with Id {updateDto.Id} not found.");

                // Check if username already exists (excluding current user)
                if (user.UserName != updateDto.UserName)
                {
                    var existingUser = await _userManager.FindByNameAsync(updateDto.UserName);
                    if (existingUser != null && existingUser.Id != updateDto.Id)
                        throw new IdentityOperationException($"Username '{updateDto.UserName}' is already taken.");
                }

                // Check if email already exists (excluding current user)
                if (user.Email != updateDto.Email && !string.IsNullOrEmpty(updateDto.Email))
                {
                    var existingUser = await _userManager.FindByEmailAsync(updateDto.Email);
                    if (existingUser != null && existingUser.Id != updateDto.Id)
                        throw new IdentityOperationException($"Email '{updateDto.Email}' is already registered.");
                }

                _mapper.Map(updateDto, user);
                user.UpdatedDate = DateTime.UtcNow;

                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new IdentityOperationException($"Failed to update {GetUserTypeName(updateDto.UserType)}: {errors}");
                }
                var response = _mapper.Map<ReturnUserDto>(user);
                return ApiResponse<ReturnUserDto>.SuccessResponse(response, $"Users returned successfully", 200);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validation error updating user");
                return ApiResponse<ReturnUserDto>.ErrorResponse(ex.Message, ex.Errors, 400);
            }
            catch (BusinessRuleException ex)
            {
                _logger.LogWarning(ex, "Business rule violation updating user");
                return ApiResponse<ReturnUserDto>.ErrorResponse(ex.Message, statusCode: 400);
            }
            catch (IdentityOperationException ex)
            {
                _logger.LogError(ex, "Identity operation failed updating user");
                return ApiResponse<ReturnUserDto>.ErrorResponse(ex.Message, ex.Errors, 400);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user");
                return ApiResponse<ReturnUserDto>.ErrorResponse("An error occurred while updating the user", statusCode: 500);
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(string id, UserType userType)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
            if (user == null || user.UserType != userType)
                throw new NotFoundException($"{GetUserTypeName(userType)} with Id {id} not found.");

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new IdentityOperationException($"Failed to delete {GetUserTypeName(userType)}: {errors}");
            }

                return ApiResponse<bool>.SuccessResponse(true, $"{GetUserTypeName(userType)} deleted successfully");
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "User not found for deletion");
                return ApiResponse<bool>.ErrorResponse(ex.Message, statusCode: 404);
            }
            catch (IdentityOperationException ex)
            {
                _logger.LogError(ex, "Identity operation failed deleting user");
                return ApiResponse<bool>.ErrorResponse(ex.Message, ex.Errors, 400);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user");
                return ApiResponse<bool>.ErrorResponse("An error occurred while deleting the user", statusCode: 500);
            }
        }

        public async Task<ApiResponse<bool>> ChangePasswordAsync(ChangePasswordDto changePasswordDto)
        {
            try { 
            var user = await _userManager.FindByIdAsync(changePasswordDto.UserId);
            if (user == null || user.UserType != changePasswordDto.UserType)
                throw new NotFoundException($"{GetUserTypeName(changePasswordDto.UserType)} with Id {changePasswordDto.UserId} not found.");

            // Generate a password reset token and reset the password
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, changePasswordDto.NewPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new IdentityOperationException($"Failed to change password: {errors}");
            }

            user.UpdatedDate = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

                return ApiResponse<bool>.SuccessResponse(true, $"Password changed successfully");
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "User not found for deletion");
                return ApiResponse<bool>.ErrorResponse(ex.Message, statusCode: 404);
            }
            catch (IdentityOperationException ex)
            {
                _logger.LogError(ex, "Identity operation failed deleting user");
                return ApiResponse<bool>.ErrorResponse(ex.Message, ex.Errors, 400);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user");
                return ApiResponse<bool>.ErrorResponse("An error occurred while deleting the user", statusCode: 500);
            }
        }

       

        // Helper methods
        public string GetRoleName(UserType userType)
        {
            return userType switch
            {
                UserType.Customer => "Customer",
                UserType.Representative => "Representative",
                UserType.Collector => "Collector",
                UserType.StoreKeeper => "StoreKeeper",
                UserType.Supplier=>"Supplier",
                _ => throw new ArgumentException("Invalid user type")
            };
        }

        public string GetUserTypeName(UserType userType)
        {
            return userType switch
            {
                UserType.Customer => "Customer",
                UserType.Representative => "Representative",
                UserType.Collector => "Collector",
                UserType.StoreKeeper => "StoreKeeper",
                UserType.Supplier => "Supplier",
                _ => throw new ArgumentException("Invalid user type")
            };
        }
    }
}
