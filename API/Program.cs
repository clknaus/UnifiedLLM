using Abstractions.Interfaces;
using API.Configuration;
using Application.Interfaces;
using Application.Services;
using Core.Interfaces;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Polly;
using Polly.Extensions.Http;
using System.Text.Json;
using UnifiedLLM.Clients;

var builder = WebApplication.CreateBuilder(args);

// Configuration
builder.Services.Configure<UnifiedLLMOptions>(
    builder.Configuration.GetSection(UnifiedLLMOptions.SectionName)
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
builder.Services
    .AddHttpClient<IHttpClientService, HttpClientService>()
    .SetHandlerLifetime(TimeSpan.FromMinutes(5))
    .AddPolicyHandler(GetRetryPolicy())
    .AddPolicyHandler(GetCircuitBreakerPolicy());
builder.Services.AddScoped<ILLMClient, LLMClient>();

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowSpecificOrigin",
//        builder => builder.WithOrigins("http://localhost:3000")
//                          .AllowAnyMethod()
//                          .AllowAnyHeader()
//                          .AllowCredentials());
//});

// Application
builder.Services.AddScoped<IChatApplicationService, ChatApplicationService>();

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
