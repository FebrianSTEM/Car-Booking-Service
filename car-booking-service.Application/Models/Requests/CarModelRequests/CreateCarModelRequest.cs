using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hyundai_testDriveBooking_service.Application.Models.Requests.CarModelRequests
{
    public class CreateCarModelRequest
    {
        [Required]
        public string Brand { get; set; } = string.Empty;

        [Required]
        public string Model { get; set; } = string.Empty;

        [Required]
        public int? Year { get; set; }

        [Required]
        public string ImageUrl { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }
}
