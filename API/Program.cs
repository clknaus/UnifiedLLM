using Abstractions.Interfaces;
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
builder.Services.Configure<OpenRouterConfiguration>(
    builder.Configuration.GetSection(OpenRouterConfiguration.SectionName)
);

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

// Add validation for OpenRouterConfiguration
builder.Services.AddOptions<OpenRouterConfiguration>()
    .Bind(builder.Configuration.GetSection(OpenRouterConfiguration.SectionName))
    .Validate(options =>
    {
        if (string.IsNullOrEmpty(options.ApiKey))
            return false;

        if (string.IsNullOrEmpty(options.BaseUrl))
            return false;

        return true;
    }, "OpenRouter configuration is invalid.");

// Register HttpClient
builder.Services
    .AddHttpClient<IOpenRouterClientService, OpenRouterClientService>((serviceProvider, httpClient) =>
    {
        var unifiedLLMOptions = serviceProvider.GetRequiredService<IOptions<OpenRouterConfiguration>>().Value;
        httpClient.BaseAddress = new Uri(unifiedLLMOptions.BaseUrl);
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {unifiedLLMOptions.ApiKey}");
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
app.Run();
