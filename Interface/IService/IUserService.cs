using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lich.api.DTO.Response.User;

namespace Lich.api.Interface.IService
{
    public interface IUserService
    {
        Task<ResUserDto> GetUserByIdAsync(int userId);
        Task<ResUserDto> UpdateRoleUserAsync(int userId);
        
    }
}