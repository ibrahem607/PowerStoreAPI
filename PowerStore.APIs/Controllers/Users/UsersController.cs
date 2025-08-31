using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PowerStore.Core.Contract.IdentityInterface;
using PowerStore.Core.DTOs.UsersDtos;
using PowerStore.Core.Enums;

namespace PowerStore.APIs.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService) => _userService = userService;

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ReturnUserDto>>> GetAll(
            [FromQuery] UserSearchParams searchParams)
        {
            var users = await _userService.GetAllAsync(searchParams);
            return Ok(users);
        }

        // GET: api/users/type/2
        [HttpGet("type/{userType}")]
        public async Task<ActionResult<IReadOnlyList<ReturnUserDto>>> GetByType(
            UserType userType, [FromQuery] UserSearchParams searchParams)
        {
            var users = await _userService.GetByTypeAsync(userType, searchParams);
            return Ok(users);
        }

        // GET: api/users/2/5 (userType/id)
        [HttpGet("{userType}/{id}")]
        public async Task<ActionResult<ReturnUserDto>> GetById(UserType userType, string id)
        {
            var user = await _userService.GetByIdAsync(id, userType);
            return Ok(user);
        }

        // POST: api/users
        [HttpPost]
        public async Task<ActionResult<ReturnUserDto>> Create(AddUserDto createDto)
        {
            var createdUser = await _userService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById),
                new { userType = createDto.UserType, id = createdUser.Id }, createdUser);
        }

        // PUT: api/users
        [HttpPut]
        public async Task<ActionResult<ReturnUserDto>> Update(UpdateUserDto updateDto)
        {
            var updatedUser = await _userService.UpdateAsync(updateDto);
            return Ok(updatedUser);
        }

        // DELETE: api/users/2/5 (userType/id)
        [HttpDelete("{userType}/{id}")]
        public async Task<IActionResult> Delete(UserType userType, string id)
        {
            await _userService.DeleteAsync(id, userType);
            return NoContent();
        }

        // POST: api/users/change-password
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            await _userService.ChangePasswordAsync(changePasswordDto);
            return Ok("Password changed successfully");
        }


    }
}
