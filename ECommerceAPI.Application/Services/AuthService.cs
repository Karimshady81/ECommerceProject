using ECommerceAPI.Application.DTOs.Request;
using ECommerceAPI.Application.DTOs.Response;
using ECommerceAPI.Application.Helpers;
using ECommerceAPI.Application.Interfaces;
using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Services
{
    internal class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<AuthResponseDto> RegisterUserAsync(RegisterUserRequestDto userDto)
        {
            if(await _userRepository.EmailExistsAsync(userDto.Email))
            {
                throw new InvalidOperationException("Unable to register user");
            }

            //Map DTO to entity
            var user = new User
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                PasswordHash = PasswordHasher.Hash(userDto.Password),
                Phone = userDto.PhoneNumber,
                CreatedAt = DateTime.UtcNow
            };

            //Save to database
            var createdUser = await _userRepository.AddAsync(user);

            //Map entity to reponse Dto
            return new AuthResponseDto
            {
                Id = createdUser.Id,
                FirstName = createdUser.FirstName,
                LastName = createdUser.LastName,
                Email = createdUser.Email,
                Phone = createdUser.Phone,
                CreatedAt = createdUser.CreatedAt.ToString("D")
            };
        }

        public async Task<AuthResponseDto?> LoginUserAsync(LoginUserRequestDto userDto)
        {
            var user = await _userRepository.GetByEmailAsync(userDto.Email);

            if (user == null || !PasswordHasher.Verify(userDto.Password, user.PasswordHash))
            {
                throw new InvalidOperationException("Invalid Credentials");
            }

            //Map entity to response
            return new AuthResponseDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                CreatedAt = user.CreatedAt.ToString("D")
            };
        }

        public async Task<AuthResponseDto?> GetUserProfileAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if(user == null)
            {
                throw new InvalidOperationException("User not found");
            }

            return new AuthResponseDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                CreatedAt = user.CreatedAt.ToString("D")
            };
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }

            return await _userRepository.DeleteAsync(id);
        }

        public async Task<AuthResponseDto> UpdateUserAsync(int id, UpdateUserRequestDto userDto)
        {
            var existingUser = await _userRepository.GetByIdAsync(id);

            if (existingUser == null)
            {
                throw new InvalidOperationException("No user found");
            }

            if(string.IsNullOrEmpty(userDto.FirstName) || string.IsNullOrEmpty(userDto.LastName) || string.IsNullOrEmpty(userDto.PhoneNumber))
            {
                throw new InvalidOperationException("First name, last name or phone cannot be empty");
            }
            else
            {
                existingUser.FirstName = userDto.FirstName;
                existingUser.LastName = userDto.LastName;
                existingUser.Phone = userDto.PhoneNumber;
            }

            if(string.IsNullOrEmpty(userDto.Password) || existingUser.PasswordHash != userDto.Password)
            {
                if(userDto.Password.Length < 8)
                {
                    throw new InvalidOperationException("Password must be at least 8 characters long");
                }
                existingUser.PasswordHash = PasswordHasher.Hash(userDto.Password);
            }

            existingUser.LastUpdatedAt = DateTime.UtcNow;

            var updatedUser = await _userRepository.UpdateAsync(existingUser);

            return new AuthResponseDto
            {
                Id = updatedUser.Id,
                FirstName = updatedUser.FirstName,
                LastName = updatedUser.LastName,
                Phone = updatedUser.Phone,
                CreatedAt = updatedUser.LastUpdatedAt.ToString("D")
            };
        }
    }
}

