using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lich.api.Model
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}