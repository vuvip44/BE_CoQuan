using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lich.api.Data;
using Lich.api.DTO.Request.Query;
using Lich.api.Model;
using Lich.api.Model.Enum;
using Microsoft.EntityFrameworkCore;

namespace Lich.api.Implement.Repository
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly ILogger<ScheduleRepository> _logger;
        public ScheduleRepository(ApplicationDBContext context, ILogger<ScheduleRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Schedule> CreateScheduleAsync(Schedule schedule)
        {
            try
            {
                await _context.Schedules.AddAsync(schedule);
                await _context.SaveChangesAsync();
                return schedule;
            }
            catch (System.Exception e)
            {
                _logger.LogError(e, "Error creating schedule");
                return null;
            }
        }

        public async Task<bool> DeleteScheduleAsync(int id)
        {
            try
            {
                var schedule = await _context.Schedules.FindAsync(id);
                if (schedule == null)
                {
                    _logger.LogWarning($"Schedule with ID {id} not found for deletion.");
                    return false;
                }
                _context.Schedules.Remove(schedule);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (System.Exception e)
            {
                _logger.LogError(e, $"Error deleting schedule with ID {id}");
                return false;
            }
        }

        public async Task<List<Schedule>> GetPendingSchedulesIdAsync()
        {

            try
            {
                return await _context.Schedules
                    .Include(s => s.CreatedBy)
                    .Where(s => s.Status == ScheduleStatus.Pending)

                    .ToListAsync();
            }
            catch (System.Exception e)
            {
                _logger.LogError(e, "Error retrieving pending schedules");
                return new List<Schedule>();
            }
        }

        public async Task<Schedule> GetScheduleByIdAsync(int id)
        {
            try
            {
                return await _context.Schedules
                    .Include(s => s.CreatedBy) // Assuming CreatedBy is a navigation property
                    .FirstOrDefaultAsync(s => s.Id == id);
            }
            catch (System.Exception e)
            {
                _logger.LogError(e, $"Error retrieving schedule by ID {id}");
                return null;
            }
        }

        public async Task<List<Schedule>> GetSchedulesBySearchAsync(QueryObject query)
        {
            try
            {
                var schedulesQuery = _context.Schedules
                    .Include(s => s.CreatedBy) // Assuming CreatedBy is a navigation property
                    .AsQueryable();

                if (!string.IsNullOrEmpty(query.Location))
                {
                    schedulesQuery = schedulesQuery.Where(s => s.Location.Contains(query.Location));
                }
                if (!string.IsNullOrEmpty(query.LeaderCompany))
                {
                    schedulesQuery = schedulesQuery.Where(s => s.Company == query.LeaderCompany);
                }
                if (query.StartTime.HasValue)
                {
                    schedulesQuery = schedulesQuery.Where(s => s.StartTime >= query.StartTime.Value);
                }
                if (query.EndTime.HasValue)
                {
                    schedulesQuery = schedulesQuery.Where(s => s.EndTime <= query.EndTime.Value);
                }
                if (query.CreateAt.HasValue)
                {
                    schedulesQuery = schedulesQuery.Where(s => s.CreatedAt >= query.CreateAt.Value);
                }


                if (!string.IsNullOrEmpty(query.Status))
                {
                    schedulesQuery = schedulesQuery.Where(s => s.Status == (ScheduleStatus)Enum.Parse(typeof(ScheduleStatus), query.Status, true));
                }

                var result = await schedulesQuery
                    .OrderByDescending(s => s.CreatedAt)

                    .ToListAsync();
                return result;


            }
            catch (System.Exception e)
            {
                _logger.LogError(e, "Error retrieving schedules by search criteria");
                return new List<Schedule>();
            }
        }

        public async Task<List<Schedule>> GetSchedulesByUserIdAsync(int userId)
        {
            try
            {
                var schedules = await _context.Schedules
                    .Include(s => s.CreatedBy) // Assuming CreatedBy is a navigation property
                    .Where(s => s.CreatedById == userId)
                    .ToListAsync();
                return schedules;
            }
            catch (System.Exception e)
            {
                _logger.LogError(e, $"Error retrieving schedules for user ID {userId}");
                return new List<Schedule>();
            }
        }

        // public async Task<List<Schedule>> GetSchedulesByWeekAsync(int week)
        // {
        //     var startOfWeek = DateTime.Today.AddDays(-7 * week).Date;
        //     var endWeek = startOfWeek.AddDays(7).Date;
        //     var schedules = await _context.Schedules
        //         .Include(s => s.CreatedBy) // Assuming CreatedBy is a navigation property
        //         .Where(s => s.StartTime >= startOfWeek && s.StartTime < endWeek)
        //         .OrderBy(s => s.StartTime)
        //         .ToListAsync();
        //     return schedules;
        // }
        public async Task<List<Schedule>> GetSchedulesByWeekAsync(int week)
        {
            var year = DateTime.Today.Year; // hoặc nhận thêm tham số năm nếu cần
            var firstDayOfYear = new DateTime(year, 1, 1);
            var startOfWeek = firstDayOfYear.AddDays((week - 1) * 7);
            var endWeek = startOfWeek.AddDays(7);

            var schedules = await _context.Schedules
                .Include(s => s.CreatedBy)
                .Where(s => s.StartTime >= startOfWeek && s.StartTime < endWeek)
                .OrderBy(s => s.StartTime)
                .ToListAsync();
            return schedules;
        }

        public async Task<bool> UpdateScheduleAsync(Schedule schedule)
        {
            try
            {
                _context.Schedules.Update(schedule);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (System.Exception e)
            {
                _logger.LogError(e, $"Error updating schedule with ID {schedule.Id}");
                return false;
            }
        }
    }
}