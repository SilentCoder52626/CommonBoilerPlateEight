using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using CommonBoilerPlateEight.Application.Contracts.Services;
using CommonBoilerPlateEight.Domain.Helper;
using CommonBoilerPlateEight.Domain.Interfaces;
using CommonBoilerPlateEight.Domain.Services;
using CommonBoilerPlateEight.Infrastructure.Context;

namespace CommonBoilerPlateEight.Infrastructure.Configurations
{
    public static class ServiceConfigurations
    {
        public static IServiceCollection ConfigureCommonBoilerPlateEightServices(this IServiceCollection services)
        {
            services.AddDbContext<IDbContext, CommonBoilerPlateEightDbContext>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.EnableForHttps = true;
            });
            services.Configure<GzipCompressionProviderOptions>(options =>
             {
                 options.Level = System.IO.Compression.CompressionLevel.Fastest;
             });
            services.UseDIConfig();
            return services;
        }

        public static void UseDIConfig(this IServiceCollection services)
        {
            UseService(services);
        }

        private static void UseService(IServiceCollection services)
        {
            //Register services here
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IFileUploaderService, FileUploaderService>();
            services.AddScoped<ISettingService, SettingService>();
            services.AddScoped<IApplicationSettingService, ApplicationSettingService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ICustomerAuthService, CustomerAuthService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ICompanyTypeService, CompanyTypeService>();

        }
    }
}
