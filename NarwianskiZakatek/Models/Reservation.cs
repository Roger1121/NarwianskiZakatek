using System.ComponentModel.DataAnnotations;

namespace NarwianskiZakatek.Models
{
    public class Reservation
    {
        public string ReservationId { get; set; } = Guid.NewGuid().ToString();
        public string UserId { get; set; } = string.Empty;
        public AppUser User { get; set; } = new AppUser();

        [Display(Name = "Data rozpoczęcia")]
        public DateTime BeginDate { get; set; }
        [Display(Name = "Data zakończenia")]
        public DateTime EndDate { get; set; }
        [Display(Name = "Cena za dobę")]
        public decimal Price { get; set; }
        [Display(Name = "Opinia")]
        public string? Opinion { get; set; }
        [Display(Name = "Ocena")]
        public int Rating { get; set; }
        public bool IsCancelled { get; set; } = false;
        public virtual ICollection<ReservedRoom>? ReservedRooms { get; set; }
    }
}
