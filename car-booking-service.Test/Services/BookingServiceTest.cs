using Bogus;
using FakeItEasy;
using Xunit;
using car_booking_service.Domain.Interfaces;
using car_booking_service.Application.Services.Interfaces;
using car_booking_service.Domain.Entities;
using car_booking_service.Application.Services.Implementations;
using FluentAssertions;
using car_booking_service.Application.Models.Requests.BookingRequests;
using car_booking_service.Domain.Exception;
using car_booking_service.Domain.Constants;

namespace car_booking_service.Test.Services
{
    public class BookingServiceTests
    {
        private readonly IBookingRepository _fakeBookingRepository;
        private readonly ICarModelRepository _fakeCarModelRepository;
        private readonly IBookingService _bookingService;
        private readonly Faker<Booking> _bookingFaker;
        private readonly Faker<CarModel> _carModelFaker;
        private readonly Faker<UpdateBookingRequest> _updateRequestFaker;
        private readonly Faker<CreateBookingRequest> _createBookingRequestFaker;

        public BookingServiceTests()
        {
            _fakeBookingRepository = A.Fake<IBookingRepository>();
            _fakeCarModelRepository = A.Fake<ICarModelRepository>();
            _bookingService = new BookingService(_fakeBookingRepository, _fakeCarModelRepository);

            _carModelFaker = new Faker<CarModel>()
                .RuleFor(c => c.CarId, f => f.Random.Int(1, 100))
                .RuleFor(c => c.Brand, f => f.Vehicle.Manufacturer())
                .RuleFor(c => c.Model, f => f.Vehicle.Model())
                .RuleFor(c => c.Year, f => f.Random.Int(2000, 2024));

            _bookingFaker = new Faker<Booking>()
                .RuleFor(b => b.BookingId, f => f.Random.Int(1, 100))
                .RuleFor(b => b.CarId, f => f.Random.Int(1, 100))
                .RuleFor(b => b.BookingDateTime, f => f.Date.Future())
                .RuleFor(b => b.CustomerName, f => f.Name.FullName())
                .RuleFor(b => b.CustomerEmail, f => f.Internet.Email())
                .RuleFor(b => b.CustomerPhone, f => f.Phone.PhoneNumber())
                .RuleFor(b => b.CreatedAt, f => f.Date.Past())
                .RuleFor(b => b.UpdatedAt, f => f.Date.Recent())
                .RuleFor(b => b.CreatedBy, f => f.Name.FullName())
                .RuleFor(b => b.UpdatedBy, f => f.Name.FullName());

            _updateRequestFaker = new Faker<UpdateBookingRequest>()
            .RuleFor(r => r.BookingId, f => f.Random.Number(1, 100))
            .RuleFor(r => r.CarId, f => f.Random.Number(1, 100))
            .RuleFor(r => r.BookingDateTime, f => f.Date.Future());

            _createBookingRequestFaker = new Faker<CreateBookingRequest>()
               .RuleFor(r => r.CarId, f => f.Random.Number(1, 100))
               .RuleFor(r => r.BookingDateTime, f => f.Date.Future())
               .RuleFor(r => r.CustomerName, f => f.Name.FullName())
               .RuleFor(r => r.CustomerEmail, f => f.Internet.Email())
               .RuleFor(r => r.CustomerPhone, f => f.Phone.PhoneNumber());
        }

        [Fact]
        public async Task GetAllBookingAsync_ShouldReturnAllBookingsWithCarDetails()
        {
            // Arrange
            var bookings = _bookingFaker.Generate(3);
            var carModels = _carModelFaker.Generate(3);

            for (int i = 0; i < bookings.Count; i++)
            {
                bookings[i].CarId = carModels[i].CarId;
            }

            A.CallTo(() => _fakeBookingRepository.GetAllAsync())
                .Returns(bookings);
            A.CallTo(() => _fakeCarModelRepository.GetListByIdsAsync(A<List<int>>._))
                .Returns(carModels);

            // Act
            var result = await _bookingService.GetAllBookingAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(3);
            result.First().CarModelBrand.Should().NotBeNullOrEmpty();
            result.First().CarModelName.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GetBookingByIdAsync_WithValidId_ShouldReturnBooking()
        {
            // Arrange
            var booking = _bookingFaker.Generate();
            var carModel = _carModelFaker.Generate();
            booking.CarId = carModel.CarId;

            A.CallTo(() => _fakeBookingRepository.GetByIdAsync(booking.BookingId))
                .Returns(booking);
            A.CallTo(() => _fakeCarModelRepository.GetByIdAsync(booking.CarId))
                .Returns(carModel);

            // Act
            var result = await _bookingService.GetBookingByIdAsync(booking.BookingId);

            // Assert
            result.Should().NotBeNull();
            result.BookingId.Should().Be(booking.BookingId);
            result.CarModelBrand.Should().Be(carModel.Brand);
            result.CarModelName.Should().Be(carModel.Model);
        }

        [Fact]
        public async Task CreateBookingAsync_WithValidRequest_ShouldCreateBooking()
        {
            // Arrange
            var carModel = _carModelFaker.Generate();
            var request = new CreateBookingRequest
            {
                CarId = carModel.CarId,
                BookingDateTime = DateTime.UtcNow.AddDays(1),
                CustomerName = "Test Customer",
                CustomerEmail = "test@example.com",
                CustomerPhone = "1234567890"
            };
            DateTime currentDate = DateTime.UtcNow;

            var bookings = _bookingFaker.Generate(2);

            A.CallTo(() => _fakeCarModelRepository.GetByIdAsync(request.CarId.Value))
                .Returns(carModel);
            A.CallTo(() => _fakeBookingRepository.GetListAsync(currentDate,
                                                                currentDate,
                                                                carModel.CarId,
                                                                A<string>._,
                                                                A<string>._,
                                                                A<string>._,
                                                                A<string>._,
                                                                A<string>._,
                                                                A<int>._)).Returns(bookings);

            // Act
            var result = await _bookingService.CreateBookingAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.CarModelBrand.Should().Be(carModel.Brand);
            result.CarModelName.Should().Be(carModel.Model);

            A.CallTo(() => _fakeBookingRepository.AddAsync(A<Booking>._))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task UpdateBookingAsync_WithValidRequestAndDifferentSlot_ShouldUpdateBooking()
        {
            // Arrange
            var existingBooking = _bookingFaker.Generate();
            var carModel = _carModelFaker.Generate();
            var updateRequest = _updateRequestFaker.Generate();
            updateRequest.CarId = carModel.CarId;
            updateRequest.BookingId = existingBooking.BookingId;

            A.CallTo(() => _fakeBookingRepository.GetByIdAsync(updateRequest.BookingId))
                .Returns(existingBooking);
            A.CallTo(() => _fakeCarModelRepository.GetByIdAsync(updateRequest.CarId))
                .Returns(carModel);
            A.CallTo(() => _fakeBookingRepository.GetListAsync(
                A<DateTime>._,
                A<DateTime>._,
                updateRequest.CarId,
                A<string>._,
                A<string>._,
                A<string>._,
                A<string>._,
                A<string>._,
                A<int>._))
                .Returns(new List<Booking>());

            // Act
            var result = await _bookingService.UpdateBookingAsync(updateRequest);

            // Assert
            result.Should().NotBeNull();
            result.BookingId.Should().Be(updateRequest.BookingId);
            result.CarModelBrand.Should().Be(carModel.Brand);
            result.CarModelName.Should().Be(carModel.Model);

            A.CallTo(() => _fakeBookingRepository.UpdateAsync(A<Booking>._))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task DeleteBookingAsync_WithValidId_ShouldDeleteBooking()
        {
            // Arrange
            var booking = _bookingFaker.Generate();
            var carModel = _carModelFaker.Generate();

            A.CallTo(() => _fakeBookingRepository.GetByIdAsync(booking.BookingId))
                .Returns(booking);
            A.CallTo(() => _fakeCarModelRepository.GetByIdAsync(booking.BookingId))
                .Returns(carModel);

            // Act
            await _bookingService.DeleteBookingAsync(booking.BookingId);

            // Assert
            A.CallTo(() => _fakeBookingRepository.DeleteAsync(A<Booking>._))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task ValidateBookingSlotTime_WithNoConflictingBookings_ShouldNotThrowException()
        {
            // Arrange
            var bookingDateTime = DateTime.UtcNow.AddHours(24);
            var request = new CreateBookingRequest
            {
                CarId = 1,
                BookingDateTime = bookingDateTime
            };

            var startDate = bookingDateTime.AddMinutes(-1 * ValidationConstants.BOOKING_MINUTE_INTERVAL);
            var endDate = bookingDateTime.AddMinutes(ValidationConstants.BOOKING_MINUTE_INTERVAL);

            _ = A.CallTo(() => _fakeBookingRepository.GetListAsync(
                    A<DateTime>.That.Matches(d => d == startDate),
                    A<DateTime>.That.Matches(d => d == endDate),
                    request.CarId,
                    A<string>.That.Matches(s => s == ""),
                    A<string>.That.Matches(s => s == ""),
                    A<string>.That.Matches(s => s == ""),
                    A<string>.That.Matches(s => s == ""),
                    A<string>.That.Matches(s => s == ""),
                    A<int?>.That.Matches(y => y == 0)))
                .Returns(new List<Booking>());

            // Act & Assert
            await _bookingService.Invoking(s => s.ValidateBookingSlotTime(request))
                .Should().NotThrowAsync();
        }

        [Fact]
        public async Task ValidateBookingSlotTime_WithExactTimeConflict_ShouldThrowException()
        {
            // Arrange
            var bookingDateTime = DateTime.UtcNow.AddHours(24);
            var request = new CreateBookingRequest
            {
                CarId = 1,
                BookingDateTime = bookingDateTime
            };

            var existingBooking = _bookingFaker.Generate();
            existingBooking.CarId = request.CarId.Value;
            existingBooking.BookingDateTime = bookingDateTime;

            var startDate = bookingDateTime.AddMinutes(-1 * ValidationConstants.BOOKING_MINUTE_INTERVAL);
            var endDate = bookingDateTime.AddMinutes(ValidationConstants.BOOKING_MINUTE_INTERVAL);

            _ = A.CallTo(() => _fakeBookingRepository.GetListAsync(A<DateTime>.That.Matches(d => d == startDate),
                                                                  A<DateTime>.That.Matches(d => d == endDate),
                                                                  A<int?>.That.Matches(id => id == request.CarId),
                                                                  A<string>.That.Matches(s => s == ""),
                                                                  A<string>.That.Matches(s => s == ""),
                                                                  A<string>.That.Matches(s => s == ""),
                                                                  A<string>.That.Matches(s => s == ""),
                                                                  A<string>.That.Matches(s => s == ""),
                                                                  A<int?>.That.Matches(y => y == 0)
                                                                 )).Returns(new List<Booking> { existingBooking });

            // Act & Assert
            await _bookingService.Invoking(s => s.ValidateBookingSlotTime(request))
                .Should().ThrowAsync<HttpStatusCodeException>()
                .WithMessage("There's Already Existing Booking for Selected Time.");
        }

        [Fact]
        public async Task ValidateBookingSlotTime_WithBookingJustBeforeInterval_ShouldThrowException()
        {
            // Arrange
            var bookingDateTime = DateTime.UtcNow.AddHours(24);
            var request = new CreateBookingRequest
            {
                CarId = 1,
                BookingDateTime = bookingDateTime
            };

            var existingBooking = _bookingFaker.Generate();
            existingBooking.CarId = request.CarId.Value;
            existingBooking.BookingDateTime = bookingDateTime.AddMinutes(-1 * (ValidationConstants.BOOKING_MINUTE_INTERVAL - 1));

            var startDate = bookingDateTime.AddMinutes(-1 * ValidationConstants.BOOKING_MINUTE_INTERVAL);
            var endDate = bookingDateTime.AddMinutes(ValidationConstants.BOOKING_MINUTE_INTERVAL);

            A.CallTo(() => _fakeBookingRepository.GetListAsync(A<DateTime>.That.Matches(d => d == startDate),
                                                               A<DateTime>.That.Matches(d => d == endDate),
                                                               request.CarId,
                                                               A<string>.That.Matches(s => s == ""),
                                                               A<string>.That.Matches(s => s == ""),
                                                               A<string>.That.Matches(s => s == ""),
                                                               A<string>.That.Matches(s => s == ""),
                                                               A<string>.That.Matches(s => s == ""),
                                                               A<int?>.That.Matches(y => y == 0)))
                                                 .Returns(new List<Booking> { existingBooking });

            // Act & Assert
            await _bookingService.Invoking(s => s.ValidateBookingSlotTime(request))
                .Should().ThrowAsync<HttpStatusCodeException>()
                .WithMessage("There's Already Existing Booking for Selected Time.");
        }

        [Fact]
        public async Task ValidateBookingSlotTime_WithBookingJustAfterInterval_ShouldThrowException()
        {
            // Arrange
            var bookingDateTime = DateTime.UtcNow.AddHours(24);
            var request = new CreateBookingRequest
            {
                CarId = 1,
                BookingDateTime = bookingDateTime
            };

            var existingBooking = _bookingFaker.Generate();
            existingBooking.CarId = request.CarId.Value;
            existingBooking.BookingDateTime = bookingDateTime.AddMinutes(ValidationConstants.BOOKING_MINUTE_INTERVAL - 1);

            var startDate = bookingDateTime.AddMinutes(-1 * ValidationConstants.BOOKING_MINUTE_INTERVAL);
            var endDate = bookingDateTime.AddMinutes(ValidationConstants.BOOKING_MINUTE_INTERVAL);

            A.CallTo(() => _fakeBookingRepository.GetListAsync(
                A<DateTime>.That.Matches(d => d == startDate),
                A<DateTime>.That.Matches(d => d == endDate),
                request.CarId,
                A<string>.That.Matches(s => s == ""),
                A<string>.That.Matches(s => s == ""),
                A<string>.That.Matches(s => s == ""),
                A<string>.That.Matches(s => s == ""),
                A<string>.That.Matches(s => s == ""),
                A<int?>.That.Matches(y => y == 0)))
                .Returns(new List<Booking> { existingBooking });

            // Act & Assert
            await _bookingService.Invoking(s => s.ValidateBookingSlotTime(request))
                .Should().ThrowAsync<HttpStatusCodeException>()
                .WithMessage("There's Already Existing Booking for Selected Time.");
        }

        [Fact]
        public async Task ValidateBookingSlotTime_WithDifferentCarId_ShouldNotThrowException()
        {
            // Arrange
            var bookingDateTime = DateTime.UtcNow.AddHours(24);
            var request = new CreateBookingRequest
            {
                CarId = 1,
                BookingDateTime = bookingDateTime
            };

            var existingBooking = _bookingFaker.Generate();
            existingBooking.CarId = request.CarId.Value + 1; // Different car ID
            existingBooking.BookingDateTime = bookingDateTime;

            var startDate = bookingDateTime.AddMinutes(-1 * ValidationConstants.BOOKING_MINUTE_INTERVAL);
            var endDate = bookingDateTime.AddMinutes(ValidationConstants.BOOKING_MINUTE_INTERVAL);

            A.CallTo(() => _fakeBookingRepository.GetListAsync(
                A<DateTime>.That.Matches(d => d == startDate),
                A<DateTime>.That.Matches(d => d == endDate),
                request.CarId,
                A<string>.That.Matches(s => s == ""),
                A<string>.That.Matches(s => s == ""),
                A<string>.That.Matches(s => s == ""),
                A<string>.That.Matches(s => s == ""),
                A<string>.That.Matches(s => s == ""),
                A<int?>.That.Matches(y => y == 0)))
                .Returns(new List<Booking>()); // Repository will return empty list for different car ID

            // Act & Assert
            await _bookingService.Invoking(s => s.ValidateBookingSlotTime(request))
                .Should().NotThrowAsync();
        }

        [Fact]
        public async Task ValidateBookingSlotTime_WithMultipleConflictingBookings_ShouldThrowException()
        {
            // Arrange
            var bookingDateTime = DateTime.UtcNow.AddHours(24);
            var request = new CreateBookingRequest
            {
                CarId = 1,
                BookingDateTime = bookingDateTime
            };

            var existingBookings = _bookingFaker.Generate(3);
            foreach (var booking in existingBookings)
            {
                booking.CarId = request.CarId.Value;
                booking.BookingDateTime = bookingDateTime;
            }

            var startDate = bookingDateTime.AddMinutes(-1 * ValidationConstants.BOOKING_MINUTE_INTERVAL);
            var endDate = bookingDateTime.AddMinutes(ValidationConstants.BOOKING_MINUTE_INTERVAL);

            _ = A.CallTo(() => _fakeBookingRepository.GetListAsync(
                A<DateTime>.That.Matches(d => d == startDate),
                A<DateTime>.That.Matches(d => d == endDate),
                request.CarId,
                A<string>.That.Matches(s => s == ""),
                A<string>.That.Matches(s => s == ""),
                A<string>.That.Matches(s => s == ""),
                A<string>.That.Matches(s => s == ""),
                A<string>.That.Matches(s => s == ""),
                A<int?>.That.Matches(y => y == 0)))
                .Returns(existingBookings);

            // Act & Assert
            await _bookingService.Invoking(s => s.ValidateBookingSlotTime(request))
                .Should().ThrowAsync<HttpStatusCodeException>()
                .WithMessage("There's Already Existing Booking for Selected Time.");
        }

        [Fact]
        public async Task GetListAsync_WithValidRequest_ShouldReturnFilteredBookings()
        {
            // Arrange
            var request = new GetBookingListRequest
            {
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(7),
                CarId = 1,
                CustomerName = "Test",
                CarBrand = "Hyundai"
            };

            var filteredBookings = _bookingFaker.Generate(2);
            A.CallTo(() => _fakeBookingRepository.GetListAsync(
                request.StartDate,
                request.EndDate,
                request.CarId,
                request.CustomerName,
                request.CustomerPhone,
                request.CustomerEmail,
                request.CarBrand,
                request.CarModel,
                request.CarYear))
                .Returns(filteredBookings);

            // Act
            var result = await _bookingService.GetListAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task ValidateRequestUpdateBooking_WithValidRequest_ShouldNotThrowException()
        {
            // Arrange
            var updateRequest = new UpdateBookingRequest
            {
                BookingId = 1,
                CarId = 1,
                BookingDateTime = DateTime.UtcNow.AddDays(1)
            };

            // Mock ValidateBookingSlotTime to return true (no conflicts)
            A.CallTo(() => _fakeBookingRepository.GetListAsync(
                A<DateTime>._,
                A<DateTime>._,
                A<int?>._,
                A<string>._,
                A<string>._,
                A<string>._,
                A<string>._,
                A<string>._,
                A<int>._))
                .Returns(new List<Booking>());

            // Mock ValidateCarModel
            var carModel = _carModelFaker.Generate();
            A.CallTo(() => _fakeCarModelRepository.GetByIdAsync(updateRequest.CarId))
                .Returns(carModel);

            // Act & Assert
            await _bookingService.Invoking(s => s.ValidateRequestUpdateBooking(updateRequest))
                .Should().NotThrowAsync();
        }

        [Fact]
        public async Task ValidateRequestUpdateBooking_WithTimeConflict_ShouldThrowException()
        {
            // Arrange
            var updateRequest = new UpdateBookingRequest
            {
                BookingId = 1,
                CarId = 1,
                BookingDateTime = DateTime.UtcNow.AddDays(1)
            };

            // Mock ValidateBookingSlotTime to return conflicting booking
            var existingBooking = _bookingFaker.Generate();
            A.CallTo(() => _fakeBookingRepository.GetListAsync(
                A<DateTime>._,
                A<DateTime>._,
                A<int?>._,
                A<string>._,
                A<string>._,
                A<string>._,
                A<string>._,
                A<string>._,
                A<int>._))
                .Returns(new List<Booking> { existingBooking });

            // Act & Assert
            await _bookingService.Invoking(s => s.ValidateRequestUpdateBooking(updateRequest))
                .Should().ThrowAsync<HttpStatusCodeException>()
                .WithMessage("There's Already Existing Booking for Selected Time.");
        }

        [Fact]
        public async Task ValidateBooking_WithExistingBooking_ShouldReturnBooking()
        {
            // Arrange
            var booking = _bookingFaker.Generate();
            A.CallTo(() => _fakeBookingRepository.GetByIdAsync(booking.BookingId))
                .Returns(booking);

            // Act
            var result = await _bookingService.ValidateBooking(booking.BookingId);

            // Assert
            result.Should().NotBeNull();
            result.BookingId.Should().Be(booking.BookingId);
        }

        [Fact]
        public async Task ValidateBooking_WithNonExistingBooking_ShouldThrowException()
        {
            // Arrange
            int nonExistingId = 999;
            A.CallTo(() => _fakeBookingRepository.GetByIdAsync(nonExistingId))
                .Returns((Booking)null);

            // Act & Assert
            await _bookingService.Invoking(s => s.ValidateBooking(nonExistingId))
                .Should().ThrowAsync<HttpStatusCodeException>()
                .WithMessage($"Booking not found for booking id {nonExistingId}");
        }

        [Fact]
        public async Task ValidateDeleteBookingRequest_WithValidBooking_ShouldReturnBooking()
        {
            // Arrange
            var booking = _bookingFaker.Generate();
            var carModel = _carModelFaker.Generate();

            A.CallTo(() => _fakeBookingRepository.GetByIdAsync(booking.BookingId))
                .Returns(booking);
            A.CallTo(() => _fakeCarModelRepository.GetByIdAsync(booking.BookingId))
                .Returns(carModel);

            // Act
            var result = await _bookingService.ValidateDeleteBookingRequest(booking.BookingId);

            // Assert
            result.Should().NotBeNull();
            result.BookingId.Should().Be(booking.BookingId);
        }

        [Fact]
        public async Task ValidateDeleteBookingRequest_WithNonExistingBooking_ShouldThrowException()
        {
            // Arrange
            int nonExistingId = 999;
            A.CallTo(() => _fakeBookingRepository.GetByIdAsync(nonExistingId))
                .Returns((Booking)null);

            // Act & Assert
            await _bookingService.Invoking(s => s.ValidateDeleteBookingRequest(nonExistingId))
                .Should().ThrowAsync<HttpStatusCodeException>()
                .WithMessage($"Booking not found for booking id {nonExistingId}");
        }

        [Fact]
        public async Task ValidateDeleteBookingRequest_WithNonExistingCar_ShouldThrowException()
        {
            // Arrange
            var booking = _bookingFaker.Generate();
            A.CallTo(() => _fakeBookingRepository.GetByIdAsync(booking.BookingId))
                .Returns(booking);
            A.CallTo(() => _fakeCarModelRepository.GetByIdAsync(booking.BookingId))
                .Returns((CarModel)null);

            // Act & Assert
            await _bookingService.Invoking(s => s.ValidateDeleteBookingRequest(booking.BookingId))
                .Should().ThrowAsync<HttpStatusCodeException>()
                .WithMessage($"Selected Car Model with ID {booking.BookingId} not found");
        }

        [Fact]
        public async Task GetPaginatedAsync_ShouldReturnPaginatedResults()
        {
            // Arrange
            var request = new GetPaginatedBookingsRequest
            {
                Page = 1,
                PageSize = 10,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(7)
            };

            var bookings = _bookingFaker.Generate(5);
            int totalCount = 15;

            A.CallTo(() => _fakeBookingRepository.GetPaginatedAsync(
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
                request.CarYear))
                .Returns((bookings, totalCount));

            // Act
            var (result, count) = await _bookingService.GetPaginatedAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(5);
            count.Should().Be(15);
        }

        [Fact]
        public async Task ValidateCarModel_WithInvalidCarId_ShouldThrowException()
        {
            // Arrange
            int invalidCarId = 999;
            A.CallTo(() => _fakeCarModelRepository.GetByIdAsync(invalidCarId))
                .Returns((CarModel)null);

            // Act & Assert
            await _bookingService.Invoking(s => s.ValidateCarModel(invalidCarId))
                .Should().ThrowAsync<HttpStatusCodeException>()
                .WithMessage($"Selected Car Model with ID {invalidCarId} not found");
        }

        [Fact]
        public async Task ValidateRequestCreateBookingAsync_WithValidRequest_ShouldReturnCarModel()
        {
            // Arrange
            var request = _createBookingRequestFaker.Generate();
            var carModel = _carModelFaker.Generate();
            carModel.CarId = request.CarId.Value;

            A.CallTo(() => _fakeCarModelRepository.GetByIdAsync(request.CarId.Value))
                .Returns(carModel);
            A.CallTo(() => _fakeBookingRepository.GetListAsync(
                A<DateTime>._,
                A<DateTime>._,
                request.CarId,
                A<string>._,
                A<string>._,
                A<string>._,
                A<string>._,
                A<string>._,
                A<int>._))
                .Returns(new List<Booking>());

            // Act
            var result = await _bookingService.ValidateRequestCreateBookingAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.CarId.Should().Be(request.CarId.Value);
        }
    }
}