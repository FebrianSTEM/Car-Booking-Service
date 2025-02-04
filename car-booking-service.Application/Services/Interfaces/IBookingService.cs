using car_booking_service.Application.Models.Requests.BookingRequests;
using car_booking_service.Application.Models.Responses.BookingResponses;
using car_booking_service.Domain.Entities;

namespace car_booking_service.Application.Services.Interfaces
{
    public interface IBookingService
    {
        Task<BookingResponse> CreateBookingAsync(CreateBookingRequest request);
        Task<BookingResponse> UpdateBookingAsync(UpdateBookingRequest request);
        Task<BookingResponse> GetBookingByIdAsync(int id);
        Task<IEnumerable<BookingResponse>> GetAllBookingAsync();
        Task DeleteBookingAsync(int id);
        Task<List<BookingResponse>> GetListAsync(GetBookingListRequest request);
        Task ValidateBookingSlotTime(CreateBookingRequest request);
        Task ValidateRequestUpdateBooking(UpdateBookingRequest request);
        Task<Booking> ValidateBooking(int bookingId);
        Task<Booking> ValidateDeleteBookingRequest(int bookingId);
        Task<CarModel> ValidateCarModel(int carId);
        Task<CarModel> ValidateRequestCreateBookingAsync(CreateBookingRequest request);
        Task<(List<BookingResponse>, int)> GetPaginatedAsync(GetPaginatedBookingsRequest request);
    }
}
