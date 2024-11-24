using Microsoft.EntityFrameworkCore.Metadata.Internal;
using CommonBoilerPlateEight.Api.Configuration;
using CommonBoilerPlateEight.Infrastructure.Configurations;
namespace CommonBoilerPlateEight.Api
{
    public static class StartUpConfiguration
    {
        public static void ConfigureServices(this IServiceCollection services,string key)
        {
            // Add services to the container.
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddControllersWithViews();
            services.AddMvc().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
            services.ConfigureIdentity();
            services.ConfigureSwagger();
            services.ConfigureSwaggerAuthentication(key);
            services.ConfigureCommonBoilerPlateEightServices();
        }
    }
}
