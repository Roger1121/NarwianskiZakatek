namespace NarwianskiZakatek.Models
{
    public class ReservedRoom
    {
        public int ReservedRoomId { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; }
        public string ReservationId { get; set; }
        public Reservation Reservation { get; set; }
    }
}
