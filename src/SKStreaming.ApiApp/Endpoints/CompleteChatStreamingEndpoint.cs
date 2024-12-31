using Microsoft.AspNetCore.Mvc;

using SKStreaming.ApiApp.Models;
using SKStreaming.ApiApp.Services;

namespace SKStreaming.ApiApp.Endpoints;

public static class CompleteChatStreamingEndpoint
{
    public static IEndpointRouteBuilder MapCompleteChatStreamingEndpoint(this IEndpointRouteBuilder routeBuilder)
    {
        var api = routeBuilder.MapGroup("api/chat");

        api.MapPost("complete/streaming", PostChatCompleteStreamingAsync)
           .Accepts<PromptRequest>(contentType: "application/json")
           .Produces<IEnumerable<PromptResponse>>(statusCode: StatusCodes.Status200OK, contentType: "application/json")
           .WithTags("openai")
           .WithName("CompleteChatStreaming")
           .WithOpenApi();

        return routeBuilder;
    }

    public static async IAsyncEnumerable<PromptResponse> PostChatCompleteStreamingAsync([FromBody] PromptRequest req, IKernelService service)
    {
        var result = service.CompleteChatStreamingAsync(req.Prompt);

        await foreach (var text in result)
        {
            yield return new PromptResponse(text);
        }
    }
}
