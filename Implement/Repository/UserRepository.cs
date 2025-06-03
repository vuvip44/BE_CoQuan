using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lich.api.Data;
using Lich.api.Interface.IRepository;
using Lich.api.Model;
using Microsoft.EntityFrameworkCore;

namespace Lich.api.Implement.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly ILogger<UserRepository> _logger;
        public UserRepository(ApplicationDBContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<User> CreateUserAsync(User user)
        {
            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return user;
            }
            catch (System.Exception e)
            {
                _logger.LogError(e, "Error creating user");
                return null;
            }
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    _logger.LogWarning($"User with ID {id} not found for deletion.");
                    return false;
                }
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (System.Exception e)
            {
                _logger.LogError(e, $"Error deleting user with ID {id}");
                return false;
            }
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            try
            {
                return await _context.Users
                    .Include(u => u.Role) // Assuming User has a Role navigation property
                    .FirstOrDefaultAsync(u => u.Email.Equals(email));
            }
            catch (System.Exception e)
            {
                _logger.LogError(e, "Error retrieving user by email");
                return null;
            }
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            try
            {
                return await _context.Users
                    .Include(u => u.Role) // Assuming User has a Role navigation property
                    .FirstOrDefaultAsync(u => u.Id == id);
            }
            catch (System.Exception e)
            {
                _logger.LogError(e, $"Error retrieving user by ID {id}");
                return null;
            }
        }

        public async Task<User> GetUserByRefreshTokenAsync(string refreshToken)
        {
            try
            {
                return await _context.Users
                    .Include(u => u.Role) // Assuming User has a Role navigation property
                    .FirstOrDefaultAsync(u => u.RefreshToken.Equals(refreshToken)
                        && u.RefreshTokenExpiryTime > DateTime.UtcNow);
            }
            catch (System.Exception e)
            {
                _logger.LogError(e, "Error retrieving user by refresh token");
                return null;
            }
        }

        public async Task<bool> IsExistUserAsync(string email)
        {
            try
            {
                return await _context.Users
                    .AnyAsync(u => u.Email.Equals(email));
            }
            catch (System.Exception e)
            {
                _logger.LogError(e, "Error checking if user exists by email");
                return false;
            }
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            try
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return user;
            }
            catch (System.Exception e)
            {
                _logger.LogError(e, "Error updating user");
                return null;
            }
        }
    }
}