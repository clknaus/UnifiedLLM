using API;
using API.Endpoints;
using Core.General.Extensions;
using Infrastructure.Interfaces.Providers.OpenRouter;
using Infrastructure.Models.OpenRouter;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;

var builder = WebApplication.CreateBuilder(args);

// Configuration
var currentDir = Directory.GetCurrentDirectory();
var parentDir = Directory.GetParent(currentDir);

var config = new ConfigurationBuilder()
    .SetBasePath(currentDir)
    .AddJsonFile(Path.Combine(
        parentDir.FullName,
        "Configuration",
        "appsettings.json"),
        optional: false,
        reloadOnChange: true)
    .AddJsonFile(Path.Combine(
        parentDir.FullName, 
        "Configuration", 
        "secrets.json"), 
        optional: false, 
        reloadOnChange: true)
    .Build();

// Validate appSettings.json
var appSettings = config.GetSection("AppSettings").Get<AppSettingsConfiguration>();
appSettings.ThrowIfNull();
appSettings!.Url.ThrowIfNullOrEmpty();
appSettings!.Port.ThrowIfNullOrEmpty();

System.Console.WriteLine(appSettings.ApplicationName);
System.Console.WriteLine(appSettings.Version);

// secrets.json
var secrets = config.GetSection("Secrets").Get<SecretsConfiguration>();
secrets.ThrowIfNull();

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
builder.Services.AddEndpointsApiExplorer();
builder.Services.Configure<OpenRouterConfiguration>(
    builder.Configuration.GetSection(OpenRouterConfiguration.SectionName));


builder.Services
    .AddHttpClient<IProviderClientService, OpenRouterClientService>((serviceProvider, httpClient) =>
    {
        var openRouterConfigurationOption = serviceProvider?.GetRequiredService<IOptions<OpenRouterConfiguration>>()?.Value ?? throw new ArgumentNullException();
        openRouterConfigurationOption.ApiKey = secrets.OpenRouterConfiguration.ApiKey;

        httpClient.BaseAddress = new Uri(openRouterConfigurationOption.BaseUrl);
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {openRouterConfigurationOption.ApiKey}");
    })
    .SetHandlerLifetime(TimeSpan.FromMinutes(5))
    .AddPolicyHandler(GetRetryPolicy())
    .AddPolicyHandler(GetCircuitBreakerPolicy()); // Assuming GetCircuitBreakerPolicy() is defined elsewhere

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase(Guid.NewGuid().ToString()));

IoCDependencyInjector.Map(builder);

// Logging
builder.Logging.AddFilter("System.Net.Http.HttpClient", LogLevel.Information);
builder.Logging.AddFilter("Polly", LogLevel.Debug);
builder.Logging.AddConsole();
builder.Logging.AddDebug();
//builder.WebHost.UseUrls($"{appSettings.Url}:{appSettings.Port}");

var app = builder.Build();
ChatEndpoints.Map(app);

app.UseRouting();
//app.UseAuthentication();
//app.MapControllers();

System.Console.WriteLine($"running {appSettings.Url}:{appSettings.Port}");
System.Console.WriteLine("Application ready!");
app.Run();
