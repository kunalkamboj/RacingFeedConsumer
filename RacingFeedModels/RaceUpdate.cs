using System.ComponentModel;
using System.Xml.Serialization;

namespace RacingFeedModels
{
    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public record RaceUpdate
    {
        public long MeetingID { get; set; }
        public long RaceId { get; set; }
        public string RaceLocation { get; set; }
        public ushort RaceDistance { get; set; }
        public byte RaceNo { get; set; }
        public string RaceType { get; set; }
        public string RaceInfo { get; set; }
        public string TrackCondition { get; set; }
        public string Source { get; set; }
        public string PriceType { get; set; }
        public byte PoolSize { get; set; }
        public uint StartTime { get; set; }
        public uint CreationTime { get; set; }

        [XmlArrayItem("Runner", IsNullable = false)]
        public RaceUpdateRunner[] Runners { get; set; }
    }

    /// <remarks/>
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public partial class RaceUpdateRunner
    {
        [XmlAttribute]
        public ushort Id { get; set; }

        [XmlAttribute]
        public byte TabNo { get; set; }

        [XmlAttribute]
        public byte Barrier { get; set; }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public decimal Price { get; set; }

        [XmlAttribute]
        public string Jockey { get; set; }

        [XmlAttribute]
        public string Trainer { get; set; }
    }
}