namespace Fisherman_Board.ViewModels;

public class HomeDashboardViewModel
{
    public bool DatabaseAvailable { get; set; }
    public int TotalVessels { get; set; }
    public int ActivePermits { get; set; }
    public int TripsThisYear { get; set; }
    public int RecreationalTickets { get; set; }
    public string TopInsight { get; set; } = string.Empty;
    public string AlertInsight { get; set; } = string.Empty;
    public List<DashboardActionViewModel> ReportCards { get; set; } = [];
    public List<DashboardActionViewModel> RegistryCards { get; set; } = [];
}
