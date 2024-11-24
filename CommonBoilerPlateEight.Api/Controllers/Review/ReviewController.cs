using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CommonBoilerPlateEight.Api.ApiModel;
using CommonBoilerPlateEight.Api.Extensions;
using CommonBoilerPlateEight.Api.Filters;
using CommonBoilerPlateEight.Domain.Interfaces;
using CommonBoilerPlateEight.Domain.Models;
using System.Net;


namespace CommonBoilerPlateEight.Api.Controllers
{
    [AuthorizeCustomer]
    [Authorize]
    [Route("api/celebrity-reviews")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost()]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddReview([FromForm] ReviewRequestModel model)
        {
            var reviewResponse = await _reviewService.AddReviewAsync(model);
            return this.ApiSuccessResponse(HttpStatusCode.OK, "Review created successfully.", reviewResponse);
        }

        [HttpGet("{celebrityId}")]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAllReviewsByCelebrityId(int celebrityId)
        {
            var reviews = await _reviewService.GetAllReviewByCelebrityIdAsync(celebrityId);
            return Ok(reviews);
        }



    }
}