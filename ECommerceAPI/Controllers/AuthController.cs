using ECommerceAPI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ECommerceAPI.Application.DTOs.Request;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ECommerceAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IJwtService _jwtService;

        public AuthController(IAuthService authService, IJwtService jwtService)
        {
            _authService = authService;
            _jwtService = jwtService;
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
                var token = _jwtService.GenerateToken(login.Id.ToString(), login.Email);
                return Ok(new { token });
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

        [Authorize(Roles = "Customer")]
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

        [Authorize(Roles = "Customer")]
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

        [Authorize(Roles = "Customer,Admin")]
        [HttpDelete("delete_me")]
        public async Task<IActionResult> DeleteAccount()
        {
            try
            {
                var userId = int.Parse(
                    User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

                await _authService.DeleteUserAsync(userId);

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
