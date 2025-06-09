using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE_CoQuan.DTO.Request.Auth;
using Lich.api.DTO.Request.Auth;
using Lich.api.DTO.Request.Query;
using Lich.api.DTO.Response.User;
using Lich.api.Model;

namespace Lich.api.Interface.IService
{
    public interface IUserService
    {
        Task<ResUserDto> GetUserByIdAsync(int userId);
        Task<bool> UpdateRoleUserAsync(int userId);
        Task<bool> UpdatePasswordByCurrentUser(int userId, ReqPasswordDto dto);
        Task<bool> DeleteUserAsync(int user);
        Task<ResUserDto> GetUserByUsernameAsync(string username);
        Task<ResUserDto> CreateUserAsync(ReqRegisterDto dto);
        Task<List<ResUserDto>> GetAllUsersAsync(QueryUser query);
        
    }
}