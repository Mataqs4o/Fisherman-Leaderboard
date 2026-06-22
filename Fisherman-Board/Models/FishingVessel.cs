using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Fisherman_Board.Models;

public class FishingVessel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Международният номер е задължителен.")]
    [StringLength(80)]
    [Display(Name = "Международен номер")]
    public string InternationalNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Позивната е задължителна.")]
    [StringLength(50)]
    [Display(Name = "Позивна")]
    public string CallSign { get; set; } = string.Empty;

    [Required(ErrorMessage = "Маркировката е задължителна.")]
    [StringLength(80)]
    [Display(Name = "Маркировка")]
    public string Marking { get; set; } = string.Empty;

    [Range(0.1, 500, ErrorMessage = "Дължината трябва да е по-голяма от 0.")]
    [Display(Name = "Дължина (м)")]
    public double Length { get; set; }

    [Range(0.1, 100, ErrorMessage = "Ширината трябва да е по-голяма от 0.")]
    [Display(Name = "Ширина (м)")]
    public double Width { get; set; }

    [Range(0.1, 50000, ErrorMessage = "Тонажът трябва да е по-голям от 0.")]
    [Display(Name = "Тонаж")]
    public double Tonnage { get; set; }

    [Range(0.1, 100, ErrorMessage = "Газенето трябва да е по-голямо от 0.")]
    [Display(Name = "Газене (м)")]
    public double Draft { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Изберете двигател.")]
    [Display(Name = "Двигател")]
    public int EngineId { get; set; }

    [ValidateNever]
    public Engine Engine { get; set; } = null!;

    [ValidateNever]
    public ICollection<VesselOwner> Owners { get; set; } = new List<VesselOwner>();

    [ValidateNever]
    public ICollection<FishingPermit> Permits { get; set; } = new List<FishingPermit>();

    [ValidateNever]
    public ICollection<FishingTrip> Trips { get; set; } = new List<FishingTrip>();
}