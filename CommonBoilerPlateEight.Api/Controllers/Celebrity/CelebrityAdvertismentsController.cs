using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CommonBoilerPlateEight.Api.Filters;
using CommonBoilerPlateEight.Api.ApiModel;
using CommonBoilerPlateEight.Api.Extensions;
using CommonBoilerPlateEight.Domain.Extensions;
using CommonBoilerPlateEight.Domain.Interfaces;
using CommonBoilerPlateEight.Domain.Models.CelebrityAdvertisment;
using System.Net;

namespace CommonBoilerPlateEight.Api.Controllers
{
    [AuthorizeCelebrity]
    [Authorize]
    [Route("api/celebrity-advertisments")]
    [ApiController]
    public class CelebrityAdvertismentsController : ControllerBase
    {
        private readonly ICelebrityAdvertismentService _celebrityAdvertismentService;

        public CelebrityAdvertismentsController(ICelebrityAdvertismentService celebrityAdvertismentService)
        {
            _celebrityAdvertismentService = celebrityAdvertismentService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetOrderById(string id)
        {
            var order = await _celebrityAdvertismentService.GetCelebrityAdvertismentAsync(id);
            return this.ApiSuccessResponse(HttpStatusCode.OK, "Order Retrived Successfully", order);

        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetOrderForCelebrities([FromQuery] CelebrityAdvertismentFilterViewModel model)
        {
            var celebrityId = AppHttpContext.ValidateAndGetCelebrityId();
            var orders = await _celebrityAdvertismentService.GetAdvertismentsOfACelebrityAsync(celebrityId, model);
            return this.ApiSuccessResponse(HttpStatusCode.OK, "Orders Retrived Successfully", orders);

        }

        [HttpPost("{id}/accept")]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AcceptOrder(string id)
        {
            var accepted = await _celebrityAdvertismentService.AcceptAdvertisment(id);
            return this.ApiSuccessResponse(HttpStatusCode.OK, "Order Accepted", accepted);
        }

        [HttpPost("{id}/reject")]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RejectOrder(string id, [FromBody] string reason)
        {
            var rejected = await _celebrityAdvertismentService.CancelAdvertisment(id, reason);
            return this.ApiSuccessResponse(HttpStatusCode.OK, "Order Rejected", rejected);
        }

        [HttpPost("{id}/complete")]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CompleteOrder(string id)
        {
            var completed = await _celebrityAdvertismentService.CompleteAdvertisment(id);
            return this.ApiSuccessResponse(HttpStatusCode.OK, "Order Completed", completed);
        }
    }
}
