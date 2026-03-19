namespace Fisherman_Board.ViewModels;

public class ReportsIndexViewModel
{
    public bool DatabaseAvailable { get; set; }
    public int ExpiringPermitCount { get; set; }
    public int RecreationalRankingCount { get; set; }
    public int VesselStatsCount { get; set; }
    public int CarbonRankingCount { get; set; }
    public string RecreationalLeaderSummary { get; set; } = string.Empty;
    public string CarbonLeaderSummary { get; set; } = string.Empty;
}
