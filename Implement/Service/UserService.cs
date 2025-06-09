using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE_CoQuan.DTO.Request.Auth;
using Lich.api.DTO.Request.Auth;
using Lich.api.DTO.Request.Query;
using Lich.api.DTO.Response.User;
using Lich.api.Interface.IRepository;
using Lich.api.Interface.IService;
using Lich.api.Model;

namespace Lich.api.Implement.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public UserService(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<ResUserDto> CreateUserAsync(ReqRegisterDto dto)
        {
            if (await _userRepository.IsExistUserAsync(dto.Email))
            {
                throw new InvalidOperationException("User with this email already exists.");
            }
            var user = new User
            {
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                FullName = dto.FullName,
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

        public async Task<bool> DeleteUserAsync(int user)
        {
            var result = await _userRepository.DeleteUserAsync(user);
            if (!result)
            {
                throw new KeyNotFoundException($"User with ID {user} not found.");
            }
            return true;
        }

        public async Task<List<ResUserDto>> GetAllUsersAsync(QueryUser query)
        {
            var users = await _userRepository.GetAllUsersAsync(query);
            return users.Select(u => new ResUserDto
            {
                Id = u.Id,
                Email = u.Email,
                FullName = u.FullName,
                RoleId = u.RoleId
            }).ToList();
        }

        public async Task<ResUserDto> GetUserByIdAsync(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }
            return new ResUserDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                RoleId = user.RoleId
            };
        }

        public async Task<ResUserDto> GetUserByUsernameAsync(string username)
        {
            var user = await _userRepository.GetUserByEmailAsync(username);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with username {username} not found.");
            }
            return new ResUserDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                RoleId = user.RoleId
            };
        }

        public async Task<bool> UpdatePasswordByCurrentUser(int userId, ReqPasswordDto dto)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, user.Password))
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }
            user.Password = dto.NewPassword;
            var newUser = await _userRepository.UpdateUserAsync(user);
            if (newUser == null)
            {
                throw new Exception("Failed to update user role.");
                return false;
            }
            return true;
        }

        public async Task<bool> UpdateRoleUserAsync(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");

            }
            if (user.RoleId == 1)
            {
                user.RoleId = 2; // Assuming 1 is Admin and 2 is User
            }
            else if (user.RoleId == 2)
            {
                user.RoleId = 1; // Switch back to Admin
            }
            else
            {
                throw new InvalidOperationException("Invalid role for user.");
            }
            var updatedUser = await _userRepository.UpdateUserAsync(user);
            if (updatedUser == null)
            {
                throw new Exception("Failed to update user role.");
                return false;
            }
            return true;
        }


    }
}