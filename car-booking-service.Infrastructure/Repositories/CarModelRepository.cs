using car_booking_service.Domain.Entities;
using car_booking_service.Domain.Interfaces;
using car_booking_service.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace car_booking_service.Infrastructure.Repositories
{
    public class CarModelRepository : GenericRepository<CarModel>, ICarModelRepository
    {
        public CarModelRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<CarModel>> GetAvailableForTestDriveAsync()
        {
            return await _context.CarModels
                                 .Where(x => x.IsAvailableForTestDrive)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<CarModel>> GetByYearAsync(int year)
        {
            return await _context.CarModels.AsNoTracking()
                                 .Where(x => x.Year == year)
                                 .ToListAsync();
        }

        public async Task<List<CarModel>> GetListAsync(string brand, string model, int? year, string description, bool isAvailable)
        {
            var query = _context.CarModels.AsNoTracking();

            if (!string.IsNullOrEmpty(brand))
                query = query.Where(x => x.Brand == brand);
            if (!string.IsNullOrEmpty(model))
                query = query.Where(x => x.Model == model);
            if(!string.IsNullOrEmpty(description))
                query = query.Where(x => x.Description == description);
            if (year.GetValueOrDefault(0) != 0)
                query = query.Where(x => x.Year == year);
            if(isAvailable)
                query = query.Where(x => x.IsAvailableForTestDrive == isAvailable);

            var result = await query.ToListAsync();

            return result;
        }

        public Task<List<CarModel>> GetListByIdsAsync(List<int> carIds)
        {
            var query = _context.CarModels.AsNoTracking()
                                .Where(x => carIds.Contains(x.CarId));
            return query.ToListAsync();
        }

        public async Task<(List<CarModel>, int)> GetPaginatedAsync(int page, int pageSize, string brand, string model, int? year, string description, bool isAvailable)
        {
            var query = _context.CarModels.AsNoTracking();

            if (!string.IsNullOrEmpty(brand))
                query = query.Where(x => x.Brand == brand);
            if (!string.IsNullOrEmpty(model))
                query = query.Where(x => x.Model == model);
            if (!string.IsNullOrEmpty(description))
                query = query.Where(x => x.Description == description);
            if (year.GetValueOrDefault(0) != 0)
                query = query.Where(x => x.Year == year);
            if (isAvailable)
                query = query.Where(x => x.IsAvailableForTestDrive == isAvailable);

            int totalData = await query.CountAsync();

            var result = await query.Skip((page - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();

            return (result, totalData);
        }
    }
}