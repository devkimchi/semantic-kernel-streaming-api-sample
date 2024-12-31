namespace SKStreaming.ConsoleApp.Clients;

public interface IApiClient
{
    Task<TOutput> InvokeAsync<TInput, TOutput>(TInput content);
    Task<IAsyncEnumerable<TOutput>> InvokeStreamAsync<TInput, TOutput>(TInput content);
}

public abstract class ApiClient : IApiClient
{
    public virtual Task<TOutput> InvokeAsync<TInput, TOutput>(TInput content)
    {
        throw new NotImplementedException();
    }

    public virtual Task<IAsyncEnumerable<TOutput>> InvokeStreamAsync<TInput, TOutput>(TInput content)
    {
        throw new NotImplementedException();
    }
}