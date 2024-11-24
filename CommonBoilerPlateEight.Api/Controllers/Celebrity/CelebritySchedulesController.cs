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
    [Route("api/celebrity-schedules")]
    [ApiController]
    public class CelebritySchedulesController : ControllerBase
    {
        private readonly ICelebrityScheduleService _celebritySchedularService;
        public CelebritySchedulesController(ICelebrityScheduleService celebritySchedularService)
        {
            _celebritySchedularService = celebritySchedularService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody] CelebrityScheduleCreateApiModel model)
        {
            var celebrityId = AppHttpContext.ValidateAndGetCelebrityId();
            await _celebritySchedularService.Create(new CelebrityScheduleCreateViewModel
            {
                CelebrityId = celebrityId,
                Date = model.Date,
                ToTime = model.ToTime,
                FromTime = model.FromTime
            }, false);
            return this.ApiSuccessResponse(HttpStatusCode.OK, "Schedule Added Successfully");

        }

        [HttpGet()]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAllSchedule([FromQuery] CelebrityScheduleFilterApiModel model)
        {
            var celebrityId = AppHttpContext.ValidateAndGetCelebrityId();
            var result = await _celebritySchedularService.GetAllAsPagedList(new CelebrityScheduleFilterViewModel
            {
                CelebrityId = celebrityId,
                From = model.FromDate,
                To = model.ToDate,
                PageNumber = model.PageNumber,
            });
            return this.ApiSuccessResponse(HttpStatusCode.OK, "Schedule Retrived Successfully", result);

        }
    }
}
