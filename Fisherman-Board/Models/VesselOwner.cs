namespace Fisherman_Board.Models;

public class VesselOwner
{
    public int FishingVesselId { get; set; }
    public FishingVessel FishingVessel { get; set; } = null!;

    public int PersonId { get; set; }
    public Person Person { get; set; } = null!;

    public bool IsCurrent { get; set; }
}
