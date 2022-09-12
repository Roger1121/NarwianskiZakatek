namespace NarwianskiZakatek.Models
{
    public class Reservation
    {
        public int ReservationId { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Price { get; set; }
        public string? Opinion { get; set; }
        public int Rating { get; set; }
        public virtual ICollection<ReservedRoom> ReservedRooms { get; set; }
    }
}
