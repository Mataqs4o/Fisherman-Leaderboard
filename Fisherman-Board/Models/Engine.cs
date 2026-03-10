using System.Collections.Generic;

namespace Fisherman_Board.Models

{
    public class Engine
    {
        public int Id { get; set; }
        public string Type { get; set; }          // напр. дизел
        public double PowerKw { get; set; }       // мощност
        public double AvgFuelPerHour { get; set; } // литра/час
        public string FuelType { get; set; }

        public ICollection<FishingVessel> Vessels { get; set; } = new List<FishingVessel>();
    }
}

