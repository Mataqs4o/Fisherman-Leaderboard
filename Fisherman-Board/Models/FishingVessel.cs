using System.ComponentModel.DataAnnotations;

namespace Fisherman_Board.Models;

public class FishingVessel
{
    public int Id { get; set; }

    [Required]
    [StringLength(80)]
    [Display(Name = "Международен номер")]
    public string InternationalNumber { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    [Display(Name = "Позивна")]
    public string CallSign { get; set; } = string.Empty;

    [Required]
    [StringLength(80)]
    [Display(Name = "Маркировка")]
    public string Marking { get; set; } = string.Empty;

    [Range(0.1, 500)]
    [Display(Name = "Дължина (м)")]
    public double Length { get; set; }

    [Range(0.1, 100)]
    [Display(Name = "Ширина (м)")]
    public double Width { get; set; }

    [Range(0.1, 50000)]
    [Display(Name = "Тонаж")]
    public double Tonnage { get; set; }

    [Range(0.1, 100)]
    [Display(Name = "Газене (м)")]
    public double Draft { get; set; }

    [Display(Name = "Двигател")]
    public int EngineId { get; set; }
    public Engine Engine { get; set; } = null!;

    public ICollection<VesselOwner> Owners { get; set; } = new List<VesselOwner>();
    public ICollection<FishingPermit> Permits { get; set; } = new List<FishingPermit>();
    public ICollection<FishingTrip> Trips { get; set; } = new List<FishingTrip>();
}
