using System.ComponentModel.DataAnnotations;

namespace Fisherman_Board.Models
{
    public class Fisherman
    {
        public int Id { get; set; }

        [Required]
        [StringLength(120)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [StringLength(30)]
        public string LicenseNumber { get; set; } = string.Empty;

        [StringLength(100)]
        public string HomePort { get; set; } = string.Empty;

        [StringLength(30)]
        public string PhoneNumber { get; set; } = string.Empty;

        public ICollection<Boat> Boats { get; set; } = new List<Boat>();

        public ICollection<Hunt> Hunts { get; set; } = new List<Hunt>();
    }
}
