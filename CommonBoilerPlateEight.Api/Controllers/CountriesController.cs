using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CommonBoilerPlateEight.Api.ApiModel;
using CommonBoilerPlateEight.Api.Extensions;
using CommonBoilerPlateEight.Domain.Interfaces;
using System.Net;

namespace CommonBoilerPlateEight.Api.Controllers
{
    [AllowAnonymous]
    [Route("api/countries")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryService _countryService;
        public CountriesController(ICountryService countryService)
        {
            _countryService = countryService;
        }
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.BadRequest)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _countryService.GetAllAsync();
            return this.ApiSuccessResponse(HttpStatusCode.OK, "Successfully Retrived Countries", result);
        }
    }
}
