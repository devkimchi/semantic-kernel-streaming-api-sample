using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using SKStreaming.ConsoleApp.Clients;
using SKStreaming.ConsoleApp.Services;

var host = Host.CreateDefaultBuilder(args)
               .UseConsoleLifetime()
               .ConfigureServices(services =>
               {
                   var config = new ConfigurationBuilder()
                                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                    .Build();
                   
                   services.AddHttpClient<IApiClient, CompleteChatClient>(client =>
                   {
                       client.BaseAddress = new Uri(config["ApiApp:Endpoints:Http:0"]!);
                   });
                   services.AddHttpClient<IApiClient, CompleteChatStreamingClient>(client =>
                   {
                       client.BaseAddress = new Uri(config["ApiApp:Endpoints:Http:0"]!);
                   });
                   services.AddHttpClient<IApiClient, CompleteBookingsStreamingClient>(client =>
                   {
                       client.BaseAddress = new Uri(config["ApiApp:Endpoints:Http:0"]!);
                   });
                   services.AddScoped<IStreamingService, StreamingService>();
               })
               .Build();

var service = host.Services.GetRequiredService<IStreamingService>();
await service.RunAsync(args);
