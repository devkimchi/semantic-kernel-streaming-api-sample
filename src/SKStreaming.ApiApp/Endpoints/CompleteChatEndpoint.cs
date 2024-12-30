using Microsoft.AspNetCore.Mvc;

using SKStreaming.ApiApp.Models;
using SKStreaming.ApiApp.Services;

namespace SKStreaming.ApiApp.Endpoints;

public static class CompleteChatEndpoint
{
    public static IEndpointRouteBuilder MapCompleteChatEndpoint(this IEndpointRouteBuilder routeBuilder)
    {
        var api = routeBuilder.MapGroup("api/chat");

        api.MapPost("complete", PostChatCompleteAsync)
           .Accepts<PromptRequest>(contentType: "application/json")
           .Produces<PromptResponse>(statusCode: StatusCodes.Status200OK, contentType: "application/json")
           .WithTags("openai")
           .WithName("CompleteChat")
           .WithOpenApi();

        return routeBuilder;
    }

    public static async Task<PromptResponse> PostChatCompleteAsync([FromBody] PromptRequest req, IKernelService service)
    {
        var result = await service.CompleteChatAsync(req.Prompt);

        return new PromptResponse(result);
    }
}
