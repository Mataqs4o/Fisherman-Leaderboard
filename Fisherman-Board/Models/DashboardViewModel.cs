namespace Fisherman_Board.Models
{
    public class DashboardViewModel
    {
        public List<Fisherman> Fishermen { get; set; } = new();

        public List<Boat> Boats { get; set; } = new();

        public List<Hunt> Hunts { get; set; } = new();
    }
}
