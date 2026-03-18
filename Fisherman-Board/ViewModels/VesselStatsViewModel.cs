namespace Fisherman_Board.ViewModels
{
    public class VesselStatsViewModel
    {
        public string VesselMarking { get; set; } = string.Empty;
        public int TripsCount { get; set; }
        public double TotalCatchKg { get; set; }

        public double AvgTripDurationHours { get; set; }
        public double MinTripDurationHours { get; set; }
        public double MaxTripDurationHours { get; set; }

        public double AvgCatchPerTrip { get; set; }
        public double MinCatchPerTrip { get; set; }
        public double MaxCatchPerTrip { get; set; }
    }
}
