using System.Net.Http.Json;

namespace SKStreaming.ConsoleApp.Clients;

public class CompleteChatClient(HttpClient http) : ApiClient
{
    private const string REQUEST_URI = "api/chat/complete";

    private readonly HttpClient _http = http ?? throw new ArgumentNullException(nameof(http));

    public async override Task<TOutput> InvokeAsync<TInput, TOutput>(TInput content)
    {
        var response = await http.PostAsJsonAsync<TInput>(REQUEST_URI, content);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<TOutput>();

        return result!;
    }
}
