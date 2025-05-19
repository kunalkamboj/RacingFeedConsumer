using RacingFeedModels;
using System.Xml;
using System.Xml.Serialization;

namespace RacingFeedConsumer.Common
{
    public static class Helper
    {
        public static void SerializeToFile<T>(T obj, string filePath)
        {
            var serializer = new XmlSerializer(typeof(T));
            using var writer = new XmlTextWriter(filePath, System.Text.Encoding.UTF8) { Formatting = Formatting.Indented };
            serializer.Serialize(writer, obj);
        }

        public static T DeserializeFromFile<T>(string filePath)
        {
            if (File.Exists(filePath))
            {
                var serializer = new XmlSerializer(typeof(T));
                using (var reader = new XmlTextReader(filePath))
                {
                    var deserializedObject = serializer.Deserialize(reader);
                    if (deserializedObject is null)
                    {
                        throw new InvalidOperationException(
                            $"Deserialization of file '{filePath}' resulted in a null object.");
                    }
                    return (T)deserializedObject;
                }

            }
            else
            {
                throw new FileNotFoundException($"File '{filePath}' not found.");   
            }
        }

        public static InternalRaceUpdate RaceConverter(RaceUpdate race)
        {
            var internalRace = new InternalRaceUpdate
            {
                RaceId = (int)race.RaceId,
                RaceLocation = race.RaceLocation,
                Distance = race.RaceDistance,
                RaceNumber = race.RaceNo,
                RaceType = race.RaceType,
                RaceInfo = race.RaceInfo,
                TrackCondition = race.TrackCondition,
                StartTimeUtc = UnixTimeStampToDateTime(race.StartTime),
                Runners = new List<Runner>()
            };

            if (race.Runners != null)
            {
                foreach (var runner in race.Runners)
                {
                    internalRace.Runners.Add(new Runner
                    {
                        Id = runner.Id,
                        Number = runner.TabNo,
                        Barrier = runner.Barrier,
                        Name = runner.Name,
                        WinPrice = (float)runner.Price,
                        Jockey = runner.Jockey,
                        Trainer = runner.Trainer
                    });
                }
            }

            return internalRace; 
        }

        private static DateTime UnixTimeStampToDateTime(uint unixTime)
        {
            // Assumes unixTime is in seconds since epoch (UTC)
            return DateTimeOffset.FromUnixTimeSeconds(unixTime).UtcDateTime;
        }
    }
}
