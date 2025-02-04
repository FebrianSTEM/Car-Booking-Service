using car_booking_service.Application.Models.Requests.CarModelRequests;
using car_booking_service.Application.Models.Responses.CarModelResponses;
using car_booking_service.Application.Services.Interfaces;
using car_booking_service.Domain.Common;
using car_booking_service.Domain.Constants;
using car_booking_service.Domain.Enums;
using car_booking_service.Domain.Exception;
using Microsoft.AspNetCore.Mvc;

namespace car_booking_service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarModelsController : ControllerBase
    {
        protected HttpResult HttpResults;
        private readonly ICarModelService _carModelService;
        private readonly ILogger<CarModelsController> _logger;

        public CarModelsController(ICarModelService carModelService, ILogger<CarModelsController> logger)
        {
            _carModelService = carModelService;
            _logger = logger;
        }

        /// <summary>
        /// Get All Car Models
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(StandardResponse<IEnumerable<CarModelResponse>>))]
        public async Task<ActionResult<IEnumerable<CarModelResponse>>> GetAllCarModels()
        {
            string message = string.Empty;
            try
            {
                var carModels = await _carModelService.GetAllCarModelsAsync();

                message = $"{ResponseMessagesConstants.SUCCESS_RETRIEVE_MESSAGE_TEMPLATE} Car Models with total {carModels.Count()}";
                HttpResults = new StandardResponse<IEnumerable<CarModelResponse>>(Enums.StatusCode.OK, Enums.StatusMessage.Success, carModels, message);

                return StatusCode((int)Enums.StatusCode.OK, HttpResults);
            }
            catch (HttpStatusCodeException ex)
            {
                message = $"{ResponseMessagesConstants.FAILED_RETRIEVE_MESSAGE_TEMPLATE} Car Models: {ex.Message}";
                HttpResults = new StandardResponse<IEnumerable<CarModelResponse>>((Enums.StatusCode)ex.StatusCode, Enums.StatusMessage.Error, null, message);
                _logger.LogError(ex, message);

                return StatusCode(ex.StatusCode, HttpResults);
            }
            catch (Exception ex)
            {
                message = $"{ResponseMessagesConstants.FAILED_RETRIEVE_MESSAGE_TEMPLATE} Car Models: {ex.Message}";
                HttpResults = new StandardResponse<IEnumerable<CarModelResponse>>(Enums.StatusCode.InternalServerErrorException, Enums.StatusMessage.Error, null, message);
                _logger.LogError(ex, message);

                return StatusCode(StatusCodes.Status500InternalServerError, HttpResults);
            }
        }

        /// <summary>
        /// Get List of Car Models
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("list")]
        [ProducesResponseType(200, Type = typeof(StandardResponse<IEnumerable<CarModelResponse>>))]
        public async Task<ActionResult<IEnumerable<CarModelResponse>>> GetCarModelList([FromQuery] GetCarModelListRequest request)
        {
            string message = string.Empty;
            try
            {
                var carModels = await _carModelService.GetListAsync(request);

                message = $"{ResponseMessagesConstants.SUCCESS_RETRIEVE_MESSAGE_TEMPLATE} Car Models with total {carModels.Count()}";
                HttpResults = new StandardResponse<IEnumerable<CarModelResponse>>(Enums.StatusCode.OK, Enums.StatusMessage.Success, carModels, message);

                return StatusCode((int)Enums.StatusCode.OK, HttpResults);
            }
            catch (HttpStatusCodeException ex)
            {
                message = $"{ResponseMessagesConstants.FAILED_RETRIEVE_MESSAGE_TEMPLATE} Car Models: {ex.Message}";
                HttpResults = new StandardResponse<IEnumerable<CarModelResponse>>((Enums.StatusCode)ex.StatusCode, Enums.StatusMessage.Error, null, message);
                _logger.LogError(ex, message);

                return StatusCode(ex.StatusCode, HttpResults);
            }
            catch (Exception ex)
            {
                message = $"{ResponseMessagesConstants.FAILED_RETRIEVE_MESSAGE_TEMPLATE} Car Models: {ex.Message}";
                HttpResults = new StandardResponse<IEnumerable<CarModelResponse>>(Enums.StatusCode.InternalServerErrorException, Enums.StatusMessage.Error, null, message);
                _logger.LogError(ex, message);

                return StatusCode(StatusCodes.Status500InternalServerError, HttpResults);
            }
        }

        /// <summary>
        /// Get Paginated List of Car Models
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("paging")]
        [ProducesResponseType(200, Type = typeof(PaginatedResponse<List<CarModelResponse>>))]
        public async Task<ActionResult<IEnumerable<CarModelResponse>>> GetPaginatedCarModelList([FromQuery] GetPaginatedCarModelRequest request)
        {
            string message = string.Empty;
            try
            {
                (var carModels, int totalData) = await _carModelService.GetPaginatedAsync(request);

                message = $"{ResponseMessagesConstants.SUCCESS_RETRIEVE_MESSAGE_TEMPLATE} Car Models with total {carModels.Count}";
                HttpResults = new PaginatedResponse<List<CarModelResponse>>(Enums.StatusCode.OK, Enums.StatusMessage.Success, carModels, message, request.Page, request.PageSize, totalData);

                return StatusCode((int)Enums.StatusCode.OK, HttpResults);
            }
            catch (HttpStatusCodeException ex)
            {
                message = $"{ResponseMessagesConstants.FAILED_RETRIEVE_MESSAGE_TEMPLATE} Car Models: {ex.Message}";
                HttpResults = new StandardResponse<IEnumerable<CarModelResponse>>((Enums.StatusCode)ex.StatusCode, Enums.StatusMessage.Error, null, message);
                _logger.LogError(ex, message);

                return StatusCode(ex.StatusCode, HttpResults);
            }
            catch (Exception ex)
            {
                message = $"{ResponseMessagesConstants.FAILED_RETRIEVE_MESSAGE_TEMPLATE} Car Models: {ex.Message}";
                HttpResults = new StandardResponse<IEnumerable<CarModelResponse>>(Enums.StatusCode.InternalServerErrorException, Enums.StatusMessage.Error, null, message);
                _logger.LogError(ex, message);

                return StatusCode(StatusCodes.Status500InternalServerError, HttpResults);
            }
        }

        /// <summary>
        /// Get Car Model By id
        /// </summary>
        /// <param name="carId"></param>
        /// <returns></returns>
        [HttpGet("{carId}")]
        [ProducesResponseType(200, Type = typeof(StandardResponse<CarModelResponse>))]
        public async Task<ActionResult<CarModelResponse>> GetCarModelById(int carId)
        {
            string message = string.Empty;
            try
            {
                var carModel = await _carModelService.GetCarModelByIdAsync(carId);

                message = $"{ResponseMessagesConstants.SUCCESS_RETRIEVE_MESSAGE_TEMPLATE} Car Model with id {carId}";
                HttpResults = new StandardResponse<CarModelResponse>(Enums.StatusCode.OK, Enums.StatusMessage.Success, carModel, message);

                return StatusCode(StatusCodes.Status200OK, HttpResults);
            }
            catch (HttpStatusCodeException ex)
            {
                message = $"{ResponseMessagesConstants.FAILED_RETRIEVE_MESSAGE_TEMPLATE} Car Model with id {carId} : {ex.Message}";
                HttpResults = new StandardResponse<CarModelResponse>((Enums.StatusCode)ex.StatusCode, Enums.StatusMessage.Error, null, message);
                _logger.LogError(ex, message);

                return StatusCode(ex.StatusCode, HttpResults);
            }
            catch (Exception ex)
            {
                message = $"{ResponseMessagesConstants.FAILED_RETRIEVE_MESSAGE_TEMPLATE} Car Model with id {carId} : {ex.Message}";
                HttpResults = new StandardResponse<CarModelResponse>(Enums.StatusCode.InternalServerErrorException, Enums.StatusMessage.Error, null, message);
                _logger.LogError(ex, message);

                return StatusCode(StatusCodes.Status500InternalServerError, HttpResults);
            }
        }

        /// <summary>
        /// Create Car Model
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(StandardResponse<CarModelResponse>))]
        public async Task<ActionResult<CarModelResponse>> CreateCarModel(CreateCarModelRequest request)
        {
            string message = string.Empty;
            try
            {
                var carModel = await _carModelService.CreateCarModelAsync(request);

                message = $"{ResponseMessagesConstants.SUCCESS_CREATE_MESSAGE_TEMPLATE} Car Model {request.Model}";
                HttpResults = new StandardResponse<CarModelResponse>(Enums.StatusCode.Created, Enums.StatusMessage.Success, carModel, message);

                return StatusCode(StatusCodes.Status201Created, HttpResults);
            }
            catch (HttpStatusCodeException ex)
            {
                message = $"{ResponseMessagesConstants.FAILED_CREATE_MESSAGE_TEMPLATE} Car Model {request.Model}, {ex.Message}";
                HttpResults = new StandardResponse<CarModelResponse>((Enums.StatusCode)ex.StatusCode, Enums.StatusMessage.Error, null, message);
                _logger.LogError(ex, message);

                return StatusCode(ex.StatusCode, HttpResults);
            }
            catch (Exception ex)
            {
                message = $"{ResponseMessagesConstants.FAILED_CREATE_MESSAGE_TEMPLATE} Car Model {request.Model}, {ex.Message}";
                HttpResults = new StandardResponse<CarModelResponse>(Enums.StatusCode.InternalServerErrorException, Enums.StatusMessage.Error, null, message);
                _logger.LogError(ex, message);
                
                return StatusCode(StatusCodes.Status500InternalServerError, HttpResults);
            }
        }

        /// <summary>
        /// Update Car Model
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(200, Type = typeof(StandardResponse<CarModelResponse>))]
        public async Task<ActionResult<CarModelResponse>> UpdateCarModel([FromBody]UpdateCarModelRequest request)
        {
            string message = string.Empty;
            try
            {
                var carModel = await _carModelService.UpdateCarModelAsync(request);
             
                message = $"{ResponseMessagesConstants.SUCCESS_UPDATE_MESSAGE_TEMPLATE} Car Model Data with id {request.CarId}";
                HttpResults = new StandardResponse<CarModelResponse>(Enums.StatusCode.OK, Enums.StatusMessage.Success, carModel, message);

                return StatusCode((int)Enums.StatusCode.OK, HttpResults);              
            }
            catch (HttpStatusCodeException ex)
            {
                message = $"{ResponseMessagesConstants.FAILED_UPDATE_MESSAGE_TEMPLATE} Car Model Data: {ex.Message}";
                HttpResults = new StandardResponse<CarModelResponse>((Enums.StatusCode)ex.StatusCode, Enums.StatusMessage.Error, null, message);
                _logger.LogError(ex, message);

                return StatusCode(ex.StatusCode, HttpResults);
            }
            catch (Exception ex)
            {
                message = $"{ResponseMessagesConstants.FAILED_UPDATE_MESSAGE_TEMPLATE} Car Model Data: {ex.Message}";
                HttpResults = new StandardResponse<CarModelResponse>(Enums.StatusCode.InternalServerErrorException, Enums.StatusMessage.Error, null, message);
                _logger.LogError(ex, message);

                return StatusCode(StatusCodes.Status500InternalServerError, HttpResults);
            }
        }

        /// <summary>
        /// Delete car model by car id.
        /// </summary>
        /// <param name="carId"></param>
        /// <returns>returns no content</returns>
        [HttpDelete("{carId}")]
        [ProducesResponseType(204, Type = typeof(StandardResponse<CarModelResponse>))]
        public async Task<ActionResult> DeleteCarModel(int carId)
        {
            string message = string.Empty;
            try
            {
                await _carModelService.DeleteCarModelAsync(carId);

                message = $"{ResponseMessagesConstants.FAILED_DELETE_MESSAGE_TEMPLATE} Car Model Data with id : {carId}";
                HttpResults = new StandardResponse<CarModelResponse>(Enums.StatusCode.NoContent, Enums.StatusMessage.Error, null, message);

                return StatusCode((int)Enums.StatusCode.NoContent, HttpResults);
            }
            catch (HttpStatusCodeException ex)
            {
                message = $"{ResponseMessagesConstants.FAILED_DELETE_MESSAGE_TEMPLATE} Car Model Data with id : {carId}, {ex.Message}";
                HttpResults = new StandardResponse<CarModelResponse>((Enums.StatusCode)ex.StatusCode, Enums.StatusMessage.Error, null, message);
                _logger.LogError(ex, message);

                return StatusCode(ex.StatusCode, HttpResults);
            }
            catch (Exception ex)
            {
                message = $"{ResponseMessagesConstants.FAILED_DELETE_MESSAGE_TEMPLATE} Car Model Data with id : {carId}, {ex.Message}";
                HttpResults = new StandardResponse<CarModelResponse>(Enums.StatusCode.InternalServerErrorException, Enums.StatusMessage.Error, null, message);
                _logger.LogError(ex, message);

                return StatusCode((int)Enums.StatusCode.InternalServerErrorException, HttpResults);
            }
        }
    }
}