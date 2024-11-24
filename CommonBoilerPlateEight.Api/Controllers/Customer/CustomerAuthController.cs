using Microsoft.AspNetCore.Mvc;
using CommonBoilerPlateEight.Api.ApiModel;
using CommonBoilerPlateEight.Api.Extensions;
using CommonBoilerPlateEight.Domain.Interfaces;
using CommonBoilerPlateEight.Domain.Models;
using System.Net;

namespace CommonBoilerPlateEight.Api.Controllers.Customer
{
    [Route("api/customer-auths")]
    [ApiController]
    public class CustomerAuthController : ControllerBase
    {
        private readonly ICustomerAuthService _customerAuthService;
        public CustomerAuthController(ICustomerAuthService customerAuthService)
        {
            _customerAuthService = customerAuthService;
        }
        [HttpPost("Login")]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Login([FromBody] CustomerLoginRequestViewModel model)
        {

            var tokenResponse = await _customerAuthService.Login(model);
            return this.ApiSuccessResponse(HttpStatusCode.OK, "Logged In Successfully.", tokenResponse);

        }

        [HttpPost("Register")]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Register([FromForm] CustomerRegisterViewModel model)
        {
            var tokenResponse = await _customerAuthService.Register(model);
            return this.ApiSuccessResponse(HttpStatusCode.OK, "Registered Successfully.", tokenResponse);
        }

        [HttpPost("Verify-Account")]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> VerifyAccount([FromBody] CustomerVerifyAccountViewModel model)
        {
            var tokenResponse = await _customerAuthService.VerifyAccount(model.Email, model.OTP);
            return this.ApiSuccessResponse(HttpStatusCode.OK, "Verified Successfully.", tokenResponse);
        }
        [HttpPost("forgot-password")]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ForgotPassword([FromBody] CustomerForgotPasswordViewModel model)
        {
            await _customerAuthService.GenerateOtpAndSendForgotPasswordEmail(model);
            return this.ApiSuccessResponse(HttpStatusCode.OK, "OTP sent to the registered email address successfully. Please Check you email.");

        }


        [HttpPost("validate-otp")]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ValidateOtp([FromBody] CustomerValidateOtpViewModel model)
        {
            await _customerAuthService.ValidateOtp(model);
            return this.ApiSuccessResponse(HttpStatusCode.OK, "OTP Validated Successfully.");

        }


        [HttpPost("reset-password")]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ResetPassword([FromBody] CustomerResetPasswordViewModel model)
        {
            await _customerAuthService.ResetPassword(model);
            return this.ApiSuccessResponse(HttpStatusCode.OK, "Password Reset Successfully.");

        }
    }
}
