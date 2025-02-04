using car_booking_service.Domain.Entities;
using car_booking_service.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;
using static car_booking_service.Domain.Constants.ValidationConstants;


namespace car_booking_service.Infrastructure.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<CarModel> CarModels { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CarModelConfiguration());
            modelBuilder.ApplyConfiguration(new BookingTestDriveConfiguration());
        }

        public override int SaveChanges()
        {
            // Get all entries in the ChangeTracker that are of type BaseEntity
            var entries = ChangeTracker.Entries<BaseEntity>();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.Now;
                    entry.Entity.CreatedBy = SYSTEM_USER;
                    entry.Entity.UpdatedAt = entry.Entity.CreatedAt;
                    entry.Entity.UpdatedBy = SYSTEM_USER;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTime.Now;
                    entry.Entity.UpdatedBy = SYSTEM_USER;
                }
            }
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<BaseEntity>();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.Now;
                    entry.Entity.CreatedBy = SYSTEM_USER;
                    entry.Entity.UpdatedAt = entry.Entity.CreatedAt;
                    entry.Entity.UpdatedBy = SYSTEM_USER;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTime.Now;
                    entry.Entity.UpdatedBy = SYSTEM_USER;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}