using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lich.api.DTO.Request.Schedule
{
    public class ReqSchedule
    {
        public string LeaderCompany { get; set; }
        public string Title { get; set; }
        public string Component { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Location { get; set; }
        public string Note { get; set; }
        
    }
}