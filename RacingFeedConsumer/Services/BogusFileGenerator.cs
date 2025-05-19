using Bogus;
using RacingFeedConsumer.Common;
using RacingFeedModels;

namespace RacingFeedConsumer.Services
{
    public class BogusFileGenerator : IBogusFileGenerator
    {
        public async Task<string> GenerateBogusDataAsync(CancellationToken cancellation = default)
        {
            cancellation.ThrowIfCancellationRequested();
            // Use Bogus to generate random data
            Randomizer.Seed = new Random(5177);

            var runners = new Faker<RaceUpdateRunner>()
                .RuleFor(r => r.Id, f => (ushort)f.IndexFaker)
                .RuleFor(r => r.TabNo, f => (byte)f.Random.Number(1, 10))
                .RuleFor(r => r.Barrier, f => (byte)f.Random.Number(1, 20))
                .RuleFor(r => r.Name, f => f.Name.FullName())
                .RuleFor(r => r.Price, f => f.Random.Decimal(1.0m, 100.0m))
                .RuleFor(r => r.Jockey, f => f.Name.FullName())
                .RuleFor(r => r.Trainer, f => f.Name.FullName());

            var raceUpdate = new Faker<RaceUpdate>()
                .RuleFor(r => r.MeetingID, f => f.Random.Long(1, 1000))
                .RuleFor(r => r.RaceId, f => f.Random.Long(1, 1000))
                .RuleFor(r => r.RaceLocation, f => f.Address.City())
                .RuleFor(r => r.RaceDistance, f => (ushort)f.Random.Number(1000, 5000))
                .RuleFor(r => r.RaceNo, f => (byte)f.Random.Number(1, 10))
                .RuleFor(r => r.RaceType, f => f.PickRandom(new[] { "Flat", "Jump", "Harness" }))
                .RuleFor(r => r.RaceInfo, f => f.Lorem.Sentence())
                .RuleFor(r => r.TrackCondition, f => f.PickRandom(new[] { "Good(4)", "Soft(2)", "Heavy(3)" }))
                .RuleFor(r => r.Source, f => f.Internet.Url())
                .RuleFor(r => r.PriceType, f => f.PickRandom(new[] { "Win", "Place", "Show" }))
                .RuleFor(r => r.PoolSize, f => (byte)f.Random.Number(1, 200))
                .RuleFor(r => r.StartTime, f => (uint)f.Date.Timespan().TotalSeconds)
                .RuleFor(r => r.CreationTime, f => (uint)f.Date.Timespan().TotalSeconds)
                .RuleFor(r => r.Runners, f => runners.GenerateBetween(3, 10).ToArray());

            var raceUpdates = new RaceUpdates { Items = raceUpdate.Generate(1000) };

            // Serialize to file
            var filePath = Guid.NewGuid().ToString() + ".xml";
            Helper.SerializeToFile(raceUpdates, filePath);
            return filePath;
        }
    }
}
