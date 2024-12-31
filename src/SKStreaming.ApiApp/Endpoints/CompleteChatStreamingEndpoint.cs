using Microsoft.AspNetCore.Mvc;

using SKStreaming.ApiApp.Models;
using SKStreaming.ApiApp.Services;

namespace SKStreaming.ApiApp.Endpoints;

public static class CompleteChatStreamingEndpoint
{
    public static IEndpointRouteBuilder MapCompleteChatStreamingEndpoint(this IEndpointRouteBuilder routeBuilder)
    {
        var api = routeBuilder.MapGroup("api/chat");

        api.MapPost("complete/streaming", PostCompleteChatStreamingAsync)
           .Accepts<PromptRequest>(contentType: "application/json")
           .Produces<IEnumerable<PromptResponse>>(statusCode: StatusCodes.Status200OK, contentType: "application/json")
           .WithTags("chat")
           .WithName("CompleteChatStreaming")
           .WithOpenApi();

        return routeBuilder;
    }

    public static async IAsyncEnumerable<PromptResponse> PostCompleteChatStreamingAsync([FromBody] PromptRequest req, IKernelService service)
    {
        var result = service.CompleteChatStreamingAsync(req.Prompt);

        await foreach (var text in result)
        {
            yield return new PromptResponse(text);
        }
    }
}
