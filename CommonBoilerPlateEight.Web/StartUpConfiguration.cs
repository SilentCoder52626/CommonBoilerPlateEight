using CommonBoilerPlateEight.Web.AuthenticationAuthorization;
using CommonBoilerPlateEight.Infrastructure.Configurations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
namespace CommonBoilerPlateEight.Web
{
    public static class StartUpConfiguration
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            // Add services to the container.
            services.AddControllersWithViews().AddViewLocalization()
                .AddDataAnnotationsLocalization();
            services.AddMvc()
             .AddMvcOptions(options =>
             {
                 var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                 options.Filters.Add(new AuthorizeFilter(policy));
             }).AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

            services.ConfigureIdentity();
            services.ConfigureAuthentication();
            services.ConfigureCommonBoilerPlateEightServices();

            services.AddLocalization(options => options.ResourcesPath = "Resources");
          

        }
    }
}
