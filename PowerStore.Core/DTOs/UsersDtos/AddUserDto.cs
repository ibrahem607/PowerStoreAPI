using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerStore.Core.Enums;

namespace PowerStore.Core.DTOs.UsersDtos
{
    public class AddUserDto
    {
     [Required]
    [StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    public string? UserName { get; set; } = string.Empty;

    public string? Password { get; set; } = string.Empty;

    public string? Email { get; set; }


    [Required]
    public UserType UserType { get; set; }
}

public class UpdateUserDto
{
    [Required]
    public string Id { get; set; } = string.Empty;

    [StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    [StringLength(50)]
    public string? UserName { get; set; } = string.Empty;

    [EmailAddress]
    public string? Email { get; set; }

  

    [Required]
    public UserType UserType { get; set; }


}

public class ChangePasswordDto
{
    [Required]
    public string UserId { get; set; } = string.Empty;

    [Required]
    public UserType UserType { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string NewPassword { get; set; } = string.Empty;
}

public class ReturnUserDto
{
    public string Id { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Password { get; set; } = string.Empty;

    public UserType UserType { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}

public class UserSearchParams
{
    private const int MaxPageSize = 50;
    public int PageIndex { get; set; } = 1;
    private int _pageSize = 10;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }
    public string? Search { get; set; }
    public UserType? UserType { get; set; }
}
}
