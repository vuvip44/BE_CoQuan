using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Lich.api.Common;
using Lich.api.DTO.Request.Query;
using Lich.api.DTO.Request.Schedule;
using Lich.api.DTO.Response.Schedule;
using Lich.api.Interface.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lich.api.Controller
{
    [ApiController]
    [Route("api/schedules")]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;
        public ScheduleController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> CreateSchedule([FromBody] DTO.Request.Schedule.ReqSchedule reqSchedule)
        {
            if (reqSchedule == null)
            {
                return BadRequest("Invalid schedule data.");
            }

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var result = await _scheduleService.CreateScheduleAsync(reqSchedule, userId);
            return Ok(new ApiResponse<ResSchedule>(200, result, "Schedule created successfully."));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetScheduleById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid schedule ID.");
            }

            var result = await _scheduleService.GetScheduleByIdAsync(id);
            if (result == null)
            {
                return NotFound("Schedule not found.");
            }

            return Ok(new ApiResponse<ResSchedule>(200, result, "Schedule retrieved successfully."));
        }

        [HttpGet("week/{week}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetSchedulesByWeek(int week)
        {
            if (week < 1 || week > 53)
            {
                return BadRequest("Invalid week number.");
            }

            var result = await _scheduleService.GetSchedulesByWeekAsync(week);
            return Ok(new ApiResponse<List<ResSchedule>>(200, result, "Schedules for the week retrieved successfully."));
        }

        [HttpGet("search")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetSchedulesBySearch([FromQuery] QueryObject query)
        {
            if (query == null)
            {
                return BadRequest("Invalid search criteria.");
            }

            var result = await _scheduleService.GetSchedulesBySearchAsync(query);
            return Ok(new ApiResponse<List<ResSchedule>>(200, result, "Schedules retrieved successfully."));

        }

        [HttpGet("pending")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetPendingSchedules()
        {
            var result = await _scheduleService.GetPendingSchedulesAsync();
            return Ok(new ApiResponse<List<ResSchedule>>(200, result, "Pending schedules retrieved successfully."));
        }

        

        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateScheduleStatus(int id, [FromBody] string status)
        {
            if (id <= 0 || string.IsNullOrEmpty(status))
            {
                return BadRequest("Invalid schedule ID or status.");
            }

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var result = await _scheduleService.UpdateScheduleStatusAsync(id, status, userId);
            if (!result)
            {
                return NotFound("Schedule not found or could not be updated.");
            }

            return Ok(new ApiResponse<bool>(200, true, "Schedule status updated successfully."));
        }

        [HttpGet("get-current-user")]
        [Authorize]
        public async Task<IActionResult> GetScheduleByCurrentUser()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            if (userId == 0)
            {
                return Unauthorized(new ApiResponse<string>(401, "User dont login"));
            }
            var result = await _scheduleService.GetSchedulesByUserIdAsync(userId);
            return Ok(new ApiResponse<List<ResSchedule>>(200, result, "Get schedules success"));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> DeleteScheduleAsync(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            if (userId == 0)
            {
                return Unauthorized(new ApiResponse<string>(401, "User dont login"));
            }
            var result = await _scheduleService.DeleteScheduleAsync(id, userId);
            return Ok(new ApiResponse<bool>(200, true, "Schedule status delete successfully."));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> UpdateScheduleAsync(int id, [FromBody] ReqSchedule req)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var result = await _scheduleService.UpdateScheduleAsync(id, userId, req);
            if (!result)
            {
                return NotFound("Schedule not found or could not be updated.");
            }
            return Ok(new ApiResponse<bool>(200, true, "Schedule status updated successfully."));
        }
    }
}