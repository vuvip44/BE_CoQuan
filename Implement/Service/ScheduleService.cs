using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lich.api.DTO.Request.Query;
using Lich.api.DTO.Request.Schedule;
using Lich.api.DTO.Response.Schedule;
using Lich.api.Implement.Repository;
using Lich.api.Interface.IService;
using Lich.api.Model;
using Lich.api.Model.Enum;

namespace Lich.api.Implement.Service
{
    public class ScheduleService : IScheduleService
    {
        private readonly IScheduleRepository _scheduleRepository;
        public ScheduleService(IScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }
        public async Task<ResSchedule> CreateScheduleAsync(ReqSchedule reqSchedule, int userId)
        {
            if (reqSchedule == null)
            {
                throw new ArgumentNullException(nameof(reqSchedule), "Request schedule cannot be null");
            }
            if (userId <= 0)
            {
                throw new ArgumentException("Invalid user ID", nameof(userId));
            }
            var schedule = new Schedule
            {
                Company = reqSchedule.LeaderCompany,
                Title = reqSchedule.Title,
                Component = reqSchedule.Component,
                StartTime = reqSchedule.StartTime,
                EndTime = reqSchedule.EndTime,
                Location = reqSchedule.Location,
                Note = reqSchedule.Note,
                CreatedAt = DateTime.UtcNow,
                CreatedById = userId,
                Status = ScheduleStatus.Pending // Assuming default status is Pending

            };
            var newSchedule = await _scheduleRepository.CreateScheduleAsync(schedule);
            if (newSchedule == null)
            {
                throw new Exception("Failed to create schedule");
            }
            return new ResSchedule
            {
                Id = newSchedule.Id,
                LeaderCompany = newSchedule.Company,
                Title = newSchedule.Title,
                Component = newSchedule.Component,
                StartTime = newSchedule.StartTime,
                EndTime = newSchedule.EndTime,
                Location = newSchedule.Location,
                Note = newSchedule.Note,
                CreatedAt = newSchedule.CreatedAt,
                Status = newSchedule.Status.ToString() // Assuming Status is an enum
            };
        }

        public async Task<bool> DeleteScheduleAsync(int id, int userId)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid schedule ID", nameof(id));
            }
            if (userId <= 0)
            {
                throw new ArgumentException("Invalid user ID", nameof(userId));
            }
            var schedule = await _scheduleRepository.GetScheduleByIdAsync(id);
            if (schedule.Status != ScheduleStatus.Pending)
            {
                throw new Exception("Failed to delete schedule");
            }
            return await _scheduleRepository.DeleteScheduleAsync(id, userId);
        }

        public async Task<List<ResSchedule>> GetPendingSchedulesAsync()
        {
            var schedules = await _scheduleRepository.GetPendingSchedulesIdAsync();
            if (schedules == null || !schedules.Any())
            {
                return new List<ResSchedule>();
            }
            return schedules.Select(s => new ResSchedule
            {
                Id = s.Id,
                LeaderCompany = s.Company,
                Title = s.Title,
                Component = s.Component,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                Location = s.Location,
                Note = s.Note,
                CreatedAt = s.CreatedAt,
                Status = s.Status.ToString() // Assuming Status is an enum
            }).ToList();
        }

        public async Task<ResSchedule> GetScheduleByIdAsync(int id)
        {
            var schedule = await _scheduleRepository.GetScheduleByIdAsync(id);
            if (schedule == null)
            {
                throw new KeyNotFoundException($"Schedule with ID {id} not found");
            }
            return new ResSchedule
            {
                Id = schedule.Id,
                LeaderCompany = schedule.Company,
                Title = schedule.Title,
                Component = schedule.Component,
                StartTime = schedule.StartTime,
                EndTime = schedule.EndTime,
                Location = schedule.Location,
                Note = schedule.Note,
                CreatedAt = schedule.CreatedAt,
                Status = schedule.Status.ToString() // Assuming Status is an enum
            };
        }

        public async Task<List<ResSchedule>> GetSchedulesBySearchAsync(QueryObject query)
        {

            var schedules = await _scheduleRepository.GetSchedulesBySearchAsync(query);
            if (schedules == null || !schedules.Any())
            {
                return new List<ResSchedule>();
            }
            return schedules.Select(s => new ResSchedule
            {
                Id = s.Id,
                LeaderCompany = s.Company,
                Title = s.Title,
                Component = s.Component,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                Location = s.Location,
                Note = s.Note,
                CreatedAt = s.CreatedAt,
                Status = s.Status.ToString() // Assuming Status is an enum
            }).ToList();
        }

        public async Task<List<ResSchedule>> GetSchedulesByUserIdAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("Invalid user ID", nameof(userId));
            }
            var schedules = await _scheduleRepository.GetSchedulesByUserIdAsync(userId);
            if (schedules == null || !schedules.Any())
            {
                return new List<ResSchedule>();
            }
            return schedules.Select(s => new ResSchedule
            {
                Id = s.Id,
                LeaderCompany = s.Company,
                Title = s.Title,
                Component = s.Component,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                Location = s.Location,
                Note = s.Note,
                CreatedAt = s.CreatedAt,
                Status = s.Status.ToString() // Assuming Status is an enum
            }).ToList();
        }

        public async Task<List<ResSchedule>> GetSchedulesByWeekAsync(int week)
        {
            if (week < 1 || week > 53)
            {
                throw new ArgumentOutOfRangeException(nameof(week), "Week must be between 1 and 53");
            }
            var schedules = await _scheduleRepository.GetSchedulesByWeekAsync(week);
            if (schedules == null || !schedules.Any())
            {
                return new List<ResSchedule>();
            }
            return schedules.Select(s => new ResSchedule
            {
                Id = s.Id,
                LeaderCompany = s.Company,
                Title = s.Title,
                Component = s.Component,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                Location = s.Location,
                Note = s.Note,
                CreatedAt = s.CreatedAt,
                Status = s.Status.ToString() // Assuming Status is an enum
            }).ToList();
        }

        public async Task<bool> UpdateScheduleAsync(int id, int userId, ReqSchedule req)
        {
            var schedule = await _scheduleRepository.GetScheduleByIdAsync(id);
            if (schedule == null)
            {
                throw new KeyNotFoundException($"Schedule with ID {id} not found");
            }
            if (userId <= 0 || userId != schedule.CreatedById)
            {
                throw new ArgumentException("Invalid user ID", nameof(userId));
            }
            if (schedule.Status != ScheduleStatus.Pending)
            {
                throw new Exception("Failed to delete schedule");
            }

            schedule.Company = req.LeaderCompany;
            schedule.Title = req.Title;
            schedule.Component = req.Component;
            schedule.StartTime = req.StartTime;
            schedule.EndTime = req.EndTime;
            schedule.Location = req.Location;
            schedule.Note = req.Note;

            var result = await _scheduleRepository.UpdateScheduleAsync(schedule);
            if (!result)
            {
                throw new Exception("Failed to update schedule");
            }
            return true;
        }

        public async Task<bool> UpdateScheduleStatusAsync(int id, string status, int userId)
        {
            var schedule = await _scheduleRepository.GetScheduleByIdAsync(id);
            if (schedule == null)
            {
                throw new KeyNotFoundException($"Schedule with ID {id} not found");
            }
            if (userId <= 0)
            {
                throw new ArgumentException("Invalid user ID", nameof(userId));
            }
            if (!Enum.TryParse<ScheduleStatus>(status, true, out var scheduleStatus))
            {
                throw new ArgumentException("Invalid status value", nameof(status));
            }
            schedule.Status = scheduleStatus;
            schedule.ApprovedById = userId; // Assuming the user approving the schedule is the one updating it
            var result = await _scheduleRepository.UpdateScheduleAsync(schedule);
            if (!result)
            {
                throw new Exception("Failed to update schedule");
            }
            return true;
        }
    }
}