using Microsoft.EntityFrameworkCore;
using StatusTracking.Core.Entities;

namespace StatusTracking.Infrastructure.Data
{
    internal class ApplicationContext : DbContext
    { 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql($"Host=localhost; Database=db-postech-final; Username=postgres; Password=banana123;");

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
