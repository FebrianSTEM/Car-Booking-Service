using hyundai_testDriveBooking_service.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace hyundai_testDriveBooking_service.Infrastructure.Data.Configurations
{
    public class BookingTestDriveConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.HasKey(x => x.BookingId);
            builder.Property(x => x.BookingId).ValueGeneratedOnAdd(); // Auto Increment

            builder.Property(x => x.CarId)
                   .IsRequired();

            builder.Property(x => x.CustomerName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.CustomerEmail)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(x => x.CustomerPhone)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(x => x.BookingDateTime)
                   .IsRequired();

            builder.Property(x => x.CreatedAt)
                   .IsRequired();

            builder.Property(x => x.CreatedBy)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(x => x.UpdatedAt)
                   .IsRequired();

            builder.Property(x => x.UpdatedBy)
                   .IsRequired()
                   .HasMaxLength(50);
        }
    }
}