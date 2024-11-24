using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CommonBoilerPlateEight.Api.ApiModel;
using CommonBoilerPlateEight.Api.Extensions;
using CommonBoilerPlateEight.Domain.Interfaces;
using CommonBoilerPlateEight.Domain.Models;
using System.Net;

namespace CommonBoilerPlateEight.Api.Controllers.Celebrity
{
    [AllowAnonymous]
    [Route("api/celebrity-auths")]
    [ApiController]
    public class CelebrityAuthController : ControllerBase
    {
        private readonly ICelebrityAuthService _celebrityAuthService;
        public CelebrityAuthController(ICelebrityAuthService celebrityAuthService)
        {
            _celebrityAuthService = celebrityAuthService;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Login([FromBody] CelebrityLoginRequestViewModel model)
        {

            var tokenResponse = await _celebrityAuthService.Login(model);
            return this.ApiSuccessResponse(HttpStatusCode.OK, "Logged In Successfully.", tokenResponse);

        }
        [HttpPost("register")]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RegisterCelebrity([FromForm] CelebrityRegisterRequestViewModel model)
        {
            var tokenResponse = await _celebrityAuthService.Register(model);
            return this.ApiSuccessResponse(HttpStatusCode.Created, "Successfully Created.", tokenResponse);

        }

        [HttpPost("forgot-password")]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ForgotPassword([FromBody] CelebrityForgotPasswordRequestViewModel model)
        {
            await _celebrityAuthService.GenerateOtpAndSendForgotPasswordEmail(model);
            return this.ApiSuccessResponse(HttpStatusCode.OK, "OTP sent to the registered email address successfully. Please Check you email.");

        }


        [HttpPost("validate-otp")]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ValidateOtp([FromBody] CelebrityValidateOtpViewModel model)
        {
            await _celebrityAuthService.ValidateOtp(model);
            return this.ApiSuccessResponse(HttpStatusCode.OK, "OTP Validated Successfully.");

        }

        [HttpPost("reset-password")]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ResetPassword([FromBody] CelebrityResetPasswordViewModel model)
        {
            await _celebrityAuthService.ResetPassword(model);
            return this.ApiSuccessResponse(HttpStatusCode.OK, "Password Reset Successfully.");

        }
    }
}
