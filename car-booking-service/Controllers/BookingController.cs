using car_booking_service.Application.Models.Requests.BookingRequests;
using car_booking_service.Application.Models.Responses.BookingResponses;
using car_booking_service.Application.Services.Interfaces;
using car_booking_service.Domain.Common;
using car_booking_service.Domain.Constants;
using car_booking_service.Domain.Enums;
using car_booking_service.Domain.Exception;
using car_booking_service;
using Microsoft.AspNetCore.Mvc;

namespace car_booking_service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        protected HttpResult HttpResults;
        private readonly IBookingService _bookingService;
        private readonly ILogger<BookingController> _logger;

        public BookingController(IBookingService bookingService, ILogger<BookingController> logger)
        {
            _bookingService = bookingService;
            _logger = logger;
        }

        /// <summary>
        /// Get Booking By Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{bookingId}")]
        [ProducesResponseType(200, Type = typeof(StandardResponse<BookingResponse>))]
        public async Task<ActionResult<BookingResponse>> GetBookingById([FromRoute] int bookingId)
        {
            string message = string.Empty;
            try
            {
                var booking = await _bookingService.GetBookingByIdAsync(bookingId);

                message = $"{ResponseMessagesConstants.SUCCESS_RETRIEVE_MESSAGE_TEMPLATE} booking with booking id {bookingId}";
                HttpResults = new StandardResponse<BookingResponse>(Enums.StatusCode.OK, Enums.StatusMessage.Success, booking, message);

                return StatusCode((int)Enums.StatusCode.OK, HttpResults);
            }
            catch (HttpStatusCodeException ex)
            {
                message = $"{ResponseMessagesConstants.FAILED_RETRIEVE_MESSAGE_TEMPLATE} booking with booking id {bookingId}: {ex.Message}";
                HttpResults = new StandardResponse<BookingResponse>((Enums.StatusCode)ex.StatusCode, Enums.StatusMessage.Error, null, message);
                _logger.LogError(ex, message);

                return StatusCode(ex.StatusCode, HttpResults);
            }
            catch (Exception ex)
            {
                message = $"{ResponseMessagesConstants.FAILED_RETRIEVE_MESSAGE_TEMPLATE} booking with booking id {bookingId}: {ex.Message}";
                HttpResults = new StandardResponse<BookingResponse>(Enums.StatusCode.InternalServerErrorException, Enums.StatusMessage.Error, null, message);
                _logger.LogError(ex, message);

                return StatusCode(StatusCodes.Status500InternalServerError, HttpResults);
            }
        }

        /// <summary>
        /// Get All Bookings
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(StandardResponse<IEnumerable<BookingResponse>>))]
        public async Task<ActionResult<IEnumerable<BookingResponse>>> GetAllBooking()
        {
            string message = string.Empty;
            try
            {
                var bookings = await _bookingService.GetAllBookingAsync();

                message = $"{ResponseMessagesConstants.SUCCESS_RETRIEVE_MESSAGE_TEMPLATE} Booking with total {bookings.Count()}";
                HttpResults = new StandardResponse<IEnumerable<BookingResponse>>(Enums.StatusCode.OK, Enums.StatusMessage.Success, bookings, message);

                return StatusCode((int)Enums.StatusCode.OK, HttpResults);
            }
            catch (HttpStatusCodeException ex)
            {
                message = $"{ResponseMessagesConstants.FAILED_RETRIEVE_MESSAGE_TEMPLATE} Booking: {ex.Message}";
                HttpResults = new StandardResponse<IEnumerable<BookingResponse>>((Enums.StatusCode)ex.StatusCode, Enums.StatusMessage.Error, null, message);
                _logger.LogError(ex, message);

                return StatusCode(ex.StatusCode, HttpResults);
            }
            catch (Exception ex)
            {
                message = $"{ResponseMessagesConstants.FAILED_RETRIEVE_MESSAGE_TEMPLATE} Booking: {ex.Message}";
                HttpResults = new StandardResponse<IEnumerable<BookingResponse>>(Enums.StatusCode.InternalServerErrorException, Enums.StatusMessage.Error, null, message);
                _logger.LogError(ex, message);

                return StatusCode(StatusCodes.Status500InternalServerError, HttpResults);
            }
        }

        /// <summary>
        /// Get Booking List
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("list")]
        [ProducesResponseType(200, Type = typeof(StandardResponse<List<BookingResponse>>))]
        public async Task<ActionResult<List<BookingResponse>>> GetBookingList([FromQuery] GetBookingListRequest request)
        {
            string message = string.Empty;
            try
            {
                var bookings = await _bookingService.GetListAsync(request);

                message = $"{ResponseMessagesConstants.SUCCESS_RETRIEVE_MESSAGE_TEMPLATE} booking list with total {bookings.Count}";
                HttpResults = new StandardResponse<List<BookingResponse>>(Enums.StatusCode.OK, Enums.StatusMessage.Success, bookings, message);

                return StatusCode((int)Enums.StatusCode.OK, HttpResults);
            }
            catch (HttpStatusCodeException ex)
            {
                message = $"{ResponseMessagesConstants.FAILED_RETRIEVE_MESSAGE_TEMPLATE} booking List: {ex.Message}";
                HttpResults = new StandardResponse<List<BookingResponse>>((Enums.StatusCode)ex.StatusCode, Enums.StatusMessage.Error, null, message);
                _logger.LogError(ex, message);

                return StatusCode(ex.StatusCode, HttpResults);
            }
            catch (Exception ex)
            {
                message = $"{ResponseMessagesConstants.FAILED_RETRIEVE_MESSAGE_TEMPLATE} booking List: {ex.Message}";
                HttpResults = new StandardResponse<List<BookingResponse>>(Enums.StatusCode.InternalServerErrorException, Enums.StatusMessage.Error, null, message);
                _logger.LogError(ex, message);

                return StatusCode(StatusCodes.Status500InternalServerError, HttpResults);
            }
        }

        /// <summary>
        /// Get Booking List Paginated
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("paging")]
        [ProducesResponseType(200, Type = typeof(PaginatedResponse<List<BookingResponse>>))]
        public async Task<ActionResult<List<BookingResponse>>> GetPaginatedBookings([FromQuery] GetPaginatedBookingsRequest request)
        {
            string message = string.Empty;
            try
            {
                (var bookings, int totalCount) = await _bookingService.GetPaginatedAsync(request);

                message = $"{ResponseMessagesConstants.SUCCESS_RETRIEVE_MESSAGE_TEMPLATE} booking list with total {bookings.Count}";
                HttpResults = new PaginatedResponse<List<BookingResponse>>(Enums.StatusCode.OK, Enums.StatusMessage.Success, bookings, message, request.Page, request.PageSize, totalCount);

                return StatusCode((int)Enums.StatusCode.OK, HttpResults);
            }
            catch (HttpStatusCodeException ex)
            {
                message = $"{ResponseMessagesConstants.FAILED_RETRIEVE_MESSAGE_TEMPLATE} booking List: {ex.Message}";
                HttpResults = new StandardResponse<List<BookingResponse>>((Enums.StatusCode)ex.StatusCode, Enums.StatusMessage.Error, null, message);
                _logger.LogError(ex, message);

                return StatusCode(ex.StatusCode, HttpResults);
            }
            catch (Exception ex)
            {
                message = $"{ResponseMessagesConstants.FAILED_RETRIEVE_MESSAGE_TEMPLATE} booking List: {ex.Message}";
                HttpResults = new StandardResponse<List<BookingResponse>>(Enums.StatusCode.InternalServerErrorException, Enums.StatusMessage.Error, null, message);
                _logger.LogError(ex, message);

                return StatusCode(StatusCodes.Status500InternalServerError, HttpResults);
            }
        }

        /// <summary>
        /// Booking test drive.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(StandardResponse<BookingResponse>))]
        public async Task<ActionResult<BookingResponse>> CreateBooking([FromBody] CreateBookingRequest request)
        {
            string message = string.Empty;
            try
            {
                BookingResponse response = await _bookingService.CreateBookingAsync(request);
                message = $"{ResponseMessagesConstants.SUCCESS_CREATE_MESSAGE_TEMPLATE} booking for {request.CustomerName} with booking id {response.BookingId}";
                HttpResults = new StandardResponse<BookingResponse>(Enums.StatusCode.Created, Enums.StatusMessage.Success, response, message);

                return StatusCode((int)Enums.StatusCode.Created, HttpResults);
            }
            catch (HttpStatusCodeException ex)
            {
                message = $"{ResponseMessagesConstants.FAILED_CREATE_MESSAGE_TEMPLATE} booking for {request.CustomerName} : {ex.Message}";
                HttpResults = new StandardResponse<BookingResponse>((Enums.StatusCode)ex.StatusCode, Enums.StatusMessage.Error, null, message);
                _logger.LogError(ex, message);

                return StatusCode(ex.StatusCode, HttpResults);
            }
            catch (Exception ex)
            {
                message = $"{ResponseMessagesConstants.FAILED_CREATE_MESSAGE_TEMPLATE} booking for {request.CustomerName}: {ex.Message}";
                HttpResults = new StandardResponse<BookingResponse>(Enums.StatusCode.InternalServerErrorException, Enums.StatusMessage.Error, null, message);
                _logger.LogError(ex, message);

                return StatusCode(StatusCodes.Status500InternalServerError, HttpResults);
            }
        }

        /// <summary>
        /// Update booking test drive.
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(200, Type = typeof(StandardResponse<BookingResponse>))]
        public async Task<ActionResult<BookingResponse>> UpdateBooking([FromBody] UpdateBookingRequest request)
        {
            string message = string.Empty;
            try
            {
                BookingResponse response = await _bookingService.UpdateBookingAsync(request);
                message = $"{ResponseMessagesConstants.SUCCESS_UPDATE_MESSAGE_TEMPLATE} booking with booking id {request.BookingId}";
                HttpResults = new StandardResponse<BookingResponse>(Enums.StatusCode.OK, Enums.StatusMessage.Success, response, message);

                return StatusCode((int)Enums.StatusCode.OK, HttpResults);
            }
            catch (HttpStatusCodeException ex)
            {
                message = $"{ResponseMessagesConstants.FAILED_UPDATE_MESSAGE_TEMPLATE} booking with booking id {request.BookingId} : {ex.Message}";
                HttpResults = new StandardResponse<BookingResponse>((Enums.StatusCode)ex.StatusCode, Enums.StatusMessage.Error, null, message);
                _logger.LogError(ex, message);

                return StatusCode(ex.StatusCode, HttpResults);
            }
            catch (Exception ex)
            {
                message = $"{ResponseMessagesConstants.FAILED_UPDATE_MESSAGE_TEMPLATE} booking with booking id {request.BookingId}: {ex.Message}";
                HttpResults = new StandardResponse<BookingResponse>(Enums.StatusCode.InternalServerErrorException, Enums.StatusMessage.Error, null, message);
                _logger.LogError(ex, message);

                return StatusCode(StatusCodes.Status500InternalServerError, HttpResults);
            }

        }

        /// <summary>
        /// Delete booking test drive.
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        [HttpDelete("{bookingId}")]
        [ProducesResponseType(204, Type = typeof(StandardResponse<BookingResponse>))]
        public async Task<ActionResult<BookingResponse>> DeleteBooking([FromRoute] int bookingId)
        {
            string message = string.Empty;
            try
            {
                await _bookingService.DeleteBookingAsync(bookingId);
                message = $"{ResponseMessagesConstants.SUCCESS_DELETE_MESSAGE_TEMPLATE} booking with booking id {bookingId}";
                HttpResults = new StandardResponse<BookingResponse>(Enums.StatusCode.NoContent, Enums.StatusMessage.Success, null, message);

                return StatusCode((int)Enums.StatusCode.NoContent, HttpResults);
            }
            catch (HttpStatusCodeException ex)
            {
                message = $"{ResponseMessagesConstants.FAILED_DELETE_MESSAGE_TEMPLATE} booking with booking id {bookingId} : {ex.Message}";
                HttpResults = new StandardResponse<BookingResponse>((Enums.StatusCode)ex.StatusCode, Enums.StatusMessage.Error, null, message);
                _logger.LogError(ex, message);

                return StatusCode(ex.StatusCode, HttpResults);
            }
            catch (Exception ex)
            {
                message = $"{ResponseMessagesConstants.FAILED_DELETE_MESSAGE_TEMPLATE} booking with booking id {bookingId}: {ex.Message}";
                HttpResults = new StandardResponse<BookingResponse>(Enums.StatusCode.InternalServerErrorException, Enums.StatusMessage.Error, null, message);
                _logger.LogError(ex, message);

                return StatusCode(StatusCodes.Status500InternalServerError, HttpResults);
            }
        }
    }
}