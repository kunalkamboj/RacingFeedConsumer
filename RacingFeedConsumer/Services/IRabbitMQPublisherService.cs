
namespace RacingFeedConsumer.Services
{
    public interface IRabbitMQPublisherService
    {
        Task PublishAsync(string exchange, string routingKey, string message, CancellationToken cancellation);
    }
}