namespace Fisherman_Board.Models

{
    public class Person
    {
        public int Id { get; set; }

        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }

        public bool IsPensioner { get; set; }
        public bool IsDisabled { get; set; }
        public string TelkNumber { get; set; }

        public ICollection<VesselOwner> OwnedVessels { get; set; } = new List<VesselOwner>();
        public ICollection<FishingPermit> CaptainPermits { get; set; } = new List<FishingPermit>();
        public ICollection<RecreationalTicket> RecreationalTickets { get; set; } = new List<RecreationalTicket>();
    }
}
