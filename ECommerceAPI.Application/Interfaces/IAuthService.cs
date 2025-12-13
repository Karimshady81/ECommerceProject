using ECommerceAPI.Application.DTOs.Request;
using ECommerceAPI.Application.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterUserAsync(RegisterUserRequestDto userDto);
        Task<AuthResponseDto?> LoginUserAsync(LoginUserRequestDto userDto);
        Task<AuthResponseDto?> GetUserProfileAsync(int id);
        Task<bool> DeleteUserAsync(int id);
        Task<AuthResponseDto> UpdateUserAsync(int id, UpdateUserRequestDto userDto);
    }
}
