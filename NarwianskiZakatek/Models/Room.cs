using System.ComponentModel.DataAnnotations;

namespace NarwianskiZakatek.Models
{
    public class Room
    {
        public int RoomId { get; set; }
        [Required]
        public string RoomNumber { get; set; }
        [Required]
        [Range(1, 10)]
        public int RoomCapacity { get; set; }
        public virtual ICollection<ReservedRoom>? ReservedRooms { get; set; }
    }
}
