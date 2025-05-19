// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RacingFeedConsumer.Services;


IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // Register your services here
        services.AddLogging();
        services.AddTransient<IBogusFileGenerator, BogusFileGenerator>();
        services.AddTransient<IProcessFeedFileService, ProcessFeedFileService>();
        services.AddTransient<IRabbitMQPublisherService, RabbitMQPublisherService>();
        services.AddHostedService<MockFileWatcherService>();

    })
    .Build();

var cts = new CancellationTokenSource();


Console.WriteLine("Starting the application...");
await host.RunAsync(cts.Token);

Console.WriteLine("Press Ctrl+C to exit.");
Console.ReadLine();

Console.CancelKeyPress += (sender, e) =>
{
    Console.WriteLine("Exiting the application...");
    cts.Cancel(); 
};

