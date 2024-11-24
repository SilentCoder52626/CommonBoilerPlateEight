using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CommonBoilerPlateEight.Api.ApiModel;
using CommonBoilerPlateEight.Api.Extensions;
using CommonBoilerPlateEight.Domain.Interfaces;
using System.Net;

namespace CommonBoilerPlateEight.Api.Controllers
{
    [AllowAnonymous]
    [Route("api/company-types")]
    [ApiController]
    public class CelebrityTypesController : ControllerBase
    {
        private readonly ICelebrityTypeService _celebrityTypeService;
        public CelebrityTypesController(ICelebrityTypeService celebrityTypeService)
        {
            _celebrityTypeService = celebrityTypeService;
        }
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.BadRequest)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _celebrityTypeService.GetAllAsync();
            return this.ApiSuccessResponse(HttpStatusCode.OK, "Successfully Retrived Celebrity Types", result);
        }
    }
}
