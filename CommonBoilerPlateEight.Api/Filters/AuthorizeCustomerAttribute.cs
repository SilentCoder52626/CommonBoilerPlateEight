using Microsoft.AspNetCore.Mvc;
namespace CommonBoilerPlateEight.Api.Filters
{
    public class AuthorizeCustomerAttribute:TypeFilterAttribute
    {
        public AuthorizeCustomerAttribute() : base(typeof(AuthorizeCustomerFilter))
        {
        }
    }
}
