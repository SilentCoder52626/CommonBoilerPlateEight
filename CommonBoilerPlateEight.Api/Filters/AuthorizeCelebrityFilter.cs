

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using CommonBoilerPlateEight.Api.ApiModel;
using CommonBoilerPlateEight.Domain.Enums;
using CommonBoilerPlateEight.Domain.Extensions;
using CommonBoilerPlateEight.Domain.Interfaces;
using System.Net;

namespace CommonBoilerPlateEight.Api.Filters
{
    public class AuthorizeCelebrityFilter : IAsyncAuthorizationFilter
    {
        private readonly ICelebrityService _celebrityService;
        public AuthorizeCelebrityFilter(ICelebrityService celebrityService)
        {
            _celebrityService = celebrityService;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var celebrityId = AppHttpContext.GetCurrentUserId();

            var celebrity = await _celebrityService.GetById(celebrityId).ConfigureAwait(false);
            if (celebrity == null)
            {
                context.Result = new UnauthorizedObjectResult(new ApiResponseModel
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized,
                    Message = "Unauthorized",
                    Errors = new List<string> { "Celebrity Does Not Exists." },
                    Status = Notify.Error.ToString()
                });
                return;
            }

            if (!celebrity.IsActive)
            {
                context.Result = new UnauthorizedObjectResult(new ApiResponseModel
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized,
                    Message = "Unauthorized",
                    Errors = new List<string> { "User is blocked.Please Contact to administrator." },
                    Status = Notify.Error.ToString()
                });
                return;
            }

        }
    }
}

