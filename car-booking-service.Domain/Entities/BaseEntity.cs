using static  car_booking_service.Domain.Constants.ValidationConstants;

namespace car_booking_service.Domain.Entities
{
    public class BaseEntity
    {
        public string CreatedBy { get; set; } = SYSTEM_USER;
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; } = SYSTEM_USER;
        public DateTime UpdatedAt { get; set; }
    }
}
