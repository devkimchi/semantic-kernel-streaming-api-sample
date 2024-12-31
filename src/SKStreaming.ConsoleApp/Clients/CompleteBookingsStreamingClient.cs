using System.Net.Http.Json;

namespace SKStreaming.ConsoleApp.Clients;

public class CompleteBookingsStreamingClient(HttpClient http) : ApiClient
{
    private const string REQUEST_URI = "api/bookings/streaming";

    private readonly HttpClient _http = http ?? throw new ArgumentNullException(nameof(http));

    public async override Task<IAsyncEnumerable<TOutput>> InvokeStreamAsync<TInput, TOutput>(TInput content)
    {
        var response = await http.PostAsJsonAsync<TInput>(REQUEST_URI, content);
        response.EnsureSuccessStatusCode();

        var result = response.Content.ReadFromJsonAsAsyncEnumerable<TOutput>();

        return result!;
    }
}
