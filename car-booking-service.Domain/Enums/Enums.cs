namespace car_booking_service.Domain.Enums
{
    public class Enums
    {
        public enum StatusCode
        {
            OK = 200,
            Created = 201,
            NoContent = 204,
            NotModified = 304,
            BadRequest = 400,
            Unauthorized = 401,
            Forbidden = 403,
            NotFound = 404,
            MethodNotAllowed = 405,
            Gone = 410,
            UnsupportedMediaType = 415,
            UnprocessableEntity = 422,
            TooManyRequests = 429,
            InternalServerErrorException = 500,
            ServiceUnavilableException = 503
        }

        public enum StatusMessage
        {
            Success,
            Fail,
            Error
        }
    }
}