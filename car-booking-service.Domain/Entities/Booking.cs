﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static car_booking_service.Domain.Constants.ValidationConstants;
using System.Text.Json.Serialization;

namespace car_booking_service.Domain.Entities
{
    [Table("TestDrive", Schema = "BookingTrx")]
    public class Booking : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookingId { get; set; }
        [ForeignKey(nameof(CarModel))]
        public int CarId { get; set; }

        // Navigation Property
        public virtual CarModel CarModel { get; set; }

        public DateTime BookingDateTime { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;

        [JsonIgnore]
        [NotMapped]
        public string CarModelName { get; set; } = string.Empty;

        [JsonIgnore]
        [NotMapped]
        public string CarModelBrand { get; set; } = string.Empty;

        [JsonIgnore]
        [NotMapped]
        public int CarModelYear { get; set; }
    }
}