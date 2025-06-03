using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lich.api.Model.Enum;

namespace Lich.api.Model
{
    public class Schedule
    {
        public int Id { get; set; }
        public string Company { get; set; }
        public string Title { get; set; }
        public string Component { get; set; }
        public string Location { get; set; }
        public string Note { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public ScheduleStatus Status { get; set; }

        public int CreatedById { get; set; }
        public virtual User CreatedBy { get; set; }

        public int? ApprovedById { get; set; }
        public virtual User? ApprovedBy { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}