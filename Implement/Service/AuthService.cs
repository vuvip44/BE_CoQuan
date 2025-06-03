using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lich.api.DTO.Request.Auth;
using Lich.api.DTO.Response.Auth;
using Lich.api.DTO.Response.User;
using Lich.api.Interface.IRepository;
using Lich.api.Interface.IService;
using Lich.api.Model;

namespace Lich.api.Implement.Service
{
    public class AuthService : IAuthService
    {
        private readonly ITokenService _tokenService;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        public AuthService(
            ITokenService tokenService,
            IRoleRepository roleRepository,
            IUserRepository userRepository,
            IConfiguration configuration)
        {
            _tokenService = tokenService;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _configuration = configuration;
        }
        public async Task<TokenDto> LoginAsync(ReqLoginDto login)
        {
            var user = await _userRepository.GetUserByEmailAsync(login.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(login.Password, user.Password))
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }
            var accessToken = _tokenService.GenerateAccessTokenAsync(user);
            var refreshToken = _tokenService.GenerateRefreshTokenAsync();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); // Set refresh token expiry time
            await _userRepository.UpdateUserAsync(user);
            return new TokenDto(
                accessToken,
                refreshToken,
                user.RefreshTokenExpiryTime.Value,
                new ResUserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = user.FullName,
                    RoleId = user.RoleId
                }
            );

        }

        public async Task LogoutAsync(string refreshToken)
        {
            var user = await _userRepository.GetUserByRefreshTokenAsync(refreshToken);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid refresh token.");
            }
            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null; // Clear the refresh token and expiry time
            await _userRepository.UpdateUserAsync(user);

        }

        public async Task<TokenDto> RefreshTokenAsync(string refreshToken)
        {
            var user = await _userRepository.GetUserByRefreshTokenAsync(refreshToken);
            if (user == null || user.RefreshTokenExpiryTime < DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException("Invalid or expired refresh token.");
            }
            var accessToken = _tokenService.GenerateAccessTokenAsync(user);
            var newRefreshToken = _tokenService.GenerateRefreshTokenAsync();
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); // Set new refresh token expiry time
            await _userRepository.UpdateUserAsync(user);
            return new TokenDto(
                accessToken,
                newRefreshToken,
                user.RefreshTokenExpiryTime.Value,
                new ResUserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = user.FullName,
                    RoleId = user.RoleId
                }
            );
        }

        public async Task<ResUserDto> RegisterAsync(ReqRegisterDto register)
        {
            if (await _userRepository.IsExistUserAsync(register.Email))
            {
                throw new InvalidOperationException("User with this email already exists.");
            }
            var user = new User
            {
                Email = register.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(register.Password),
                FullName = register.FullName,
                RoleId = (await _roleRepository.GetRoleByNameAsync("User")).Id // Default to User role
            };

            var newUser = await _userRepository.CreateUserAsync(user);
            if (newUser == null)
            {
                throw new InvalidOperationException("Failed to create user.");
            }
            return new ResUserDto
            {
                Id = newUser.Id,
                Email = newUser.Email,
                FullName = newUser.FullName,
                RoleId = newUser.RoleId
            };
        }
    }
}