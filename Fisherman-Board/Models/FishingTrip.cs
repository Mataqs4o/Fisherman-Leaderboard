namespace Fisherman_Board.Models;

public class FishingTrip
{
    public int Id { get; set; }

    public int FishingVesselId { get; set; }
    public FishingVessel FishingVessel { get; set; } = null!;

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public string StartLocation { get; set; } = string.Empty;
    public string EndLocation { get; set; } = string.Empty;

    public ICollection<CatchRecord> Catches { get; set; } = new List<CatchRecord>();
}
