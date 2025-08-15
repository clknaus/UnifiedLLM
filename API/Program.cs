using Application.Events;
using Application.Handler;
using Application.Interfaces;
using Application.Services;
using Core.Domain.Events;
using Core.Domain.Interfaces;
using Core.General.Handler;
using Core.General.Interfaces;
using Core.Supportive.Interfaces;
using Core.Supportive.Interfaces.DomainEvents;
using Core.Supportive.Interfaces.Tracker;
using Infrastructure.Interfaces.Providers.OpenRouter;
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
     .AddJsonFile(Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "Configuration", "appsettings.json"), optional: false, reloadOnChange: true)
     .AddJsonFile(Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "Configuration", "secrets.json"), optional: false, reloadOnChange: true)
    .Build();

var appSettings = config.GetSection("AppSettings").Get<AppSettingsConfiguration>();
if (appSettings == null)
{
    throw new ArgumentNullException();
}

System.Console.WriteLine(appSettings.ApplicationName);
System.Console.WriteLine(appSettings.Version);

// secrets.json
var secrets = config.GetSection("Secrets").Get<SecretsConfiguration>();
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

// Dependency Injections

// Application
builder.Services.AddScoped<IChatService, ChatService>();
// Application Handler
builder.Services.AddScoped<IQueryHandler<IModelsResponse>, GetAvailableModelsQueryHandler>();
builder.Services.AddScoped<ICommandHandler<IChatRequest, IChatResponse>, CreateChatCompletionCommandHandler>();
// Events
builder.Services.AddScoped<IAsyncDomainEventHandler<ChatCompletedEvent>, ChatCompletedHandler>();
builder.Services.AddScoped<IAsyncDomainEventHandler<ErrorLogEvent>, ErrorLogEventHandler>();
// Infrastructure
builder.Services.AddScoped(typeof(IAsyncRepository<>), typeof(EfRepository<>));
builder.Services.AddScoped(typeof(IUnitOfWork), typeof(EfUnitOfWork));
// Domain Supportive
builder.Services.AddScoped(typeof(IDomainEventQueue), typeof(DomainEventQueue));
builder.Services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
builder.Services.AddScoped<ITrackerService<Guid>, TrackerService<Guid>>();
// Domain General
builder.Services.AddScoped<IHashHandler, HashHandler>();

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
