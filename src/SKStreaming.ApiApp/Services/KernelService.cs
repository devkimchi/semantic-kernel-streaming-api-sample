using Microsoft.SemanticKernel;

namespace SKStreaming.ApiApp.Services;

public interface IKernelService
{
    Task<string> CompleteChatAsync(string prompt);
}

public class KernelService(Kernel kernel) : IKernelService
{
    private readonly Kernel _kernel = kernel ?? throw new ArgumentNullException(nameof(kernel));

    public async Task<string> CompleteChatAsync(string prompt)
    {
        var result = await this._kernel.InvokePromptAsync(prompt).ConfigureAwait(false);

        return result!.GetValue<string>()!;
    }
}
