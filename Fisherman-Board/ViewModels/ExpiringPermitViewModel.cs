namespace Fisherman_Board.ViewModels
{
    public class ExpiringPermitViewModel
    {
        public string VesselMarking { get; set; } = string.Empty;
        public string InternationalNumber { get; set; } = string.Empty;
        public DateTime ValidTo { get; set; }
    }
}
