using car_booking_service.Domain.Entities;

namespace car_booking_service.Domain.Interfaces
{
    public interface ICarModelRepository : IGenericRepository<CarModel>
    {
        Task<IEnumerable<CarModel>> GetAvailableForTestDriveAsync();
        Task<IEnumerable<CarModel>> GetByYearAsync(int year);
        Task<List<CarModel>> GetListByIdsAsync(List<int> carIds);
        Task<List<CarModel>> GetListAsync(string brand, string model, int? year, string description, bool isAvailable);
        Task<(List<CarModel>, int)> GetPaginatedAsync(int page, int pageSize, string brand, string model, int? year, string description, bool isAvailable);
    }
}