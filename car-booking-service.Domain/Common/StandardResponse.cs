using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static hyundai_testDriveBooking_service.Domain.Enums.Enums;

namespace hyundai_testDriveBooking_service.Domain.Common
{
    public class StandardResponse<T> : HttpResult
    {
        public string Status { get; set; }
        public int Code { get; set; }
        public T? Data { get; set; }
        public string Message { get; set; }

        public StandardResponse(StatusCode responseStatusCodeType,
                               StatusMessage responseStatusMessageType,
                               T? data,
                               string message) : base(responseStatusCodeType, responseStatusMessageType)
        {
            Status = GetResponseStatusMessage();
            Code = GetResponseStatusCode();
            Message = message;
            Data = data;
        }
    }
}