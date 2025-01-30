using car_booking_service.Application.Models.Requests.CarModelRequests;
using car_booking_service.Application.Models.Responses.CarModelResponses;
using car_booking_service.Application.Services.Interfaces;
using car_booking_service.Domain.Entities;
using car_booking_service.Domain.Exception;
using car_booking_service.Domain.Interfaces;
using Mapster;
using static car_booking_service.Domain.Enums.Enums;

namespace car_booking_service.Application.Services.Implementations
{
    public class CarModelService : ICarModelService
    {
        private readonly ICarModelRepository _carModelRepository;

        public CarModelService(ICarModelRepository carModelRepository)
        {
            _carModelRepository = carModelRepository;
        }

        public async Task<CarModelResponse> CreateCarModelAsync(CreateCarModelRequest request)
        {
            var carModel = request.Adapt<CarModel>();
            DateTime currentTime = DateTime.UtcNow;
            carModel.CreatedAt = currentTime;
            carModel.UpdatedAt = currentTime;
            carModel.IsAvailableForTestDrive = true;

            await _carModelRepository.AddAsync(carModel);

            return carModel.Adapt<CarModelResponse>();
        }

        public async Task<CarModelResponse> UpdateCarModelAsync(UpdateCarModelRequest request)
        {
            var carModel = await _carModelRepository.GetByIdAsync(request.CarId);
            if (carModel == null)
                throw new HttpStatusCodeException((int)StatusCode.NotFound, $"Car model with ID {request.CarId} not found");
            request.Adapt(carModel);
            carModel.UpdatedAt = DateTime.UtcNow;

            await _carModelRepository.UpdateAsync(carModel);

            return carModel.Adapt<CarModelResponse>();
        }

        public async Task<CarModelResponse> GetCarModelByIdAsync(int id)
        {
            var carModel = await _carModelRepository.GetByIdAsync(id);
            if (carModel == null)
                throw new HttpStatusCodeException((int)StatusCode.NotFound, $"Car model with ID {id} not found");

            return carModel.Adapt<CarModelResponse>();
        }

        public async Task<IEnumerable<CarModelResponse>> GetAllCarModelsAsync()
        {
            var carModels = await _carModelRepository.GetAllAsync();

            return carModels.Adapt<IEnumerable<CarModelResponse>>();
        }

        public async Task DeleteCarModelAsync(int id)
        {
            var carModel = await _carModelRepository.GetByIdAsync(id);
            if (carModel == null)
                throw new HttpStatusCodeException((int)StatusCode.NotFound, $"Car model with ID {id} not found");

            await _carModelRepository.DeleteAsync(carModel);
        }

        public async Task<List<CarModelResponse>> GetListAsync(GetCarModelListRequest request)
        {
            var carModels = await _carModelRepository.GetListAsync(request?.Brand ?? "",
                                                                   request?.Model ?? "",
                                                                   request?.Year ?? 0,
                                                                   request?.Description ?? "",
                                                                   request.IsAvailable);

            return carModels.Adapt<List<CarModelResponse>>();
        }

        public async Task<(List<CarModelResponse>, int)> GetPaginatedAsync(GetPaginatedCarModelRequest request)
        {
            (var carModels, int totalData) = await _carModelRepository.GetPaginatedAsync(request.Page,
                                                                                         request.PageSize,
                                                                                         request?.Brand ?? "",
                                                                                         request?.Model ?? "",
                                                                                         request?.Year ?? 0,
                                                                                         request?.Description ?? "",
                                                                                         request.IsAvailable);

            return (carModels.Adapt<List<CarModelResponse>>(), totalData);
        }
    }
}