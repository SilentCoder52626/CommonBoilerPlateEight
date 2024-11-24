using CommonBoilerPlateEight.Api;
using CommonBoilerPlateEight.Api.Middlewares;
using CommonBoilerPlateEight.Domain.Extensions;

var builder = WebApplication.CreateBuilder(args);

//Add services to the container.
builder.Configuration.AddEnvironmentVariables();
var environment = builder.Environment.EnvironmentName;
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true)
    .AddEnvironmentVariables();

var key = builder.Configuration.GetValue<string>("JwtConfig:Key");
builder.Services.ConfigureServices(key);
var app = builder.Build();
AppHttpContext.Services = app.Services;
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseResponseCompression();
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
app.UseMiddleware<ValidationErrorHandlingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
