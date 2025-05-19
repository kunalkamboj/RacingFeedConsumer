using System.Text;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace RacingFeedConsumer.Services
{
    public class RabbitMQPublisherService : IRabbitMQPublisherService 
    {
        private readonly ILogger<RabbitMQPublisherService> _logger;
        private readonly ConnectionFactory _factory;

        public RabbitMQPublisherService(ILogger<RabbitMQPublisherService> logger)
        {
            _logger = logger;
            _factory = new RabbitMQ.Client.ConnectionFactory
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672/")
            };
        }

        public async Task PublishAsync(string exchange, string routingKey, string message, CancellationToken cancellation)
        {
            if (string.IsNullOrEmpty(exchange))
                throw new ArgumentNullException(nameof(exchange));
            if (string.IsNullOrEmpty(routingKey))
                throw new ArgumentNullException(nameof(routingKey));
            if (string.IsNullOrEmpty(message))
                throw new ArgumentNullException(nameof(message));
            cancellation.ThrowIfCancellationRequested();
            try
            {
                using var connection = await _factory.CreateConnectionAsync(cancellation);
                using var channel = await connection.CreateChannelAsync(cancellationToken: cancellation);
                // Declare the exchange (topic type)
                await channel.ExchangeDeclareAsync(
                    exchange: exchange,
                    type: ExchangeType.Topic,
                    durable: true,
                    cancellationToken: cancellation);
                var body = Encoding.UTF8.GetBytes(message);
                await channel.BasicPublishAsync(
                    exchange: exchange,
                    routingKey: routingKey,
                    body: body,
                    cancellationToken: cancellation);
                //_logger.LogInformation("Published message to exchange '{Exchange}' with routing key '{RoutingKey}'", exchange, routingKey);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to publish message to exchange '{Exchange}' with routing key '{RoutingKey}'", exchange, routingKey);
                throw;
            }
        }
    }
}