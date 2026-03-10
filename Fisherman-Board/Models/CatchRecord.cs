namespace Fisherman_Board.Models

{
    public class CatchRecord
    {
        public int Id { get; set; }

        public int FishingTripId { get; set; }
        public FishingTrip FishingTrip { get; set; }

        public string Species { get; set; }
        public double QuantityKg { get; set; }
    }
}
