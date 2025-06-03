using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lich.api.DTO.Request.Query;
using Lich.api.DTO.Request.Schedule;
using Lich.api.DTO.Response.Schedule;

namespace Lich.api.Interface.IService
{
    public interface IScheduleService
    {
        Task<ResSchedule> CreateScheduleAsync(ReqSchedule reqSchedule, int userId);
        Task<ResSchedule> GetScheduleByIdAsync(int id);
        Task<List<ResSchedule>> GetSchedulesByWeekAsync(int week);
        Task<List<ResSchedule>> GetSchedulesBySearchAsync(QueryObject query);
        Task<List<ResSchedule>> GetPendingSchedulesAsync();
        Task<List<ResSchedule>> GetSchedulesByUserIdAsync(int userId);
        Task<bool> UpdateScheduleAsync(int id, string status, int userId);
        Task<bool> DeleteScheduleAsync(int id, int userId);
    }
}