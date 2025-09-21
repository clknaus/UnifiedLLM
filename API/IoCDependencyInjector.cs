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
using Infrastructure.Persistence.Repositories;

namespace API;

public static class IoCDependencyInjector
{
    public static void Map(WebApplicationBuilder builder)
    {
        // Application
        builder.Services.AddScoped<IChatService, ChatService>();
        builder.Services.AddScoped<IAnonymizerService, AnonymizerService>();
        // Application Handler
        builder.Services.AddScoped<IQueryHandler<IModelsResponse>, GetAvailableModelsQueryHandler>();
        builder.Services.AddScoped<ICommandHandler<IChatRequest, IChatResponse>, CreateChatCompletionCommandHandler>();
        builder.Services.AddScoped<IHandlerManagerService, HandlerManagerService>();
        // Events
        builder.Services.AddScoped<IAsyncDomainEventHandler<ChatCompletedEvent>, ChatCompletedHandler>();
        builder.Services.AddScoped<IAsyncDomainEventHandler<LogEvent>, LogEventHandler>();
        // Infrastructure
        builder.Services.AddScoped(typeof(IAsyncRepository<>), typeof(EfRepository<>));
        builder.Services.AddScoped(typeof(IUnitOfWork), typeof(EfUnitOfWork));
        // Domain Supportive
        builder.Services.AddScoped(typeof(IDomainEventQueue), typeof(DomainEventQueue));
        builder.Services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        builder.Services.AddScoped<ITrackerService<Guid>, TrackerService<Guid>>();
        // Domain General
        builder.Services.AddScoped<IHashHandler, HashHandler>();

    }
}
