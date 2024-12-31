using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace SKStreaming.ApiApp.Services;

public interface IKernelService
{
    Task<string> CompleteChatAsync(string prompt);

    IAsyncEnumerable<string> CompleteChatStreamingAsync(string prompt);

    IAsyncEnumerable<string> CompleteFunctionCallingStreamingAsync(string prompt);
}

public class KernelService(Kernel kernel) : IKernelService
{
    private readonly Kernel _kernel = kernel ?? throw new ArgumentNullException(nameof(kernel));

    public async Task<string> CompleteChatAsync(string prompt)
    {
        var result = await this._kernel.InvokePromptAsync(prompt).ConfigureAwait(false);

        return result!.GetValue<string>()!;
    }

    public async IAsyncEnumerable<string> CompleteChatStreamingAsync(string prompt)
    {
        var result = this._kernel.InvokePromptStreamingAsync(prompt).ConfigureAwait(false);

        await foreach (var text in result)
        {
            yield return text.ToString();
        }
    }

    public async IAsyncEnumerable<string> CompleteFunctionCallingStreamingAsync(string prompt)
    {
        var result = this._kernel.InvokePromptStreamingAsync(
            prompt,
            new KernelArguments(new OpenAIPromptExecutionSettings { FunctionChoiceBehavior = FunctionChoiceBehavior.Auto() })
            ).ConfigureAwait(false);

        await foreach (var text in result)
        {
            yield return text.ToString();
        }
    }
}
