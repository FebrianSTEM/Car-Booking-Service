using hyundai_testDriveBooking_service.Domain.Entities;
using hyundai_testDriveBooking_service.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;


namespace hyundai_testDriveBooking_service.Infrastructure.Data.Context
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
    }
}