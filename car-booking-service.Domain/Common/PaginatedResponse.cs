using static hyundai_testDriveBooking_service.Domain.Enums.Enums;

namespace hyundai_testDriveBooking_service.Domain.Common
{
    public class PaginatedResponse<T> : StandardResponse<T>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalData { get; set; }
        public int TotalPages => PageSize > 0
            ? (int)Math.Ceiling((double)TotalData / PageSize)
            : 0;

        public PaginatedResponse(
            StatusCode responseStatusCodeType,
            StatusMessage responseStatusMessageType,
            T? data,
            string message,
            int pageIndex,
            int pageSize,
            int totalData)
            : base(responseStatusCodeType, responseStatusMessageType, data, message)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalData = totalData;
        }
    }
}