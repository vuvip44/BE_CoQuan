using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lich.api.DTO.Response.User
{
    public class ResUserDto
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string FullName { get; set; }

        public int RoleId { get; set; }


    }
}