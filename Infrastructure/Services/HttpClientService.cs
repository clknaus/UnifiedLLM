using Abstractions.Interfaces;

namespace Infrastructure.Services;

public class HttpClientService(HttpClient httpClient) : IHttpClientService
{
    public string BaseAddress { set => httpClient.BaseAddress = new Uri(value); }
    public async Task<Stream> TryGetContentStreamAsync(string uri, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await httpClient.GetAsync(uri, cancellationToken);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStreamAsync(cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            throw new HttpRequestException($"HTTP request to '{uri}' failed.", ex);
        }
    }
}
