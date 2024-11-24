using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CommonBoilerPlateEight.Api.Filters;
using CommonBoilerPlateEight.Api.ApiModel;
using CommonBoilerPlateEight.Api.Extensions;
using CommonBoilerPlateEight.Domain.Extensions;
using CommonBoilerPlateEight.Domain.Interfaces;
using CommonBoilerPlateEight.Domain.Models;
using CommonBoilerPlateEight.Domain.Models.Celebrity;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace CommonBoilerPlateEight.Api.Controllers
{
    [AuthorizeCelebrity]
    [Authorize]

    [Route("api/celebrities")]
    [ApiController]
    public class CelebrityController : ControllerBase
    {
        private readonly ICelebrityService _celebrityService;
        public CelebrityController(ICelebrityService celebrityService)
        {
            _celebrityService = celebrityService;
        }

        //For a filtered celebrities.Ref:search filter
        [HttpGet("filter")]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetFilteredCelebrities([FromQuery] CelebrityFilterPageViewModel filterRequest)
        {
            var customerId = AppHttpContext.ValidateAndGetCustomerId();
            var celebrities = await _celebrityService.GetFilteredPageCelebrities(filterRequest, customerId);
            return Ok(celebrities);
        }


        [HttpGet("recommended")]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetRecommendedCelebrities([FromQuery] int pageSize, int pageNumber)
        {
            var customerId = AppHttpContext.ValidateAndGetCustomerId();
            var celebrities = await _celebrityService.GetRecommendedCelebrities(customerId, pageSize, pageNumber);
            return Ok(celebrities);
        }

        //For a single celebrity. Ref:home screen
        [HttpPost("get-by-id")]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetCelebrityById([FromForm, Required] int celebrityId)
        {

            var celebrity = await _celebrityService.GetById(celebrityId);
            return this.ApiSuccessResponse(HttpStatusCode.OK, "Details Retrived Successfully.", celebrity);

        }

        [HttpGet("details")]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetDetails()
        {
            var ownerId = AppHttpContext.ValidateAndGetCelebrityId();
            var details = await _celebrityService.GetById(ownerId);
            return this.ApiSuccessResponse(HttpStatusCode.OK, "Details Retrived Successfully.", details);

        }


        [HttpPost("Update-Basic-Detail")]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateBasicDetail([FromForm] CelebrityEditBasicDetailRequestViewModel model)
        {
            var ownerId = AppHttpContext.ValidateAndGetCelebrityId();
            await _celebrityService.EditBasicDetail(new CelebrityEditBasicDetailViewModel
            {
                Id = ownerId,
                FullName = model.FullName,
                Email = model.Email,
                CountryId = model.CountryId,
                ProfileImageFile = model.ProfileImageFile,
                TimeToCall = model.TimeToCall,
                PricePerDelivery = model.PricePerDelivery,
                PricePerEvent = model.PricePerEvent,
                PricePerPost = model.PricePerPost,
                CelebrityTypeId = model.CelebrityTypeId,
                Gender = model.Gender,
                Description = model.Description
            });
            return this.ApiSuccessResponse(HttpStatusCode.OK, "Updated Successfully.");

        }

        [HttpPost("Update-Social-Detail")]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateSocialDetail([FromBody] CelebritySocialLinkRequestViewModel model)
        {
            var ownerId = AppHttpContext.ValidateAndGetCelebrityId();
            await _celebrityService.EditSocialLink(new CelebritySocialLinkUpdateViewModel
            {
                FacebookLink = model.FacebookLink,
                InstagramLink = model.InstagramLink,
                YoutubeLink = model.YoutubeLink,
                ThreadsLink = model.ThreadsLink,
                TwitterLink = model.TwitterLink,
                SnapchatLink = model.SnapchatLink,
                Id = ownerId
            });
            return this.ApiSuccessResponse(HttpStatusCode.OK, "Social Link Updated Successfully.");

        }

        [HttpPost("Update-Attachment")]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateAttachment([FromForm] CelebrityAttachmentUploadViewModel model)
        {
            var ownerId = AppHttpContext.ValidateAndGetCelebrityId();
            await _celebrityService.UploadAttachment(model.Attachment, model.Type, ownerId);
            return this.ApiSuccessResponse(HttpStatusCode.OK, $"{model.Type} Updated Successfully.");

        }

        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.BadRequest)]
        [HttpPatch("toggle-connectivity")]
        public async Task<IActionResult> ToggleConnectivity()
        {
            var celebrityId = AppHttpContext.ValidateAndGetCelebrityId();
            await _celebrityService.ToogleConnectivity(celebrityId);
            return this.ApiSuccessResponse(HttpStatusCode.OK, "Successfull.");
        }

        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.BadRequest)]
        [HttpGet("get-connectivity-status")]
        public async Task<IActionResult> GetConnectivityStatus()
        {
            var celebrityId = AppHttpContext.ValidateAndGetCelebrityId();
            var connectivityStatus = await _celebrityService.GetCelebrityConnectivityStatus(celebrityId);
            return this.ApiSuccessResponse(HttpStatusCode.OK, string.Empty, connectivityStatus);
        }

        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.BadRequest)]
        [HttpPatch("set-device-id")]
        public async Task<IActionResult> SetDeviceId([FromQuery] string deviceId)
        {
            var celebrityId = AppHttpContext.ValidateAndGetCelebrityId();
            await _celebrityService.SetDeviceId(celebrityId, deviceId);
            return this.ApiSuccessResponse(HttpStatusCode.OK, "Celebrity Id Set Successfully");
        }
    }
}
