using Microsoft.Extensions.Logging;
using Moq;
using RacingFeedConsumer.Services;

namespace RacingFeedConsumer.Test
{
    [TestFixture]
    public class RabbitMQPublisherServiceTests
    {
        private Mock<ILogger<RabbitMQPublisherService>> _loggerMock;
        private RabbitMQPublisherService _service;
        private CancellationTokenSource _cts;
        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<RabbitMQPublisherService>>();
            _service = new RabbitMQPublisherService(_loggerMock.Object);
            _cts = new CancellationTokenSource();
        }
        [TearDown]
        public void Cleanup()
        {
            _cts.Dispose();
        }
        [Test]
        public async Task PublishAsync_ValidParameters_PublishesSuccessfully()
        {
            // Arrange
            string exchange = "test.exchange";
            string routingKey = "test.route";
            string message = "test message";
            // Act & Assert
            Assert.DoesNotThrowAsync(async () =>
                await _service.PublishAsync(exchange, routingKey, message, _cts.Token));
        }
        [Test]
        public void PublishAsync_CancellationRequested_ThrowsOperationCanceledException()
        {
            // Arrange
            string exchange = "test.exchange";
            string routingKey = "test.route";
            string message = "test message";
            _cts.Cancel();
            // Act & Assert
            Assert.ThrowsAsync<OperationCanceledException>(async () =>
                await _service.PublishAsync(exchange, routingKey, message, _cts.Token));
        }
        [Test]
        public void PublishAsync_NullExchange_ThrowsArgumentNullException()
        {
            // Arrange
            string exchange = null;
            string routingKey = "test.route";
            string message = "test message";
            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await _service.PublishAsync(exchange, routingKey, message, _cts.Token));
        }
        [Test]
        public void PublishAsync_NullRoutingKey_ThrowsArgumentNullException()
        {
            // Arrange
            string exchange = "test.exchange";
            string routingKey = null;
            string message = "test message";
            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await _service.PublishAsync(exchange, routingKey, message, _cts.Token));
        }
        [Test]
        public void PublishAsync_NullMessage_ThrowsArgumentNullException()
        {
            // Arrange
            string exchange = "test.exchange";
            string routingKey = "test.route";
            string message = null;
            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await _service.PublishAsync(exchange, routingKey, message, _cts.Token));
        }
    }
}