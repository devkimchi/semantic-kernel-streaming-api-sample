using Microsoft.SemanticKernel;

namespace SKStreaming.ApiApp.Services;

public interface IKernelService
{
    Task<string> CompleteChatAsync(string prompt);

    IAsyncEnumerable<string> CompleteChatStreamingAsync(string prompt);

    IAsyncEnumerable<string> CompleteFunctionCallingStreamingAsync(string prompt);
}

public class KernelService(Kernel kernel, IConfiguration config) : IKernelService
{
    private readonly Kernel _kernel = kernel ?? throw new ArgumentNullException(nameof(kernel));
    private readonly IConfiguration _config = config ?? throw new ArgumentNullException(nameof(config));

    public async Task<string> CompleteChatAsync(string prompt)
    {
#pragma warning disable SKEXP0001
        var settings = new PromptExecutionSettings { ServiceId = this._config["SemanticKernel:ServiceId"] };
#pragma warning restore SKEXP0001
        var arguments = new KernelArguments(settings);

        var result = await this._kernel.InvokePromptAsync(prompt, arguments).ConfigureAwait(false);

        return result!.GetValue<string>()!;
    }

    public async IAsyncEnumerable<string> CompleteChatStreamingAsync(string prompt)
    {
#pragma warning disable SKEXP0001
        var settings = new PromptExecutionSettings { ServiceId = this._config["SemanticKernel:ServiceId"] };
#pragma warning restore SKEXP0001
        var arguments = new KernelArguments(settings);

        var result = this._kernel.InvokePromptStreamingAsync(prompt, arguments).ConfigureAwait(false);

        await foreach (var text in result)
        {
            yield return text.ToString();
        }
    }

    public async IAsyncEnumerable<string> CompleteFunctionCallingStreamingAsync(string prompt)
    {
#pragma warning disable SKEXP0001
        var settings = new PromptExecutionSettings
        {
            ServiceId = this._config["SemanticKernel:ServiceId"],
            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
        };
#pragma warning restore SKEXP0001
        var arguments = new KernelArguments(settings);

        var result = this._kernel.InvokePromptStreamingAsync(prompt, arguments).ConfigureAwait(false);

        await foreach (var text in result)
        {
            yield return text.ToString();
        }
    }
}
