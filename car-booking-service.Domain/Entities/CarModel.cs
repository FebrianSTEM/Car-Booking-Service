using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static car_booking_service.Domain.Constants.ValidationConstants;

namespace car_booking_service.Domain.Entities
{
    [Table("CarModels", Schema = "Master")]
    public class CarModel : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CarId { get; set; }
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsAvailableForTestDrive { get; set; }

        // Navigation Property
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}