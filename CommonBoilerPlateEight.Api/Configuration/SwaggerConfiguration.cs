using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using CommonBoilerPlateEight.Api.ApiModel;
using CommonBoilerPlateEight.Domain.Enums;
using System.Text;
using System.Text.Json;

namespace CommonBoilerPlateEight.Api.Configuration
{
    public static class SwaggerConfiguration
    {
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(opt =>
            {
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            },
                            Scheme= "Bearer",
                            In = ParameterLocation.Header,
                            Name ="Bearer"

                        },
                        new string[]{}
                    }
                });
                opt.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1.0",
                    Title = "CommonBoilerPlateEight Api Version 1",
                    Description = "Api with rest pattern",
                    TermsOfService = new Uri("https://examples.com"),
                    Contact = new OpenApiContact
                    {
                        Name = "CommonBoilerPlateEight",
                        Url = new Uri("https://examples.com")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Example License",
                        Url = new Uri("https://license.com")
                    }

                });
            });

            //services.AddApiVersioning(options =>
            //{
            //    options.AssumeDefaultVersionWhenUnspecified = false;
            //    options.DefaultApiVersion = new ApiVersion(1, 0);
            //    options.ReportApiVersions = true;
            //}).AddApiExplorer(options =>
            //{
            //    options.GroupNameFormat = "'v'VVV";
            //    options.SubstituteApiVersionInUrl = true;
            //}); 


        }

        public static void ConfigureSwaggerAuthentication(this IServiceCollection services, string secretKey)
        {
            services.AddAuthentication(a =>
            {
                a.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                a.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
                x.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = async context =>
                    {
                        context.Response.ContentType = "application/json";
                        var data = new ApiResponseModel
                        {
                            StatusCode = (int)System.Net.HttpStatusCode.Unauthorized,
                            Errors = new List<string> { "Unauthenticated" },
                            Status = Notify.Error.ToString(),
                            Message = "Unauthenticated"
                        };
                        await context.Response.WriteAsJsonAsync(data);
                    },
                    OnForbidden = async context =>
                    {
                        context.Response.ContentType = "application/json";
                        var data = new ApiResponseModel
                        {
                            StatusCode = (int)System.Net.HttpStatusCode.Forbidden,
                            Errors = new List<string> { "Unauthorized" },
                            Status = Notify.Error.ToString(),
                            Message = "Unauthorized"
                        };
                        await context.Response.WriteAsJsonAsync(data);
                    },
                };
            });
        }

    }
}

