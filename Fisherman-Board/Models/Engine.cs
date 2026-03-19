using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fisherman_Board.Models;

public class Engine
{
    public int Id { get; set; }

    [Required]
    [StringLength(80)]
    [Display(Name = "Тип двигател")]
    public string Type { get; set; } = string.Empty;

    [Range(0.1, 50000)]
    [Display(Name = "Мощност (kW)")]
    public double PowerKw { get; set; }

    [Range(0.1, 5000)]
    [Display(Name = "Среден разход (л/ч)")]
    public double AvgFuelPerHour { get; set; }

    [Required]
    [StringLength(60)]
    [Display(Name = "Гориво")]
    public string FuelType { get; set; } = string.Empty;

    public ICollection<FishingVessel> Vessels { get; set; } = new List<FishingVessel>();
}
