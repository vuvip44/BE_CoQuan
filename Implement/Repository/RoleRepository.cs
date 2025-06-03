using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lich.api.Data;
using Lich.api.Interface.IRepository;
using Lich.api.Model;
using Microsoft.EntityFrameworkCore;

namespace Lich.api.Implement.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly ILogger<RoleRepository> _logger;
        public RoleRepository(ApplicationDBContext context, ILogger<RoleRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Role> CreateRoleAsync(Role role)
        {
            try
            {
                await _context.Roles.AddAsync(role);
                await _context.SaveChangesAsync();
                return role;
            }
            catch (System.Exception e)
            {
                _logger.LogError(e, "Error creating role");
                return null;
            }
        }

        public async Task<Role> GetRoleByIdAsync(int id)
        {
            try
            {
                return await _context.Roles.FindAsync(id);
            }
            catch (System.Exception e)
            {
                _logger.LogError(e, "Error retrieving role by ID");
                return null;
            }
        }

        public async Task<Role> GetRoleByNameAsync(string name)
        {
            try
            {
                return await _context.Roles
                    .FirstOrDefaultAsync(r => r.Name.Equals(name));
            }
            catch (System.Exception e)
            {
                _logger.LogError(e, "Error retrieving role by name");
                return null;
            }
        }
    }
}