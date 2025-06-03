using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lich.api.DTO.Response.User;

namespace Lich.api.DTO.Response.Auth
{
    public class TokenDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
        public ResUserDto User { get; set; }
        public TokenDto(string accessToken, string refreshToken, DateTime refreshTokenExpiration, ResUserDto user)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            RefreshTokenExpiration = refreshTokenExpiration;
            User = user;
        }

        public TokenDto(string accessToken, string refreshToken, DateTime refreshTokenExpiration)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            RefreshTokenExpiration = refreshTokenExpiration;
           
        }
    }
}