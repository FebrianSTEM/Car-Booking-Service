using car_booking_service.Domain.Entities;

namespace car_booking_service.Domain.Interfaces
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        Task<List<Booking>> GetListAsync(DateTime startDate,
                                         DateTime endDate,
                                         int? carId,
                                         string customerName = "",
                                         string customerPhone = "",
                                         string customerEmail = "",
                                         string carBrand = "",
                                         string carModel = "",
                                         int? carYear = 0);

        Task<(List<Booking>, int)> GetPaginatedAsync(int pageSize,
                                                     int pageIndex,
                                                     DateTime startDate,
                                                     DateTime endDate,
                                                     int? carId,
                                                     string customerName = "",
                                                     string customerPhone = "",
                                                     string customerEmail = "",
                                                     string carBrand = "",
                                                     string carModel = "",
                                                     int? carYear = 0);
}
}