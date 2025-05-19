
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RacingFeedConsumer.Services;

public class MockFileWatcherService : BackgroundService
{
    private readonly IProcessFeedFileService _processor;
    private readonly ILogger<MockFileWatcherService> _logger;
    private readonly IBogusFileGenerator _bogusFileGenerator;
    private readonly HashSet<string> _processedKeys = new();

    public MockFileWatcherService(IProcessFeedFileService processor, ILogger<MockFileWatcherService> logger, IBogusFileGenerator bogusFileGenerator)
    {
        _processor = processor;
        _logger = logger;
        _bogusFileGenerator = bogusFileGenerator;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellation)
    {
        while (!cancellation.IsCancellationRequested)
        {
            try
            {
                var fileName = await _bogusFileGenerator.GenerateBogusDataAsync(cancellation);
                _logger.LogInformation($"Processing new file: {fileName}");
                await _processor.ProcessAsync(fileName, cancellation);
            }
            catch (Exception)
            {

                throw;
            }
            await Task.Delay(TimeSpan.FromSeconds(5), cancellation); // Poll every 5 seconds
        }
    }
}


