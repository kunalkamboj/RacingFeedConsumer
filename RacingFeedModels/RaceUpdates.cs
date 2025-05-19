using RacingFeedModels;
using System.Xml.Serialization;

[XmlRoot("RaceUpdates")]
public class RaceUpdates
{
    [XmlElement("RaceUpdate")]
    public List<RaceUpdate> Items { get; set; } = new();
}