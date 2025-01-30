using Bogus;
using Castle.Core.Logging;
using FakeItEasy;
using FluentAssertions;
using car_booking_service.Application.Models.Requests.CarModelRequests;
using car_booking_service.Application.Models.Responses.CarModelResponses;
using car_booking_service.Application.Services.Implementations;
using car_booking_service.Application.Services.Interfaces;
using car_booking_service.Domain.Entities;
using car_booking_service.Domain.Exception;
using car_booking_service.Domain.Interfaces;
using Mapster;
using Xunit;

namespace hyundai_testDriveBooking_service.Test.Services
{
    public class CarModelServiceTest
    {
        private readonly ICarModelRepository _carModelRepository;
        private readonly ICarModelService _carModelService;
        private readonly Faker<CarModel> _carModelFaker;

        public CarModelServiceTest()
        {
            _carModelRepository = A.Fake<ICarModelRepository>();
            
            _carModelService = new CarModelService(_carModelRepository);

            _carModelFaker = new Faker<CarModel>()
                .RuleFor(c => c.CarId, f => f.IndexFaker + 1)
                .RuleFor(c => c.Brand, f => f.Vehicle.Manufacturer())
                .RuleFor(c => c.Model, f => f.Vehicle.Model())
                .RuleFor(c => c.Year, f => f.Date.Past(10).Year)
                .RuleFor(c => c.ImageUrl, f => f.Internet.UrlWithPath("https://www.hyundai.com/content/dam/hyundai/id/en/data/vehicle-thumbnail/product/"))
                .RuleFor(c => c.Description, f => f.Lorem.Letter(100))
                .RuleFor(c => c.CreatedAt, f => f.Date.Recent())
                .RuleFor(c => c.CreatedBy, f => f.Person.FirstName)
                .RuleFor(c => c.UpdatedAt, f => f.Date.Recent())
                .RuleFor(c => c.UpdatedBy, f => f.Person.FirstName);
        }

        private static List<CarModel> CreateFakeData(int totalData)  => new Faker<CarModel>()
                                                                                    .RuleFor(c => c.CarId, f => f.IndexFaker + 1)
                                                                                    .RuleFor(c => c.Brand, f => f.Vehicle.Manufacturer())
                                                                                    .RuleFor(c => c.Model, f => f.Vehicle.Model())
                                                                                    .RuleFor(c => c.Year, f => f.Date.Past(10).Year)
                                                                                    .RuleFor(c => c.ImageUrl, f => f.Internet.UrlWithPath("https://www.hyundai.com/content/dam/hyundai/id/en/data/vehicle-thumbnail/product/"))
                                                                                    .RuleFor(c => c.Description, f => f.Lorem.Letter(100))
                                                                                    .RuleFor(c => c.CreatedAt, f => f.Date.Recent())
                                                                                    .RuleFor(c => c.CreatedBy, f => f.Person.FirstName)
                                                                                    .RuleFor(c => c.UpdatedAt, f => f.Date.Recent())
                                                                                    .RuleFor(c => c.UpdatedBy, f => f.Person.FirstName)
                                                                                    .Generate(totalData);

        [Fact]
        public async Task CreateCarModelAsync_ShouldReturnCarModelResponse()
        {
            // Arrange
            var fakeData = CreateFakeData(1).FirstOrDefault();
            var requestService = fakeData.Adapt<CreateCarModelRequest>();

            // Act
            var result = await _carModelService.CreateCarModelAsync(requestService);
            result.CarId = 1;
 
            // Assert
            result.Should().NotBeNull();

            result.CarId.Should().Be(fakeData.CarId);
            result.Brand.Should().Be(fakeData.Brand);
            result.Model.Should().Be(fakeData.Model);
            result.Year.Should().Be(fakeData.Year);
            result.ImageUrl.Should().Be(fakeData.ImageUrl);
            result.Description.Should().Be(fakeData.Description);
        }

        [Fact]
        public async Task UpdateCarModelAsync_WithExistingId_ShouldUpdateAndReturnCarModel()
        {
            // Arrange
            var existingModel = _carModelFaker.Generate();
            UpdateCarModelRequest updateRequest = existingModel.Adapt<UpdateCarModelRequest>();
            updateRequest.Brand = "Updated Brand";
            updateRequest.Model = "Updated Model";
            updateRequest.Year = 2025;
            updateRequest.Description = "Updated Description";

            A.CallTo(() => _carModelRepository.GetByIdAsync(existingModel.CarId))
                .Returns(existingModel);
            A.CallTo(() => _carModelRepository.UpdateAsync(A<CarModel>._))
                .Returns(existingModel);

            // Act
            var result = await _carModelService.UpdateCarModelAsync(updateRequest);

            // Assert
            result.Should().NotBeNull();
            result.Brand.Should().Be(updateRequest.Brand);
            result.Model.Should().Be(updateRequest.Model);
            result.Year.Should().Be(updateRequest.Year);
            result.Description.Should().Be(updateRequest.Description);
        }

        [Fact]
        public async Task UpdateCarModelAsync_WithNonExistingId_ShouldThrowException()
        {
            // Arrange
            var updateRequest = new UpdateCarModelRequest { CarId = 999 };
            A.CallTo(() => _carModelRepository.GetByIdAsync(999))
                .Returns((CarModel)null);

            // Act & Assert
            await _carModelService.Invoking(s => s.UpdateCarModelAsync(updateRequest))
                .Should().ThrowAsync<HttpStatusCodeException>()
                .WithMessage($"Car model with ID {updateRequest.CarId} not found");
        }

        [Fact]
        public async Task GetCarModelByIdAsync_WithExistingId_ShouldReturnCarModel()
        {
            // Arrange
            var existingModel = _carModelFaker.Generate();
            A.CallTo(() => _carModelRepository.GetByIdAsync(existingModel.CarId))
                .Returns(existingModel);

            // Act
            var result = await _carModelService.GetCarModelByIdAsync(existingModel.CarId);

            // Assert
            result.Should().NotBeNull();
            result.CarId.Should().Be(existingModel.CarId);
            result.Brand.Should().Be(existingModel.Brand);
            result.Model.Should().Be(existingModel.Model);

            A.CallTo(() => _carModelRepository.GetByIdAsync(existingModel.CarId))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task GetAllCarModelsAsync_ShouldReturnAllCarModels()
        {
            // Arrange
            var carModels = _carModelFaker.Generate(3);
            A.CallTo(() => _carModelRepository.GetAllAsync())
                .Returns(carModels);

            // Act
            var result = await _carModelService.GetAllCarModelsAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(3);
            result.Select(c => c.CarId).Should().BeEquivalentTo(carModels.Select(c => c.CarId));

            A.CallTo(() => _carModelRepository.GetAllAsync())
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task DeleteCarModelAsync_WithExistingId_ShouldDeleteCarModel()
        {
            // Arrange
            var existingModel = _carModelFaker.Generate();
            A.CallTo(() => _carModelRepository.GetByIdAsync(existingModel.CarId))
                .Returns(existingModel);
            A.CallTo(() => _carModelRepository.DeleteAsync(A<CarModel>._))
                .Returns(Task.CompletedTask);

            // Act
            await _carModelService.DeleteCarModelAsync(existingModel.CarId);

            // Assert
            A.CallTo(() => _carModelRepository.DeleteAsync(A<CarModel>._))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task GetListAsync_ShouldReturnFilteredCarModels()
        {
            // Arrange
            var request = new GetCarModelListRequest
            {
                Brand = "Hyundai",
                Model = "Tucson",
                Year = 2024,
                Description = "Test",
                IsAvailable = true
            };

            var filteredModels = _carModelFaker.Generate(2);
            A.CallTo(() => _carModelRepository.GetListAsync(
                request.Brand,
                request.Model,
                request.Year,
                request.Description,
                request.IsAvailable))
                .Returns(filteredModels);

            // Act
            var result = await _carModelService.GetListAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);

            A.CallTo(() => _carModelRepository.GetListAsync(
                request.Brand,
                request.Model,
                request.Year,
                request.Description,
                request.IsAvailable))
                .MustHaveHappenedOnceExactly();
        }
    }
}