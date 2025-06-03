using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lich.api.DTO.Request.Query
{
    public class QueryObject
    {
        public string? LeaderCompany { get; set; }
        public string? Title { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? Status { get; set; }
        public string? Location { get; set; }
        public DateTime? CreateAt { get; set; }

    }
}