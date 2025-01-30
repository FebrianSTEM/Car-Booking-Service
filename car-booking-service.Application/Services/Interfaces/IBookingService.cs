using hyundai_testDriveBooking_service.Application.Models.Requests.BookingRequests;
using hyundai_testDriveBooking_service.Application.Models.Responses.BookingResponses;
using hyundai_testDriveBooking_service.Domain.Entities;

namespace hyundai_testDriveBooking_service.Application.Services.Interfaces
{
    public interface IBookingService
    {
        Task<BookingResponse> CreateBookingAsync(CreateBookingRequest request);
        Task<BookingResponse> UpdateBookingAsync(UpdateBookingRequest request);
        Task<BookingResponse> GetBookingByIdAsync(int id);
        Task<IEnumerable<BookingResponse>> GetAllBookingAsync();
        Task DeleteBookingAsync(int id);
        Task<List<BookingResponse>> GetListAsync(GetBookingListRequest request);
        Task<bool> ValidateBookingSlotTime(CreateBookingRequest request);
        Task ValidateRequestUpdateBooking(UpdateBookingRequest request);
        Task<(bool, Booking)> ValidateBooking(int bookingId);
        Task<(bool, Booking)> ValidateDeleteBookingRequest(int bookingId);
        Task<(bool, CarModel)> ValidateCarModel(int carId);
        Task<(bool, CarModel)> ValidateRequestCreateBookingAsync(CreateBookingRequest request);
        Task<(List<BookingResponse>, int)> GetPaginatedAsync(GetPaginatedBookingsRequest request);
    }
}
