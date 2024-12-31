using Microsoft.AspNetCore.Mvc;

using SKStreaming.ApiApp.Models;
using SKStreaming.ApiApp.Services;

namespace SKStreaming.ApiApp.Endpoints;

public static class CompleteBookingsStreamingEndpoint
{
    public static IEndpointRouteBuilder MapCompleteBookingsStreamingEndpoint(this IEndpointRouteBuilder routeBuilder)
    {
        var api = routeBuilder.MapGroup("api/bookings");

        api.MapPost("streaming", PostCompleteBookingsStreamingAsync)
           .Accepts<PromptRequest>(contentType: "application/json")
           .Produces<IEnumerable<BookingsResponse>>(statusCode: StatusCodes.Status200OK, contentType: "application/json")
           .WithTags("bookings")
           .WithName("BookingsStreaming")
           .WithOpenApi();

        return routeBuilder;
    }

    public static async IAsyncEnumerable<BookingsResponse> PostCompleteBookingsStreamingAsync([FromBody] BookingsRequest req, IKernelService service)
    {
        var result = service.CompleteFunctionCallingStreamingAsync(req.Prompt);

        await foreach (var text in result)
        {
            yield return new BookingsResponse(text);
        }
    }
}
