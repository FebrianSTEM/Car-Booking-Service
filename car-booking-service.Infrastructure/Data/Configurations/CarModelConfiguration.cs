using car_booking_service.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace car_booking_service.Infrastructure.Data.Configurations
{
    public class CarModelConfiguration : IEntityTypeConfiguration<CarModel>
    {
        public void Configure(EntityTypeBuilder<CarModel> builder)
        {
            builder.HasKey(x => x.CarId);
            builder.Property(x => x.CarId).ValueGeneratedOnAdd();

            builder.Property(x => x.Brand)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Model)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Year)
                .IsRequired();

            builder.Property(x => x.ImageUrl)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.Description)
               .HasMaxLength(500);

            builder.Property(x => x.IsAvailableForTestDrive)
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