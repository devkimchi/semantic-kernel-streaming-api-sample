using Microsoft.SemanticKernel;

using SKStreaming.ApiApp.Endpoints;
using SKStreaming.ApiApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add Semantic Kernel instance as a singleton
builder.Services.AddSingleton<Kernel>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var kernel = Kernel.CreateBuilder()
                       .AddAzureOpenAIChatCompletion(
                           endpoint: config["Azure:OpenAI:Endpoint"]!,
                           apiKey: config["Azure:OpenAI:ApiKey"]!,
                           deploymentName: config["Azure:OpenAI:DeploymentName"]!)
                       .Build();
    return kernel;
});
builder.Services.AddScoped<IKernelService, KernelService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Add endpoint for chat completion through Semantic Kernel
app.MapCompleteChatEndpoint();
app.MapCompleteChatStreamingEndpoint();

app.Run();
