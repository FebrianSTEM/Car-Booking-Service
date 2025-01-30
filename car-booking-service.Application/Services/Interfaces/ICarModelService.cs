using hyundai_testDriveBooking_service.Application.Models.Requests.CarModelRequests;
using hyundai_testDriveBooking_service.Application.Models.Responses.CarModelResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hyundai_testDriveBooking_service.Application.Services.Interfaces
{
    public interface ICarModelService
    {
        Task<CarModelResponse> CreateCarModelAsync(CreateCarModelRequest request);
        Task<CarModelResponse> UpdateCarModelAsync(UpdateCarModelRequest request);
        Task<CarModelResponse> GetCarModelByIdAsync(int id);
        Task<IEnumerable<CarModelResponse>> GetAllCarModelsAsync();
        Task DeleteCarModelAsync(int id);
        Task<List<CarModelResponse>> GetListAsync(GetCarModelListRequest request);
        Task<(List<CarModelResponse>, int)> GetPaginatedAsync(GetPaginatedCarModelRequest request);

    }
}
