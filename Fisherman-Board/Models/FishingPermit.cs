namespace Fisherman_Board.Models

{
    public class FishingPermit
    {
        public int Id { get; set; }

        public int FishingVesselId { get; set; }
        public FishingVessel FishingVessel { get; set; }

        public int OwnerId { get; set; }
        public Person Owner { get; set; }

        public int? CaptainId { get; set; }
        public Person Captain { get; set; }

        public DateTime IssueDate { get; set; }
        public DateTime ValidTo { get; set; }
        public bool IsRevoked { get; set; }

        public ICollection<FishingGear> Gears { get; set; } = new List<FishingGear>();
    }
}
