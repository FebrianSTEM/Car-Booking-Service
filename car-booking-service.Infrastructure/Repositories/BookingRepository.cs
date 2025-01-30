using hyundai_testDriveBooking_service.Domain.Entities;
using hyundai_testDriveBooking_service.Domain.Interfaces;
using hyundai_testDriveBooking_service.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
namespace hyundai_testDriveBooking_service.Infrastructure.Repositories
{
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        public BookingRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<Booking>> GetListAsync(DateTime startDate, 
                                                      DateTime endDate,
                                                      int? carId,
                                                      string customerName = "",
                                                      string customerPhone = "", 
                                                      string customerEmail = "", 
                                                      string carBrand = "", 
                                                      string carModel = "", 
                                                      int? carYear = 0)
        {
            var bookingQuery = _context.Bookings.AsNoTracking()
                                            .Where(x => x.BookingDateTime >= startDate &&
                                                        x.BookingDateTime <= endDate)
                                            .Join(
                                                _context.CarModels.AsNoTracking(),
                                                booking => booking.CarId,
                                                carModel => carModel.CarId,
                                                (booking, carModel) => new Booking
                                                {
                                                    BookingId = booking.BookingId,
                                                    CarId = carModel.CarId,
                                                    CarModelBrand = carModel.Brand,
                                                    CarModelName = carModel.Model,
                                                    CarModelYear = carModel.Year,
                                                    BookingDateTime = booking.BookingDateTime,
                                                    CustomerName = booking.CustomerName,
                                                    CustomerPhone = booking.CustomerPhone,
                                                    CustomerEmail = booking.CustomerEmail,
                                                    CreatedAt = booking.CreatedAt,
                                                    CreatedBy = booking.CreatedBy,
                                                    UpdatedBy = booking.UpdatedBy,
                                                    UpdatedAt = booking.UpdatedAt
                                                });

            if(carId.GetValueOrDefault(0) != 0)
                bookingQuery = bookingQuery.Where(x => x.CarId == carId);
            if (carYear.GetValueOrDefault(0) != 0)
                bookingQuery = bookingQuery.Where(x => x.CarModelYear == carYear);

            if (!string.IsNullOrEmpty(carBrand))
                bookingQuery = bookingQuery.Where(x => x.CarModelBrand == carBrand);

            if (!string.IsNullOrEmpty(carModel))
                bookingQuery = bookingQuery.Where(x => x.CarModelName == carModel);

            if (!string.IsNullOrEmpty(customerName))
                bookingQuery = bookingQuery.Where(x => x.CustomerName == customerName);

            if (!string.IsNullOrEmpty(customerEmail))
                bookingQuery = bookingQuery.Where(x => x.CustomerEmail == customerEmail);

            if (!string.IsNullOrEmpty(customerPhone))
                bookingQuery = bookingQuery.Where(x => x.CustomerPhone == customerPhone);

            var bookingResult = await bookingQuery.ToListAsync();

            return bookingResult;
        }

        public async Task<(List<Booking>, int)> GetPaginatedAsync(int pageIndex, 
                                                           int pageSize, 
                                                           DateTime startDate, 
                                                           DateTime endDate, 
                                                           int? carId, 
                                                           string customerName = "", 
                                                           string customerPhone = "", 
                                                           string customerEmail = "", 
                                                           string carBrand = "", 
                                                           string carModel = "", 
                                                           int? carYear = 0)
        {
            var bookingQuery = _context.Bookings.AsNoTracking()
                                            .Where(x => x.BookingDateTime >= startDate &&
                                                        x.BookingDateTime <= endDate)
                                            .Join(
                                                _context.CarModels.AsNoTracking(),
                                                booking => booking.CarId,
                                                carModel => carModel.CarId,
                                                (booking, carModel) => new Booking
                                                {
                                                    BookingId = booking.BookingId,
                                                    CarId = carModel.CarId,
                                                    CarModelBrand = carModel.Brand,
                                                    CarModelName = carModel.Model,
                                                    CarModelYear = carModel.Year,
                                                    BookingDateTime = booking.BookingDateTime,
                                                    CustomerName = booking.CustomerName,
                                                    CustomerPhone = booking.CustomerPhone,
                                                    CustomerEmail = booking.CustomerEmail,
                                                    CreatedAt = booking.CreatedAt,
                                                    CreatedBy = booking.CreatedBy,
                                                    UpdatedBy = booking.UpdatedBy,
                                                    UpdatedAt = booking.UpdatedAt
                                                });

            if (carId.GetValueOrDefault(0) != 0)
                bookingQuery = bookingQuery.Where(x => x.CarId == carId);
            if (carYear.GetValueOrDefault(0) != 0)
                bookingQuery = bookingQuery.Where(x => x.CarModelYear == carYear);

            if (!string.IsNullOrEmpty(carBrand))
                bookingQuery = bookingQuery.Where(x => x.CarModelBrand == carBrand);

            if (!string.IsNullOrEmpty(carModel))
                bookingQuery = bookingQuery.Where(x => x.CarModelName == carModel);

            if (!string.IsNullOrEmpty(customerName))
                bookingQuery = bookingQuery.Where(x => x.CustomerName == customerName);

            if (!string.IsNullOrEmpty(customerEmail))
                bookingQuery = bookingQuery.Where(x => x.CustomerEmail == customerEmail);

            if (!string.IsNullOrEmpty(customerPhone))
                bookingQuery = bookingQuery.Where(x => x.CustomerPhone == customerPhone);

            var totalRecords = await bookingQuery.CountAsync();

            var bookingResult = await bookingQuery.Skip((pageIndex - 1) * pageSize)
                                                  .Take(pageSize)
                                                  .ToListAsync();

            return (bookingResult, totalRecords);
        }

    }
}