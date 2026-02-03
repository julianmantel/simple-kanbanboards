using Microsoft.AspNetCore.Mvc;
using SimpleKanbanBoards.Business.Models;
using SimpleKanbanBoards.Business.Models.User;
using SimpleKanbanBoards.Business.Service.IService;

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
            SetTokenCookie(await _userService.LoginAsync(request));

            return Ok(ApiResult<string>.Success("Successful login"));
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

        private void SetTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,    
                Secure = true,      
                SameSite = SameSiteMode.Strict, 
                Expires = DateTime.UtcNow.AddMinutes(30) 
            };
            Response.Cookies.Append("Token", token, cookieOptions);
        }
    }
}
