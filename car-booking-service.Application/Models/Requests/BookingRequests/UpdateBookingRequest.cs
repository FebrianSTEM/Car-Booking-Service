using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hyundai_testDriveBooking_service.Application.Models.Requests.BookingRequests
{
    public class UpdateBookingRequest
    {
        [Required(ErrorMessage = "Booking id is Required.")]
        public int BookingId { get; set; }

        [Required(ErrorMessage = "Car Model is Required.")]
        public int CarId { get; set; }

        [Required(ErrorMessage = "Customer Name is Required.")]
        public string CustomerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Customer Phone is Required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string CustomerPhone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email Address is Required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address format.")]
        public string CustomerEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please Select Booking Date and Time.")]
        public DateTime BookingDateTime { get; set; }
    }
}
