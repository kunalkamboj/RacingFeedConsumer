using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RacingFeedConsumer.Common;
using RacingFeedModels;
using System.Collections.Concurrent;

namespace RacingFeedConsumer.Services
{
    public class ProcessFeedFileService : IProcessFeedFileService
    {
        private ILogger<ProcessFeedFileService> _logger { get; }
        private readonly IRabbitMQPublisherService _rabbitMQPublisherService;

        public ProcessFeedFileService(ILogger<ProcessFeedFileService> logger, IRabbitMQPublisherService rabbitMQPublisherService)
        {
            _logger = logger;
            _rabbitMQPublisherService = rabbitMQPublisherService;
        }

        public async Task ProcessAsync(string fileName, CancellationToken cancellation)
        {
            try
            {
                cancellation.ThrowIfCancellationRequested();
                var races = Helper.DeserializeFromFile<RaceUpdates>(fileName);
                _logger.LogInformation($"Messages found: {races.Items.Count().ToString()}");

                //will process in batches of 100
                foreach (var raceChunks in races.Items.Chunk(100))
                {
                    ConcurrentBag<InternalRaceUpdate> raceUpdates = new ConcurrentBag<InternalRaceUpdate>();
                    await Parallel.ForEachAsync(raceChunks, cancellation, async (raceUpdate, token) =>
                    {
                        // Simulate processing
                        var internalRace = Helper.RaceConverter(raceUpdate);
                        await _rabbitMQPublisherService.PublishAsync("racing.feed", "racing.update", JsonConvert.SerializeObject(internalRace), token);
                        raceUpdates.Add(internalRace);
                        await Task.Delay(1000, token);
                    });
                    _logger.LogInformation($"Messages Published: {raceUpdates.Count().ToString()}");

                }

            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Operation was canceled.");
                return;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error processing file: {fileName}");
                throw;
            }
        }
    }
}
