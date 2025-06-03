using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lich.api.Model;

namespace Lich.api.Interface.IService
{
    public interface ITokenService
    {
        string GenerateAccessTokenAsync(User user);
        string GenerateRefreshTokenAsync();
    }
}