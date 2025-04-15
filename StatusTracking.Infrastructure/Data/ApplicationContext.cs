using Microsoft.EntityFrameworkCore;
using StatusTracking.Core.Entities;

namespace StatusTracking.Infrastructure.Data
{
    internal class ApplicationContext : DbContext
    { 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql($"Host={Environment.GetEnvironmentVariable("DB_HOST")}; Database={Environment.GetEnvironmentVariable("DB_NAME")}; Username={Environment.GetEnvironmentVariable("DB_USER")}; Password={Environment.GetEnvironmentVariable("DB_PASSWORD")};");

            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<StatusTrackingAggregate> StatusTracking { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StatusTrackingAggregate>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
        }
    }
}
