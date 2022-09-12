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

        [Required]
        public decimal Price { get; set; }
        public virtual ICollection<ReservedRoom>? ReservedRooms { get; set; }

        public override string ToString()
        {
            return "Pokój numer: " + RoomNumber + ", liczba łóżek: " + RoomCapacity + ", cena za noc: " + Price;
        }
    }
}
