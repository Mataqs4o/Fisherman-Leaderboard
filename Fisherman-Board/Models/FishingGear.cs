namespace Fisherman_Board.Models;

public class FishingGear
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public int FishingPermitId { get; set; }
    public FishingPermit FishingPermit { get; set; } = null!;
}
