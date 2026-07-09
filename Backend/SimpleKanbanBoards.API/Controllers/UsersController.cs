using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleKanbanBoards.Business.Models;
using SimpleKanbanBoards.Business.Models.Role;
using SimpleKanbanBoards.Business.Models.User;
using SimpleKanbanBoards.Business.Service.IService;
using System.Security.Claims;

namespace SimpleKanbanBoards.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound(ApiResult<string>.Failure(new List<string> { "User not found" }));

            return Ok(ApiResult<UserModel>.Success(user));
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync([FromBody] CreateUserModel newUser)
        {
            await _userService.CreateUserAsync(newUser);
            return Ok(ApiResult<string>.Success($"{newUser.UserName} created successfully"));
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequestModel request)
        {
            var token = await _userService.LoginAsync(request);

            SetTokenCookie(token);
            return Ok(ApiResult<string>.Success(token));
        }

        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("Token");
            return Ok(ApiResult<string>.Success("Successful logout"));
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPassRequestModel request)
        {
            await _userService.ResetPasswordAsync(request);
            return Ok(ApiResult<string>.Success(""));
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordModel request)
        {
            await _userService.ChangePasswordAsync(request);
            return Ok(ApiResult<string>.Success("Password changed successfully"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAsync(int id)
        {
            await _userService.DeleteUserAsync(id);
            return Ok(ApiResult<string>.Success("User deleted successfully"));
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUserAsync()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userIdClaim is null || !int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized("Invalid token");
            }

            var user = await _userService.GetUserByIdAsync(userId);

            return Ok(ApiResult<UserModel>.Success(user));
        }

        [HttpPost("change-roles")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeTokenRolesAsync([FromBody] UserModel request)
        {
            var newToken = await _userService.ChangeTokenRolesAsync(request);
            SetTokenCookie(newToken);

            return Ok(ApiResult<string>.Success("Token roles changed successfully"));
        }

        private void SetTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("Token", token, cookieOptions);
        }
    }
}
