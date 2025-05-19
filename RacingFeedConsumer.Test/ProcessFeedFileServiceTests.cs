using Moq;
using Microsoft.Extensions.Logging;
using RacingFeedConsumer.Services;
using RacingFeedModels;
using RacingFeedConsumer.Common;
namespace RacingFeedConsumer.Test.Services
{
    [TestFixture]
    public class ProcessFeedFileServiceTests
    {
        private Mock<ILogger<ProcessFeedFileService>> _loggerMock;
        private Mock<IRabbitMQPublisherService> _publisherMock;
        private ProcessFeedFileService _service;
        private CancellationTokenSource _cts;
        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<ProcessFeedFileService>>();
            _publisherMock = new Mock<IRabbitMQPublisherService>();
            _service = new ProcessFeedFileService(_loggerMock.Object, _publisherMock.Object);
            _cts = new CancellationTokenSource();
        }
        [TearDown]
        public void Cleanup()
        {
            _cts.Dispose();
        }
        [Test]
        public async Task ProcessAsync_ValidFile_ProcessesAllRaces()
        {
            // Arrange
            var testFile = "test_races.xml";
            var raceUpdates = new RaceUpdates
            {
                Items = new List<RaceUpdate>
                {
                    new RaceUpdate
                    {
                        RaceId = 1,
                        RaceLocation = "Test Location",
                        RaceDistance = 1000,
                        RaceNo = 1,
                        RaceType = "Test",
                        Runners = new[]
                        {
                            new RaceUpdateRunner
                            {
                                Id = 1,
                                Name = "Test Runner",
                                TabNo = 1,
                                Barrier = 1
                            }
                        }
                    }
                }
            };
            Helper.SerializeToFile(raceUpdates, testFile);
            // Act
            await _service.ProcessAsync(testFile, _cts.Token);
            // Assert
            _publisherMock.Verify(x => x.PublishAsync(
                It.Is<string>(e => e == "racing.feed"),
                It.Is<string>(r => r == "racing.update"),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()), 
                Times.Once);
            // Cleanup
            if (File.Exists(testFile))
            {
                File.Delete(testFile);
            }
        }
      
        [Test]
        public async Task ProcessAsync_PublisherFails_ThrowsException()
        {
            // Arrange
            var testFile = "test_races.xml";
            var raceUpdates = new RaceUpdates
            {
                Items = new List<RaceUpdate> { new RaceUpdate { RaceId = 1 } }
            };
            Helper.SerializeToFile(raceUpdates, testFile);
            _publisherMock
                .Setup(x => x.PublishAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Publisher failed"));
            // Act & Assert
            Assert.ThrowsAsync<Exception>(async () =>
                await _service.ProcessAsync(testFile, _cts.Token));

        }
    }
}