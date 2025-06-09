using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lich.api.DTO.Request.Query;
using Lich.api.Model;

namespace Lich.api.Implement.Repository
{
    public interface IScheduleRepository
    {
        Task<Schedule> GetScheduleByIdAsync(int id);
        Task<Schedule> CreateScheduleAsync(Schedule schedule);
        Task<bool> UpdateScheduleAsync(Schedule schedule);
        Task<bool> DeleteScheduleAsync(int id, int userId);
        Task<List<Schedule>> GetSchedulesByWeekAsync(int week);

        Task<List<Schedule>> GetSchedulesBySearchAsync(QueryObject query);
        Task<List<Schedule>> GetPendingSchedulesIdAsync();
        Task<List<Schedule>> GetSchedulesByUserIdAsync(int userId);

    }
}