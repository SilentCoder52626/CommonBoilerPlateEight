using Microsoft.AspNetCore.Http;
using CommonBoilerPlateEight.Domain.Constants;
using CommonBoilerPlateEight.Domain.Exceptions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CommonBoilerPlateEight.Domain.Extensions
{
    public static class AppHttpContext
    {
        static IServiceProvider services = null;

        /// <summary>
        /// Provides static access to the framework's services provider
        /// </summary>
        public static IServiceProvider Services
        {
            get { return services; }
            set
            {
                if (services != null)
                {
                    throw new Exception("Can't set once a value has already been set.");
                }
                services = value;
            }
        }

        /// <summary>
        /// Provides static access to the current HttpContext
        /// </summary>
        public static HttpContext Current
        {
            get
            {
                IHttpContextAccessor httpContextAccessor = services.GetService(typeof(IHttpContextAccessor)) as IHttpContextAccessor;
                return httpContextAccessor?.HttpContext;
            }
        }

        public static int GetCurrentUserId()
        {
            var userId = 0;
            string authorizationHeader = Current.Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(authorizationHeader))
            {
                var handler = new JwtSecurityTokenHandler();
                var authHeader = authorizationHeader.Replace("Bearer ", "");
                var jsonToken = handler.ReadToken(authHeader);
                var token = handler.ReadToken(authHeader) as JwtSecurityToken;
                userId = Convert.ToInt32(token.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Sub).Value);
            }
            return userId;
        }


        public static string GetType()
        {
            var type = string.Empty;
            string authorizationHeader = Current.Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(authorizationHeader))
            {
                var handler = new JwtSecurityTokenHandler();
                var authHeader = authorizationHeader.Replace("Bearer ", "");
                var jsonToken = handler.ReadToken(authHeader);
                var token = handler.ReadToken(authHeader) as JwtSecurityToken;
                type = token.Claims.First(claim => claim.Type == ClaimTypeConstant.ClaimUserType).Value;
            }
            return type;
        }

        public static int ValidateAndGetCelebrityId()
        {
            var type = GetType();
            if (type != ClaimTypeConstant.UserTypeCelebrity) throw new CustomException("User is not a celebrity.");
            return GetCurrentUserId();
        }

        public static int ValidateAndGetCustomerId()
        {
            var type = GetType();
            if (type != ClaimTypeConstant.UserTypeCustomer) throw new CustomException("User is not a customer.");
            return GetCurrentUserId();
        }

         public static string GetAdminCurrentUserId()
        {
            var currentUserId = Current.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new ArgumentNullException("Current user empty");
            return currentUserId;
        }


    }


}
