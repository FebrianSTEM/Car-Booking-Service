using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static car_booking_service.Domain.Constants.ValidationConstants;

namespace car_booking_service.Domain.Entities
{
    [Table("CarModels", Schema = "Master")]
    public class CarModel
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
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = SYSTEM_USER;
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set;} = SYSTEM_USER;
    }
}