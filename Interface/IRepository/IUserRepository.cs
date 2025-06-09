using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lich.api.DTO.Request.Query;
using Lich.api.Model;

namespace Lich.api.Interface.IRepository
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> CreateUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(int id);
        Task<User> GetUserByRefreshTokenAsync(string refreshToken);
        Task<bool> IsExistUserAsync(string email);
        Task<IEnumerable<User>> GetAllUsersAsync(QueryUser query);
    }
}