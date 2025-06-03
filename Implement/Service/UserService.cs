using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lich.api.DTO.Response.User;
using Lich.api.Interface.IRepository;
using Lich.api.Interface.IService;

namespace Lich.api.Implement.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;


        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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

        public async Task<ResUserDto> UpdateRoleUserAsync(int userId)
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
            }
            return new ResUserDto
            {
                Id = updatedUser.Id,
                Email = updatedUser.Email,
                FullName = updatedUser.FullName,
                RoleId = updatedUser.RoleId
            };
        }
    }
}