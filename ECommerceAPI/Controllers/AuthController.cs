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
        public async Task<IActionResult> RegisterUser(RegisterUserRequestDto user)
        {
            try
            {
                var createdUser = await _authService.RegisterUserAsync(user);
                return Ok(createdUser);
            }
            catch (InvalidDataException ex)
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
