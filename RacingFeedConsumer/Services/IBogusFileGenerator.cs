
namespace RacingFeedConsumer.Services
{
    public interface IBogusFileGenerator
    {
        Task<string> GenerateBogusDataAsync(CancellationToken token);
    }
}