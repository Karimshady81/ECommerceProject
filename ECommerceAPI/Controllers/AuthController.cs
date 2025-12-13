using Microsoft.AspNetCore.Http;
using ECommerceAPI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ECommerceAPI.Application.DTOs.Request;

namespace ECommerceAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(RegisterUserRequestDto registerUser)
        {
            try
            {
                var createdUser = await _authService.RegisterUserAsync(registerUser);
                return Ok(createdUser);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error: " + ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(LoginUserRequestDto loginUser)
        {
            try
            {
                var login = await _authService.LoginUserAsync(loginUser);
                return Ok(login);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error: " + ex.Message });
            }
        }

        [HttpGet("profile/{id}")]
        public async Task<IActionResult> GetUserProfile(int id)
        {
            try
            {
                var profile = await _authService.GetUserProfileAsync(id);
                return Ok(profile);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error: " + ex.Message });
            }
        }

        [HttpPut("update_user/{id}")]
        public async Task<IActionResult> UpdateUserDetails(int id, UpdateUserRequestDto updateUser)
        {
            try
            {
                var updateProfile = await _authService.UpdateUserAsync(id, updateUser);
                return Ok(updateProfile);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error: " + ex.Message });
            }
        }

        [HttpDelete("delete_user/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var user = await _authService.DeleteUserAsync(id);
                return Ok(new { message = "User deleted successfully"});
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error: " + ex.Message });
            }
        }
    }
}
