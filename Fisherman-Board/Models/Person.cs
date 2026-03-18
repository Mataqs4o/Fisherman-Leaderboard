using System.ComponentModel.DataAnnotations;

namespace Fisherman_Board.Models;

public class Person
{
    public int Id { get; set; }

    [Required]
    [StringLength(120)]
    [Display(Name = "Рибар")]
    public string FullName { get; set; } = string.Empty;

    [Display(Name = "Дата на раждане")]
    public DateTime BirthDate { get; set; }

    [Display(Name = "Пенсионер")]
    public bool IsPensioner { get; set; }

    [Display(Name = "С увреждане")]
    public bool IsDisabled { get; set; }

    [Display(Name = "TELK номер")]
    public string? TelkNumber { get; set; }

    public ICollection<VesselOwner> OwnedVessels { get; set; } = new List<VesselOwner>();
    public ICollection<FishingPermit> CaptainPermits { get; set; } = new List<FishingPermit>();
    public ICollection<RecreationalTicket> RecreationalTickets { get; set; } = new List<RecreationalTicket>();
}
