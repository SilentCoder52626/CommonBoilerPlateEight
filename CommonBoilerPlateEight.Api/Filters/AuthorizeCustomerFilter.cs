
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using CommonBoilerPlateEight.Api.ApiModel;
using CommonBoilerPlateEight.Domain.Enums;
using CommonBoilerPlateEight.Domain.Extensions;
using CommonBoilerPlateEight.Domain.Interfaces;
using System.Net;

namespace CommonBoilerPlateEight.Api.Filters
{
    public class AuthorizeCustomerFilter : IAsyncAuthorizationFilter
    {
        private readonly ICustomerService _customerService;
        public AuthorizeCustomerFilter(ICustomerService customerService)
        {
            _customerService = customerService;
        }


        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var customerId = AppHttpContext.GetCurrentUserId();

            var customer = await _customerService.GetById(customerId).ConfigureAwait(false);
            if (customer == null)
            {
                context.Result = new UnauthorizedObjectResult(new ApiResponseModel
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized,
                    Message = "Unauthorized",
                    Errors = new List<string> { "Customer Does Not Exists." },
                    Status = Notify.Error.ToString()
                });
                return;
            }

            if (!customer.IsActive)
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

