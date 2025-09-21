using Abstractions;
using Application.Interfaces;
using Core.Domain.Entities;
using Core.Domain.Interfaces;
using Core.General.Models;
using Core.Supportive.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace Application.Services;
public class AnonymizerService(IMemoryCache cache, IAsyncRepository<Anonymizer> anonymizerRepository) : IAnonymizerService
{
    private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(5); // TODO subscribe on changes from db-trigger or similar logic

    public async Task<Result<IChatRequest>> Anonymize(IChatRequest request)
    {
        var lastMessage = request?.Messages?.Last();
        if (string.IsNullOrWhiteSpace(lastMessage?.Content))
        {
            return Result<IChatRequest>.Failure(
                message: "no content provided", 
                errorType: ErrorType.Validation, 
                logLevel: LogLevel.Information
            );
        }

        var (lookup, regex) = await cache.GetOrCreateAsync<(Dictionary<string, string>, Regex)>("anonymizer-replace", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = _cacheDuration;

            var dict = (await anonymizerRepository.ListAllAsync())
                ?.ToDictionary(k => k.Original!, v => v.Replacement!, StringComparer.OrdinalIgnoreCase) 
                ?? [];

            string pattern = dict.Keys.Count > 0
                ? @"\b(" + string.Join("|", dict.Keys.Select(Regex.Escape)) + @")\b"
                : @"\b(?!)\b";

            return (dict, new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase));
        });

        lastMessage.Content = regex.Replace(lastMessage.Content, match =>
            lookup.TryGetValue(match.Value, out var replacement)
                ? replacement
                : match.Value
        );

        return request.AsResultSuccess();
    }

    public async Task<Result<IChatResponse>> Deanonymize(IChatResponse response)
    {
        var lastMessage = response?.Choices?.Last();
        if (string.IsNullOrWhiteSpace(lastMessage?.Message?.Content))
        {
            var warning = "no content provided";
            logger.LogWarning(warning); // TODO inject applicatoin error handler/manager that creates Results, logs and Events
            return Result<IChatResponse>.Failure(warning);
        }

        var (lookup, regex) = await cache.GetOrCreateAsync<(Dictionary<string, string>, Regex)>("anonymizer-reconstruct", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = _cacheDuration;

            var dict = (await anonymizerRepository.ListAllAsync())
                ?.ToDictionary(k => k.Replacement!, v => v.Original!, StringComparer.OrdinalIgnoreCase)
                ?? [];

            string pattern = dict.Keys.Count > 0
                ? @"\b(" + string.Join("|", dict.Keys.Select(Regex.Escape)) + @")\b"
                : @"\b(?!)\b";

            return (dict, new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase));
        });

        lastMessage.Message.Content = regex.Replace(lastMessage.Message.Content, match =>
            lookup.TryGetValue(match.Value, out var replacement)
                ? replacement
                : match.Value
        );

        return response.AsResultSuccess();
    }
}
