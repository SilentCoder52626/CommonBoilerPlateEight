using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CommonBoilerPlateEight.Api.Filters;
using CommonBoilerPlateEight.Api.ApiModel;
using CommonBoilerPlateEight.Api.Extensions;
using CommonBoilerPlateEight.Domain.Extensions;
using CommonBoilerPlateEight.Domain.Interfaces;
using CommonBoilerPlateEight.Domain.Models;
using System.Net;

namespace CommonBoilerPlateEight.Api.Controllers.Celebrity
{
    [AuthorizeCelebrity]
    [Authorize]
    [Route("api/celebrity-locations")]
    [ApiController]
    public class CelebrityLocationsController : ControllerBase
    {
        private readonly ICelebrityLocationService _celebrityLocationService;
        public CelebrityLocationsController(ICelebrityLocationService celebrityLocationService)
        {
            _celebrityLocationService = celebrityLocationService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody] CelebrityLocationCreateApiViewModel model)
        {
            var celebrityId = AppHttpContext.ValidateAndGetCelebrityId();
            await _celebrityLocationService.Create(new CelebrityLocationCreateViewModel
            {
                CelebrityId = celebrityId,
                FullAddress = model.FullAddress,
                Latitude = model.Latitude,
                Longitude = model.Longitude,
                Area = model.Area,
                Governorate = model.Governorate,
                GooglePlusCode = model.GooglePlusCode,
                Block = model.Block,
                Street = model.Street,
                Note = model.Note,
            });
            return this.ApiSuccessResponse(HttpStatusCode.OK, "Location Added Successfully");

        }


        [HttpGet]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAllLocations()
        {
            var celebrityId = AppHttpContext.ValidateAndGetCelebrityId();
            var locations = await _celebrityLocationService.GetAllLocationsOfcelebrity(celebrityId);
            return this.ApiSuccessResponse(HttpStatusCode.OK, "Location Retrived Successfully", locations);

        }


        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Update(int id, [FromBody] CelebrityLocationEditViewModel model)
        {

            var celebrityId = AppHttpContext.ValidateAndGetCelebrityId();

            model.Id = id;
            model.CelebrityId = celebrityId;

            await _celebrityLocationService.Update(model);


            return this.ApiSuccessResponse(HttpStatusCode.OK, "Location Updated Successfully");
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            var celebrityId = AppHttpContext.ValidateAndGetCelebrityId();
            await _celebrityLocationService.Delete(id);
            return this.ApiSuccessResponse(HttpStatusCode.OK, "Location Deleted Successfully");
        }


    }
}
