using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PowerStore.Core.Contract.IdentityInterface;
using PowerStore.Core.Contract.Responses;
using PowerStore.Core.DTOs.UsersDtos;
using PowerStore.Core.Enums;

namespace PowerStore.APIs.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IReadOnlyList<ReturnUserDto>>>> GetAll(
            [FromQuery] UserSearchParams searchParams)
        {
            var result = await _userService.GetAllAsync(searchParams);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("type/{userType}")]
        public async Task<ActionResult<ApiResponse<IReadOnlyList<ReturnUserDto>>>> GetByType(
            UserType userType, [FromQuery] UserSearchParams searchParams)
        {
            var result = await _userService.GetByTypeAsync(userType, searchParams);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{userType}/{id}")]
        public async Task<ActionResult<ApiResponse<ReturnUserDto>>> GetById(UserType userType, string id)
        {
            var result = await _userService.GetByIdAsync(id, userType);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<ReturnUserDto>>> Create(AddUserDto createDto)
        {
            var result = await _userService.CreateAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut]
        public async Task<ActionResult<ApiResponse<ReturnUserDto>>> Update(UpdateUserDto updateDto)
        {
            var result = await _userService.UpdateAsync(updateDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{userType}/{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(UserType userType, string id)
        {
            var result = await _userService.DeleteAsync(id, userType);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("change-password")]
        public async Task<ActionResult<ApiResponse<bool>>> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            var result = await _userService.ChangePasswordAsync(changePasswordDto);
            return StatusCode(result.StatusCode, result);
        }
    }
}
