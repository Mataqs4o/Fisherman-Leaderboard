using System.ComponentModel.DataAnnotations;

namespace Fisherman_Board.Models;

public class CatchRecord
{
    public int Id { get; set; }

    [Display(Name = "Излет")]
    public int FishingTripId { get; set; }
    public FishingTrip FishingTrip { get; set; } = null!;

    [Required]
    [StringLength(80)]
    [Display(Name = "Вид риба")]
    public string Species { get; set; } = string.Empty;

    [Range(0.1, 100000)]
    [Display(Name = "Количество (кг)")]
    public double QuantityKg { get; set; }
}
