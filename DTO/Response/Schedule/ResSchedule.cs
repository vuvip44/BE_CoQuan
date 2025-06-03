using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lich.api.DTO.Response.Schedule
{
    public class ResSchedule
    {
        public int Id { get; set; }
        public string LeaderCompany { get; set; }
        public string Title { get; set; }
        public string Component { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Location { get; set; }
        public string Status { get; set; } // Assuming Status is a string representation of an enum
        public string Note { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}