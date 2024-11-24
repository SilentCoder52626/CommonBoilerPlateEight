using Microsoft.AspNetCore.Mvc;
using CommonBoilerPlateEight.Domain.Enums;
using CommonBoilerPlateEight.Web.Models;
using System.Net;

namespace CommonBoilerPlateEight.Web.Extensions
{
    public class APIResponseResult : IActionResult
    {
        private readonly ApiResponseModel _response;

        public APIResponseResult(ApiResponseModel response)
        {
            _response = response;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var objectResult = new ObjectResult(_response)
            {
                StatusCode = _response.StatusCode
            };
            await objectResult.ExecuteResultAsync(context);
        }
    }

    public static class ControllerExtensions
    {


        public static IActionResult ApiErrorResponse(this ControllerBase controller, HttpStatusCode code, List<string> errors, string status, string message = "Error")
        {
            var response = new ApiResponseModel
            {
                StatusCode = (int)code,
                Message = message,
                Errors = errors,
                Status = status
            };

            return new APIResponseResult(response);
        }


        public static IActionResult ApiSuccessResponse(this ControllerBase controller, HttpStatusCode statusCode, string message = "Success", object data = null)
        {
            if (statusCode == HttpStatusCode.NoContent)
            {
                return new StatusCodeResult((int)statusCode);
            }

            var response = new ApiResponseModel
            {
                StatusCode = (int)statusCode,
                Data = data,
                Message = message,
                Status = Notify.Success.ToString()
            };

            return new APIResponseResult(response);
        }
    }

}
