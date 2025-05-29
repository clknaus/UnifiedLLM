namespace Abstractions.Interfaces;
public interface IHttpClientService
{
    string BaseAddress { set; }
    Task<Stream> TryGetContentStreamAsync(string uri, CancellationToken cancellationToken = default);
}
