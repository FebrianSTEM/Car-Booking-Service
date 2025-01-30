using System.ComponentModel.DataAnnotations;

namespace car_booking_service.Domain.Exception
{
    public class HttpStatusCodeException : IOException
    {
        public int StatusCode { get; }

        public HttpStatusCodeException(int statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}