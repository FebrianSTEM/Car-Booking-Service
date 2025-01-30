namespace car_booking_service.Application.Models.Responses.BookingResponses
{
    public class BookingResponse
    {
        public int BookingId { get; set; }
        public int CarId { get; set; }
        public string CarModelBrand { get; set; } = string.Empty;
        public string CarModelName { get; set; } = string.Empty;
        public int CarModelYear { get; set; }
        public DateTime BookingDateTime { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; } = string.Empty;
        public DateTime UpdatedAt { get; set; }
    }
}