using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BE_CoQuan.DTO.Request.Auth;
using Lich.api.Common;
using Lich.api.DTO.Request.Auth;
using Lich.api.DTO.Response.User;
using Lich.api.Interface.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lich.api.Controller
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;
        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPut("{id}/role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateRoleUser(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid schedule ID or status.");
            }
            var result = await _userService.UpdateRoleUserAsync(id);
            if (!result)
            {
                return NotFound("User not found or could not be updated.");
            }
            return Ok(new ApiResponse<bool>(200, true, "User role updated successfully."));
        }

        [HttpPut("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePasswordByUser([FromBody] ReqPasswordDto dto)
        {
            if (!ModelState.IsValid)
            {
                var error = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                return BadRequest(new ApiResponse<string>(400, error));
            }
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var result = await _userService.UpdatePasswordByCurrentUser(userId, dto);
            if (!result)
            {
                return NotFound("User not found or could not be updated.");
            }
            return Ok(new ApiResponse<bool>(200, true, "User password updated successfully."));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result)
            {
                return NotFound("User not found or could not be updated.");
            }
            return Ok(new ApiResponse<bool>(200, true, "User delete successfully."));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUserAsync([FromBody] ReqRegisterDto dto)
        {
            if (!ModelState.IsValid)
            {
                var error = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                return BadRequest(new ApiResponse<string>(400, error));
            }
            var result = await _userService.CreateUserAsync(dto);
            return Ok(new ApiResponse<ResUserDto>(200, result, "Create user success"));
        }

    }
}