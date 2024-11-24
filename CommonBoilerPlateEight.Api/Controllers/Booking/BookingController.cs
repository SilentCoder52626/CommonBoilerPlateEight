using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CommonBoilerPlateEight.Api.ApiModel;
using CommonBoilerPlateEight.Api.Extensions;
using CommonBoilerPlateEight.Api.Filters;
using CommonBoilerPlateEight.Domain.Extensions;
using CommonBoilerPlateEight.Domain.Interfaces;
using CommonBoilerPlateEight.Domain.Models;
using System.Net;

namespace CommonBoilerPlateEight.Api.Controllers
{
    [AuthorizeCustomer]
    [Authorize]
    [Route("api/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }


        [HttpGet("{trackingId}")]
        public async Task<IActionResult> GetAdvertisementByTrackingId(string trackingId)
        {
            var result = await _bookingService.GetAdvertisementByTrackingIdAsync(trackingId);
            return this.ApiSuccessResponse(HttpStatusCode.OK, "Advertisement retrieved successfully.", result);
        }


        // Get current logged in customer all bookings.Ref:my bookings
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetBookingsForCustomer([FromQuery] AdvertisementFilterViewModel page)
        {
            var customerId = AppHttpContext.ValidateAndGetCustomerId();
            var bookings = await _bookingService.GetAdsForCurrentCustomerAsync(customerId, page);
            return this.ApiSuccessResponse(HttpStatusCode.OK, "Bookings retrieved successfully.", bookings);
        }

        [HttpPost("verify-checkout")]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> VerifyCheckout()
        {
            var customerId = AppHttpContext.ValidateAndGetCustomerId();
            await _bookingService.VerifyCheckoutAsync(customerId);
            return this.ApiSuccessResponse(HttpStatusCode.OK, "Checkout verification completed.");
        }

        [HttpPost("checkout")]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Checkout()
        {
            var customerId = AppHttpContext.ValidateAndGetCustomerId();
            await _bookingService.CheckoutAsync(customerId);
            return this.ApiSuccessResponse(HttpStatusCode.OK, "Checkout process completed.");
        }


        [HttpPost("cancel/{id}")]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CancelBooking(int id, string reason)
        {
            await _bookingService.CancelBookingByAdAsync(id, reason);
            return this.ApiSuccessResponse(HttpStatusCode.OK, "Booking cancelled successfully.");
        }

        // Create booking with multi ads. Ref: Bookings Admin panel
        //[HttpPost()]
        //[ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.OK)]
        //[ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.BadRequest)]
        //public async Task<IActionResult> CreateBookings([FromBody] List<CelebrityAdRequestModel> models)
        //{
        //    var customerId = AppHttpContext.ValidateAndGetCustomerId();
        //    var bookingResponse = await _bookingService.CreateBookingsFromAdminAsync(customerId, models);
        //    return this.ApiSuccessResponse(HttpStatusCode.OK, "Booking created successfully.", bookingResponse);
        //}





        //To cancel a booking. Ref:my bookings

    }
}