namespace Fisherman_Board.Models;

public class RecreationalCatch
{
    public int Id { get; set; }

    public int RecreationalTicketId { get; set; }
    public RecreationalTicket RecreationalTicket { get; set; } = null!;

    public DateTime CatchDate { get; set; }
    public string WaterBody { get; set; } = string.Empty;
    public string Species { get; set; } = string.Empty;
    public double QuantityKg { get; set; }
}
