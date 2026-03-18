namespace Fisherman_Board.Models;

public class RecreationalTicket
{
    public int Id { get; set; }

    public int PersonId { get; set; }
    public Person Person { get; set; } = null!;

    public DateTime IssueDate { get; set; }
    public DateTime ValidTo { get; set; }
    public decimal Price { get; set; }

    public TicketType TicketType { get; set; }

    public ICollection<RecreationalCatch> Catches { get; set; } = new List<RecreationalCatch>();
}
