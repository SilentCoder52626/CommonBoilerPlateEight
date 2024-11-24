using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CommonBoilerPlateEight.Api.ApiModel;
using CommonBoilerPlateEight.Api.Extensions;
using CommonBoilerPlateEight.Api.Filters;
using CommonBoilerPlateEight.Domain.Extensions;
using CommonBoilerPlateEight.Domain.Interfaces;
using CommonBoilerPlateEight.Domain.Models;
using System.Net;

namespace CommonBoilerPlateEight.Api.Controllers.Customer
{
    [AuthorizeCustomer]
    [Authorize]
    [Route("api/customers")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }


        [HttpGet("details")]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetDetails()
        {
            var customerId = AppHttpContext.ValidateAndGetCustomerId();
            var details = await _customerService.GetById(customerId);
            return this.ApiSuccessResponse(HttpStatusCode.OK, "Details Retrived Successfully.", details);

        }


        [HttpPost("Update-Basic-Detail")]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateBasicDetail([FromForm] CustomerEditBasicDetailRequestViewModel model)
        {
            var customerId = AppHttpContext.ValidateAndGetCustomerId();
            await _customerService.EditBasicDetails(new CustomerEditBasicDetailViewModel
            {
                Id = customerId,
                ProfileImageFile = model.ProfileImageFile,
                Description = model.Description,
                CountryId = model.CountryId,
                MobileNumber = model.MobileNumber,
                Email = model.Email,
                FullName = model.FullName,
                Gender = model.Gender
            });
            return this.ApiSuccessResponse(HttpStatusCode.OK, "Updated Successfully.");

        }

        [HttpPost("Add-Interest")]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddInterest([FromBody] List<int> interests)
        {
            var customerId = AppHttpContext.ValidateAndGetCustomerId();
            await _customerService.AddEditInterests(new CustomerEditTypesViewModel
            {
                CustomerId = customerId,
                CelebrityTyesIds = interests
            });
            return this.ApiSuccessResponse(HttpStatusCode.OK, "Interest Added Successfully.");

        }

        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.BadRequest)]
        [HttpPatch("toggle-connectivity")]
        public async Task<IActionResult> ToggleConnectivity()
        {
            var customerId = AppHttpContext.ValidateAndGetCustomerId();
            await _customerService.ToogleConnectivity(customerId);
            return this.ApiSuccessResponse(HttpStatusCode.OK, "Successfull.");
        }

        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.BadRequest)]
        [HttpGet("get-connectivity-status")]
        public async Task<IActionResult> GetConnectivityStatus()
        {
            var customerId = AppHttpContext.ValidateAndGetCustomerId();
            var connectivityStatus = await _customerService.GetCustomerConnectivityStatus(customerId);
            return this.ApiSuccessResponse(HttpStatusCode.OK, string.Empty, connectivityStatus);
        }

        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.BadRequest)]
        [HttpPatch("set-device-id")]
        public async Task<IActionResult> SetDeviceId([FromQuery] string deviceId)
        {
            var customerId = AppHttpContext.ValidateAndGetCustomerId();
            await _customerService.SetDeviceId(customerId, deviceId);
            return this.ApiSuccessResponse(HttpStatusCode.OK, "Customer Device Id Set Successfully");
        }
    }
}
