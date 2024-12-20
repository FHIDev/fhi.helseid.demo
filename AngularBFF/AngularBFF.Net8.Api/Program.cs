using AngularBFF.Net8.Api.Weather;
using Fhi.HelseId.Api.ExtensionMethods;
using Fhi.HelseId.Common.Configuration;
using Fhi.HelseId.Web.ExtensionMethods;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets("b1a93959-6172-416e-bd25-8d43347eb8f3");
}
builder.Services.AddLogging();
builder.Services.AddTransient<IWeatherForecastService, WeatherForecastService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHelseIdAuthenticationServicesForApis(
[
    new ApiOutgoingKonfigurasjon() { Name = "WeatherApi", Url = "https://localhost:7278/" }
]);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var authBuilder = builder.AddHelseIdWebAuthentication()
    .UseJwkKeySecretHandler()
    .Build();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseHelseIdProtectedPaths();

app.MapControllers();
app.MapFallbackToFile("/index.html");

app.Run();
