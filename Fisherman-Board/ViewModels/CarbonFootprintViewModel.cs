namespace Fisherman_Board.ViewModels
{
    public class CarbonFootprintViewModel
    {
        public string VesselMarking { get; set; } = string.Empty;
        public double TotalCatchKg { get; set; }
        public double TotalFuel { get; set; }
        public double CarbonPerKg { get; set; }
    }
}
