using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace car_booking_service.Application.Models.Requests.CarModelRequests
{
    public class GetCarModelListRequest
    {
        public string? Brand { get; set; } = string.Empty;
        public string? Model { get; set; } = string.Empty;
        public int? Year { get; set; } = 0;
        public string? Description { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }
    }
}
