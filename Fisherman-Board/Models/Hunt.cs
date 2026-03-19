using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fisherman_Board.Models
{
    public class Hunt
    {
        public int Id { get; set; }

        public int FishermanId { get; set; }

        public Fisherman Fisherman { get; set; } = null!;

        public int BoatId { get; set; }

        public Boat Boat { get; set; } = null!;

        public DateTime StartedAt { get; set; }

        public DateTime EndedAt { get; set; }

        [Required]
        [StringLength(80)]
        public string FishType { get; set; } = string.Empty;

        [Column(TypeName = "decimal(10,2)")]
        public decimal QuantityKg { get; set; }

        [StringLength(120)]
        public string FishingArea { get; set; } = string.Empty;
    }
}
