using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lich.api.DTO.Request.Auth;
using Lich.api.DTO.Response.Auth;
using Lich.api.DTO.Response.User;

namespace Lich.api.Interface.IService
{
    public interface IAuthService
    {
        Task<TokenDto> LoginAsync(ReqLoginDto login);
        Task<TokenDto> RefreshTokenAsync(string refreshToken);
        Task LogoutAsync(string refreshToken);

        Task<ResUserDto> RegisterAsync(ReqRegisterDto register);
    }
}