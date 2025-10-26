using API;
using API.Endpoints;
using Core.Domain.Entities;
using Core.General.Extensions;
using Core.Supportive.Interfaces;
using Infrastructure.Interfaces.Providers.OpenRouter;
using Infrastructure.Models.OpenRouter;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using System.Text.Json;

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

var dbName = $"SharedInMemoryDb-{Guid.NewGuid()}"; // Fixed name ensures sharing across scopes/instances
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase(dbName));

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

// TODO remove
// Seed the repository on startup (async, so use a hosted service or run it here...)
using (var scope = app.Services.CreateScope())
{
    var anonymizerRepository = scope.ServiceProvider.GetRequiredService<IAsyncRepository<Anonymizer>>();

    // Check if data already exists to avoid duplicates
    var jsonFilePath = "..\\Configuration\\anonymizer.json";
    if (!File.Exists(jsonFilePath))
    {
        throw new FileNotFoundException($"JSON file not found: {jsonFilePath}");
    }

    var jsonContent = await File.ReadAllTextAsync(jsonFilePath);
    var anonymizers = JsonSerializer.Deserialize<List<Anonymizer>>(jsonContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    if (anonymizers == null || anonymizers.Count == 0)
    {
        throw new ArgumentNullException();
    }

    Anonymizer anonymizerEntity;
    foreach (var a in anonymizers)
    {
        anonymizerEntity = new Anonymizer() { Original = a.Original, Replacement = a.Replacement };
        if (anonymizerEntity.ValidateThenRaiseEvent().IsFailure)
        {
            continue;
        }

        await anonymizerRepository.AddAsync(new Anonymizer() { Original = a.Original, Replacement = a.Replacement });
    }

    await anonymizerRepository.SaveChangesAsync();
}

System.Console.WriteLine($"running {appSettings.Url}:{appSettings.Port}");
System.Console.WriteLine("Application ready!");
app.Run();

