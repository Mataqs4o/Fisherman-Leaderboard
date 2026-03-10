namespace Fisherman_Board.Models

{
    public class FishingVessel
    {
        public int Id { get; set; }

        public string InternationalNumber { get; set; }
        public string CallSign { get; set; }
        public string Marking { get; set; }

        public double Length { get; set; }
        public double Width { get; set; }
        public double Tonnage { get; set; }
        public double Draft { get; set; }

        public int EngineId { get; set; }
        public Engine Engine { get; set; }

        public ICollection<VesselOwner> Owners { get; set; } = new List<VesselOwner>();
        public ICollection<FishingPermit> Permits { get; set; } = new List<FishingPermit>();
        public ICollection<FishingTrip> Trips { get; set; } = new List<FishingTrip>();
    }
}
