namespace Fisherman_Board.Models

{
    public class RecreationalCatch
    {
        public int Id { get; set; }

        public int RecreationalTicketId { get; set; }
        public RecreationalTicket RecreationalTicket { get; set; }

        public DateTime CatchDate { get; set; }
        public string WaterBody { get; set; }
        public string Species { get; set; }
        public double QuantityKg { get; set; }
    }
}
