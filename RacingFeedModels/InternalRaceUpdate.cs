namespace RacingFeedModels
{
    public record InternalRaceUpdate
    {
        public int RaceId { get; set; }
        public string RaceLocation { get; set; }
        public int Distance { get; set; }
        public int RaceNumber { get; set; }
        public string RaceType { get; set; }
        public string RaceInfo { get; set; }
        public string TrackCondition { get; set; }
        public DateTime StartTimeUtc { get; set; }
        public List<Runner> Runners { get; set; }
    }

    public class Runner
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int Barrier { get; set; }
        public string Name { get; set; }
        public float WinPrice { get; set; }
        public string Jockey { get; set; }
        public string Trainer { get; set; }
    }

}
