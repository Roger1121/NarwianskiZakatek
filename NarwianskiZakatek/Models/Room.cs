namespace NarwianskiZakatek.Models
{
    public class Room
    {
        public int RoomId { get; set; }
        public string RoomNumber { get; set; }
        public int RoomCapacity { get; set; }
        public virtual ICollection<ReservedRoom> ReservedRooms { get; set; }
    }
}
