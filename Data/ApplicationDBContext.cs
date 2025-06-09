using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lich.api.Model;
using Microsoft.EntityFrameworkCore;

namespace Lich.api.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        public DbSet<Model.User> Users { get; set; }
        public DbSet<Model.Role> Roles { get; set; }
        public DbSet<Model.Schedule> Schedules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Model.User>()
                .HasMany(u => u.CreatedSchedules)
                .WithOne(s => s.CreatedBy)
                .HasForeignKey(s => s.CreatedById);

            modelBuilder.Entity<Model.User>()
                .HasMany(u => u.ApprovedSchedules)
                .WithOne(s => s.ApprovedBy)
                .HasForeignKey(s => s.ApprovedById);
            modelBuilder.Entity<Schedule>()
                .Property(s => s.Status)
                .HasConversion<string>();
            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.CreatedBy)
                .WithMany(u => u.CreatedSchedules)
                .HasForeignKey(s => s.CreatedById)
                .OnDelete(DeleteBehavior.NoAction); // Hạn chế Cascade

            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.ApprovedBy)
                .WithMany(u => u.ApprovedSchedules)
                .HasForeignKey(s => s.ApprovedById)
                .OnDelete(DeleteBehavior.SetNull);

        }
    }

}