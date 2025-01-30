using hyundai_testDriveBooking_service.Application.Models.Requests.BookingRequests;
using hyundai_testDriveBooking_service.Application.Models.Responses.BookingResponses;
using hyundai_testDriveBooking_service.Application.Services.Interfaces;
using hyundai_testDriveBooking_service.Domain.Constants;
using hyundai_testDriveBooking_service.Domain.Entities;
using hyundai_testDriveBooking_service.Domain.Exception;
using hyundai_testDriveBooking_service.Domain.Interfaces;
using Mapster;
using static hyundai_testDriveBooking_service.Domain.Enums.Enums;

namespace hyundai_testDriveBooking_service.Application.Services.Implementations
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ICarModelRepository _carModelRepository;

        public BookingService(IBookingRepository bookingRepository, ICarModelRepository carModelRepository)
        {
            _bookingRepository = bookingRepository;
            _carModelRepository = carModelRepository;
        }

        public async Task<IEnumerable<BookingResponse>> GetAllBookingAsync()
        {
            IEnumerable<Booking> bookings = await _bookingRepository.GetAllAsync();
            List<int> carIds = bookings.Select(x => x.CarId).Distinct().ToList();
            var carModels = await _carModelRepository.GetListByIdsAsync(carIds);

            var result = bookings.Join(carModels, booking => booking.CarId,
                                                  carModel => carModel.CarId,
                                                  (booking, carModel) => new BookingResponse()
                                                  {
                                                      BookingId = booking.BookingId,
                                                      CarId = booking.CarId,
                                                      CarModelBrand = carModel.Brand,
                                                      CarModelName = carModel.Model,
                                                      CarModelYear = carModel.Year,
                                                      BookingDateTime = booking.BookingDateTime,
                                                      CustomerName = booking.CustomerName,
                                                      CustomerEmail = booking.CustomerEmail,
                                                      CustomerPhone = booking.CustomerPhone,
                                                      CreatedAt = booking.CreatedAt,
                                                      CreatedBy = booking.CreatedBy,
                                                      UpdatedAt = booking.UpdatedAt,
                                                      UpdatedBy = booking.UpdatedBy
                                                  });

            return result;
        }

        public async Task<BookingResponse> GetBookingByIdAsync(int id)
        {
            Booking? booking = await _bookingRepository.GetByIdAsync(id);
            if (booking == null)
                throw new HttpStatusCodeException((int)StatusCode.NotFound, $"Booking with ID {id} not found");

            var carModel = await _carModelRepository.GetByIdAsync(booking.CarId);
            if (carModel == null)
                throw new HttpStatusCodeException((int)StatusCode.NotFound, $"Car Model with ID {booking.CarId} not found");

            BookingResponse result = booking.Adapt<BookingResponse>();
            result.CarModelBrand = carModel.Brand;
            result.CarModelName = carModel.Model;
            result.CarModelYear = carModel.Year;

            return result;
        }

        public async Task<List<BookingResponse>> GetListAsync(GetBookingListRequest request)
        {
            //add validation Here
            var result = await _bookingRepository.GetListAsync(request.StartDate,
                                                               request.EndDate,
                                                               request.CarId,
                                                               request.CustomerName,
                                                               request.CustomerPhone,
                                                               request.CustomerEmail,
                                                               request.CarBrand,
                                                               request.CarModel,
                                                               request.CarYear);
            return result.Adapt<List<BookingResponse>>();
        }

        public async Task<(List<BookingResponse>, int)> GetPaginatedAsync(GetPaginatedBookingsRequest request)
        {
            //add validation Here
            var (result, totalCount) = await _bookingRepository.GetPaginatedAsync(
                                                               request.Page,
                                                               request.PageSize,
                                                               request.StartDate,
                                                               request.EndDate,
                                                               request.CarId,
                                                               request.CustomerName,
                                                               request.CustomerPhone,
                                                               request.CustomerEmail,
                                                               request.CarBrand,
                                                               request.CarModel,
                                                               request.CarYear);

            return (result.Adapt<List<BookingResponse>>(), totalCount);
        }

        public async Task<BookingResponse> CreateBookingAsync(CreateBookingRequest request)
        {
            (bool isValidated, CarModel carModel) = await ValidateRequestCreateBookingAsync(request);
            if (!isValidated)
                throw new HttpStatusCodeException((int)StatusCode.UnprocessableEntity, $"Car model not found or selected time already booked.");
            Booking bookingEntity = request.Adapt<Booking>();
            DateTime currentTime = DateTime.UtcNow;
            bookingEntity.CreatedAt = currentTime;
            bookingEntity.UpdatedAt = currentTime;
            await _bookingRepository.AddAsync(bookingEntity);

            var result = bookingEntity.Adapt<BookingResponse>();
            result.CarId = carModel.CarId;
            result.CarModelBrand = carModel.Brand;
            result.CarModelName = carModel.Model;
            result.CarModelYear = carModel.Year;

            return result;
        }

        public async Task<BookingResponse> UpdateBookingAsync(UpdateBookingRequest request)
        {
            var booking = await _bookingRepository.GetByIdAsync(request.BookingId)
                ?? throw new HttpStatusCodeException((int)StatusCode.UnprocessableEntity,
                    $"Booking not found for booking id {request.BookingId}");

            (bool isValid, CarModel carModel) = await ValidateCarModel(request.CarId);

            if (request.CarId != booking.CarId || request.BookingDateTime != booking.BookingDateTime)
            {
                await ValidateBookingSlotTime(request.Adapt<CreateBookingRequest>());
            }

            request.Adapt(booking);
            await _bookingRepository.UpdateAsync(booking);

            var result = booking.Adapt<BookingResponse>();
            result.CarModelBrand = carModel.Brand;
            result.CarModelName = carModel.Model;
            result.CarModelYear = carModel.Year;

            return result;
        }

        public async Task DeleteBookingAsync(int id)
        {
            (bool isValidated, Booking booking) = await ValidateDeleteBookingRequest(id);
            if (!isValidated)
                throw new HttpStatusCodeException((int)StatusCode.UnprocessableEntity, $"Cannot performed delete booking");
            await _bookingRepository.DeleteAsync(booking);
        }

        public async Task<(bool, CarModel)> ValidateRequestCreateBookingAsync(CreateBookingRequest request)
        {
            (bool icarModelValidated, CarModel carModel) = await ValidateCarModel(request.CarId.GetValueOrDefault(0));
            if (!icarModelValidated)
                throw new HttpStatusCodeException((int)StatusCode.NotFound, $"Selected Car Model with ID {request.CarId} not found");
            bool isBookingSlotTimeValidated = await ValidateBookingSlotTime(request);
            if (!isBookingSlotTimeValidated)
                throw new HttpStatusCodeException((int)StatusCode.UnprocessableEntity, $"Selected time already booked.");

            return (icarModelValidated && isBookingSlotTimeValidated, carModel);
        }

        public async Task<(bool, CarModel)> ValidateCarModel(int carId)
        {
            var carModel = await _carModelRepository.GetByIdAsync(carId);
            if (carModel == null)
                throw new HttpStatusCodeException((int)StatusCode.UnprocessableEntity, $"Selected Car Model with ID {carId} not found");
            return (true, carModel);
        }

        public async Task<bool> ValidateBookingSlotTime(CreateBookingRequest request)
        {
            DateTime startDate = request.BookingDateTime.AddMinutes(-1 * ValidationConstants.BOOKING_MINUTE_INTERVAL);
            DateTime endDate = request.BookingDateTime.AddMinutes(ValidationConstants.BOOKING_MINUTE_INTERVAL);

            var existingBookings = await _bookingRepository.GetListAsync(startDate,
                                                                         endDate,
                                                                         request.CarId);
            if (existingBookings.Any())
                throw new HttpStatusCodeException((int)StatusCode.UnprocessableEntity, $"There's Already Existing Booking for Selected Time.");

            return true;
        }

        public async Task ValidateRequestUpdateBooking(UpdateBookingRequest request)
        {
            CreateBookingRequest req = request.Adapt<CreateBookingRequest>();
            bool isBookingSlotTimeValidated = await ValidateBookingSlotTime(req);
            if (!isBookingSlotTimeValidated)
                throw new HttpStatusCodeException((int)StatusCode.UnprocessableEntity, $"Selected time already booked.");
            await ValidateCarModel(request.CarId);
        }

        public async Task<(bool, Booking)> ValidateBooking(int bookingId)
        {
            Booking? booking = await _bookingRepository.GetByIdAsync(bookingId);
            if (booking == null)
                throw new HttpStatusCodeException((int)StatusCode.UnprocessableEntity, $"Booking not found for booking id {bookingId}");

            return (true, booking);
        }

        public async Task<(bool, Booking)> ValidateDeleteBookingRequest(int bookingId)
        {
            (bool isBookingValidate, Booking booking) = await ValidateBooking(bookingId);
            (bool isCarExist, CarModel carModel) = await ValidateCarModel(booking.BookingId);

            return (isBookingValidate && isCarExist, booking);
        }
    }
}