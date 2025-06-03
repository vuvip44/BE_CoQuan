using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Lich.api.Common;
using Lich.api.DTO.Request.Auth;
using Lich.api.DTO.Response.Auth;
using Lich.api.DTO.Response.User;
using Lich.api.Interface.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lich.api.Controller
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        private readonly IUserService _userService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, IUserService userService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] ReqLoginDto dto)
        {
            if (!ModelState.IsValid)
            {
                var error = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                return BadRequest(new ApiResponse<string>(400, error));

            }
            var result = await _authService.LoginAsync(dto);

            if (result == null)
            {
                return Unauthorized(new ApiResponse<string>(401, "Invalid username or password"));
            }

            SetAccessTokenCookie(result.AccessToken);
            RefreshTokenCookie(result.RefreshToken);

            return Ok(new ApiResponse<TokenDto>(200, result, "Login successful"));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] ReqRegisterDto dto)
        {
            if (!ModelState.IsValid)
            {
                var error = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                return BadRequest(new ApiResponse<string>(400, error));
            }
            var result = await _authService.RegisterAsync(dto);
            return Ok(new ApiResponse<ResUserDto>(200, result, "Registration successful"));
        }

        [HttpPost("refresh-token")]
        
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refresh_token"];
            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized(new ApiResponse<string>(401, "Refresh token is missing"));
            }
            var result = await _authService.RefreshTokenAsync(refreshToken);

            if (result == null)
            {
                return Unauthorized(new ApiResponse<string>(401, "Invalid refresh token"));
            }
            SetAccessTokenCookie(result.AccessToken);
            RefreshTokenCookie(result.RefreshToken);
            return Ok(new ApiResponse<TokenDto>(200, result, "Token refreshed successfully"));
        }

        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            var refreshToken = Request.Cookies["refresh_token"];
            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized(new ApiResponse<string>(401, "Refresh token is missing"));
            }

            _authService.LogoutAsync(refreshToken).Wait();

            // Clear cookies
            Response.Cookies.Delete("access_token");
            Response.Cookies.Delete("refresh_token");

            return Ok(new ApiResponse<string>(200, "Logout successful"));
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new ApiResponse<string>(401, "User not authenticated"));
            }
            var user = await _userService.GetUserByIdAsync(int.Parse(userId));
            if (user == null)
            {
                return NotFound(new ApiResponse<string>(404, "User not found"));
            }
            return Ok(new ApiResponse<ResUserDto>(200, user, "User retrieved successfully"));
        }

        private void SetAccessTokenCookie(string accessToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(15) // Set cookie expiration
            };
            Response.Cookies.Append("access_token", accessToken, cookieOptions);
        }

        private void RefreshTokenCookie(string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(7) // Set cookie expiration
            };
            Response.Cookies.Append("refresh_token", refreshToken, cookieOptions);
        }
    }
}