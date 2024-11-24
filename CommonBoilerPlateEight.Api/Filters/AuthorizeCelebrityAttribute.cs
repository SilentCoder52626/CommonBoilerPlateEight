using Microsoft.AspNetCore.Mvc;
using CommonBoilerPlateEight.Api.Filters;
using CommonBoilerPlateEight.Api.Filters;

namespace CommonBoilerPlateEight.Api.Filters
{
    public class AuthorizeCelebrityAttribute:TypeFilterAttribute
    {
        public AuthorizeCelebrityAttribute() : base(typeof(AuthorizeCelebrityFilter))
        {
        }
    }
}
