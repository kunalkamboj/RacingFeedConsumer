namespace RacingFeedConsumer.Services
{
    public interface IProcessFeedFileService
    {
        Task ProcessAsync(string fileName, CancellationToken cancellation);
    }
}