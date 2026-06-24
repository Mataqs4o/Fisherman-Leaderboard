using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Fisherman_Board.Models;

public class CatchRecord
{
    public int Id { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Изберете рибар.")]
    [Display(Name = "Рибар")]
    public int PersonId { get; set; }

    [ValidateNever]
    public Person Person { get; set; } = null!;

    [Range(1, int.MaxValue, ErrorMessage = "Изберете излет.")]
    [Display(Name = "Излет")]
    public int FishingTripId { get; set; }

    [ValidateNever]
    public FishingTrip FishingTrip { get; set; } = null!;

    [Required(ErrorMessage = "Видът риба е задължителен.")]
    [StringLength(80)]
    [Display(Name = "Вид риба")]
    public string Species { get; set; } = string.Empty;

    [Range(0.1, 100000, ErrorMessage = "Количеството трябва да е по-голямо от 0.")]
    [Display(Name = "Количество (кг)")]
    public double QuantityKg { get; set; }
}