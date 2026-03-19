using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fisherman_Board.Models
{
    public class Boat
    {
        public int Id { get; set; }

        [Required]
        [StringLength(80)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(30)]
        public string RegistrationNumber { get; set; } = string.Empty;

        [Column(TypeName = "decimal(6,2)")]
        public decimal LengthMeters { get; set; }

        [StringLength(80)]
        public string EngineModel { get; set; } = string.Empty;

        [Column(TypeName = "decimal(8,2)")]
        public decimal FuelConsumptionPerHour { get; set; }

        public int FishermanId { get; set; }

        public Fisherman Fisherman { get; set; } = null!;

        public ICollection<Hunt> Hunts { get; set; } = new List<Hunt>();
    }
}
