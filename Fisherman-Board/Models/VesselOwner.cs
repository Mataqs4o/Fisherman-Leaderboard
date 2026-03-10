namespace Fisherman_Board.Models

{
    public class VesselOwner
    {
        public int FishingVesselId { get; set; }
        public FishingVessel FishingVessel { get; set; }

        public int PersonId { get; set; }
        public Person Person { get; set; }

        public bool IsCurrent { get; set; }
    }
}
