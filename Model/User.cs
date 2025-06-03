using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lich.api.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string? RefreshToken { get; set; }=string.Empty;
        public DateTime? RefreshTokenExpiryTime { get; set; }=null;
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }

        public virtual ICollection<Schedule> CreatedSchedules { get; set; }
        public virtual ICollection<Schedule> ApprovedSchedules { get; set; }
    }
}