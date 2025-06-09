using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lich.api.DTO.Request.Query
{
    public class QueryUser
    {
        public string? Email { get; set; }
        public string? FullName { get; set; }
    }
}