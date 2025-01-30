using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace car_booking_service.Application.Models.Requests
{
    public class BasePaginationRequest
    {
        [Required]
        public int Page { get; set; }

        [Required]
        public int PageSize { get; set; }
    }
}
