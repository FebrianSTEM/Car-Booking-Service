﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace car_booking_service.Application.Models.Requests.BookingRequests
{
    public class GetBookingListRequest
    {
        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;

        public int? CarId { get; set; } = 0;
        public string CarBrand { get; set; } = string.Empty;
        public string CarModel { get; set; } = string.Empty;
        public int? CarYear { get; set; } = 0;
    }
}
