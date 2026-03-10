namespace Fisherman_Board.Models

{
    public class FishingGear
    {
        public int Id { get; set; }

        public string Name { get; set; }        // мрежа, трал и т.н.
        public string Description { get; set; }

        public int FishingPermitId { get; set; }
        public FishingPermit FishingPermit { get; set; }
    }
}
