using SKStreaming.ConsoleApp.Clients;
using SKStreaming.ConsoleApp.Models;
using SKStreaming.ConsoleApp.Options;

namespace SKStreaming.ConsoleApp.Services;

public interface IStreamingService
{
    Task RunAsync(string[] args);
}

public class StreamingService(IEnumerable<IApiClient> clients) : IStreamingService
{
    private readonly IEnumerable<IApiClient> _clients = clients ?? throw new ArgumentNullException(nameof(clients));

    public async Task RunAsync(string[] args)
    {
        var options = ArgumentOptions.Parse(args);
        if (options.Help)
        {
            this.DisplayHelp();
            return;
        }

        if (options.QuestionType == QuestionType.Undefined)
        {
            this.DisplayHelp();
            return;
        }

        try
        {
            var client = default(IApiClient);
            client = options.QuestionType switch
            {
                QuestionType.Chat => this._clients.OfType<CompleteChatClient>().SingleOrDefault(),
                QuestionType.ChatStreaming => this._clients.OfType<CompleteChatStreamingClient>().SingleOrDefault(),
                QuestionType.Booking => throw new InvalidOperationException("Invalid question type"),
                _ => throw new InvalidOperationException("Invalid question type"),
            };

            while (true)
            {
                Console.Write("User: ");
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    break;
                }

                var prompt = new PromptRequest(input);

                Console.Write("Assistant: ");

                if (options.QuestionType == QuestionType.Chat)
                {
                    var result1 = await client!.InvokeAsync<PromptRequest, PromptResponse>(prompt).ConfigureAwait(false);

                    Console.WriteLine(result1!.Content);
                }
                else
                {
                    var result2 = await client!.InvokeStreamAsync<PromptRequest, PromptResponse>(prompt);

                    await foreach (var item in result2)
                    {
                        await Task.Delay(20);
                        Console.Write(item!.Content);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void DisplayHelp()
    {
        Console.WriteLine("Usage:");
        Console.WriteLine("  -t, --question-type    The type of question. Possible values: chat, chatstreaming, chat-streaming, booking");
        Console.WriteLine("  -h, --help     Display help");
    }
}
