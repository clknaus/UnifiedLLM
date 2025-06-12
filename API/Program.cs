using Application.Interfaces;
using Application.Services;
using Core.Interfaces;
using Infrastructure.Interfaces.OpenRouter;
using Infrastructure.Models.OpenRouter;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Configuration
// appsettings.json
var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("secrets.json", optional: false, reloadOnChange: true)
    .Build();

var appSettings = config.GetSection("AppSettings").Get<AppSettings>();
if (appSettings == null)
{
    throw new ArgumentNullException();
}

System.Console.WriteLine(appSettings.ApplicationName);
System.Console.WriteLine(appSettings.Version);

// secrets.json
var secrets = config.GetSection("Secrets").Get<Secrets>();
if (secrets == null)
{
    throw new ArgumentNullException();
}

// Policies
static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() =>
    HttpPolicyExtensions
        .HandleTransientHttpError()
        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy() =>
    HttpPolicyExtensions
        .HandleTransientHttpError()
        .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));

// Infrastructure
builder.Services.Configure<OpenRouterConfiguration>(
    builder.Configuration.GetSection(OpenRouterConfiguration.SectionName));

//builder.Services.AddSingleton(sp =>
//{
//    var config = sp.GetRequiredService<IConfiguration>();
//    var openRouterConfig = config.GetSection(OpenRouterConfiguration.SectionName).Get<OpenRouterConfiguration>();
//    openRouterConfig.ApiKey = secrets.OpenRouterConfiguration.ApiKey;

//    return openRouterConfig;
//});

// Add validation for OpenRouterConfiguration
//builder.Services.AddOptions<OpenRouterConfiguration>()
//    .Bind(builder.Configuration.GetSection(OpenRouterConfiguration.SectionName))
//    .Validate(options =>
//    {
//        if (string.IsNullOrEmpty(options.ApiKey))
//            return false;

//        if (string.IsNullOrEmpty(options.BaseUrl))
//            return false;

//        return true;
//    }, "OpenRouter configuration is invalid.");

// Register HttpClient
builder.Services
    .AddHttpClient<IOpenRouterClientService, OpenRouterClientService>((serviceProvider, httpClient) =>
    {
        var openRouterConfigurationOption = serviceProvider.GetRequiredService<IOptions<OpenRouterConfiguration>>().Value;
        httpClient.BaseAddress = new Uri(openRouterConfigurationOption.BaseUrl);
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {openRouterConfigurationOption.ApiKey}");
        //httpClient.DefaultRequestHeaders.Add("HTTP-Referer", unifiedLLMOptions?.AppReferer);
        //httpClient.DefaultRequestHeaders.Add("X-Title", unifiedLLMOptions?.AppTitle);
    })
    .SetHandlerLifetime(TimeSpan.FromMinutes(5))
    .AddPolicyHandler(GetRetryPolicy())
    .AddPolicyHandler(GetCircuitBreakerPolicy()); // Assuming GetCircuitBreakerPolicy() is defined elsewhere

//builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("MyDatabase"));
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase(Guid.NewGuid().ToString()));

builder.Services.AddScoped(typeof(IAsyncRepository<>), typeof(EfRepository<>));
//using var context = new AppDbContext();
//context.Database.EnsureCreated();

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowSpecificOrigin",
//        builder => builder.WithOrigins("http://localhost:3000")
//                          .AllowAnyMethod()
//                          .AllowAnyHeader()
//                          .AllowCredentials());
//});

// Application
builder.Services.AddScoped<IChatService, ChatService>();

// Controllers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

// Logging
builder.Logging.AddFilter("System.Net.Http.HttpClient", LogLevel.Information);
builder.Logging.AddFilter("Polly", LogLevel.Debug);

var app = builder.Build();
app.UseRouting();
//app.UseAuthentication();
app.MapControllers();

System.Console.WriteLine("Application ready ...");
app.Run();
