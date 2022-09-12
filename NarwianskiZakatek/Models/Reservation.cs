using System.ComponentModel.DataAnnotations;

namespace NarwianskiZakatek.Models
{
    public class Reservation
    {
        public int ReservationId { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }

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
        public virtual ICollection<ReservedRoom> ReservedRooms { get; set; }
    }
}
