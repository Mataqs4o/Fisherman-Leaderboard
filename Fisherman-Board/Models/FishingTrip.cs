using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Fisherman_Board.Models;

public class FishingTrip
{
    public int Id { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Изберете лодка.")]
    [Display(Name = "Лодка")]
    public int FishingVesselId { get; set; }

    [ValidateNever]
    public FishingVessel FishingVessel { get; set; } = null!;

    [Display(Name = "Начало на излета")]
    public DateTime StartTime { get; set; }

    [Display(Name = "Край на излета")]
    public DateTime EndTime { get; set; }

    [Required(ErrorMessage = "Началната локация е задължителна.")]
    [StringLength(120)]
    [Display(Name = "Начална локация")]
    public string StartLocation { get; set; } = string.Empty;

    [Required(ErrorMessage = "Крайната локация е задължителна.")]
    [StringLength(120)]
    [Display(Name = "Крайна локация")]
    public string EndLocation { get; set; } = string.Empty;

    [ValidateNever]
    public ICollection<CatchRecord> Catches { get; set; } = new List<CatchRecord>();
}