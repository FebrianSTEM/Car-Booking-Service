using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hyundai_testDriveBooking_service.Application.Models.Requests.CarModelRequests
{
    public class UpdateCarModelRequest
    {
        public int CarId { get; set; }
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int? Year { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsAvailableForTestDrive { get; set; }
    }
}
