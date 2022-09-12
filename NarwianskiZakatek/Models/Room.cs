using NarwianskiZakatek.CustomAnnotations;
using System.ComponentModel.DataAnnotations;

namespace NarwianskiZakatek.Models
{
    public class Room
    {
        public int RoomId { get; set; }

        [RequiredNotNullOrWhitespace(ErrorMessage = "Nie podano numeru pokoju")]
        [Display(Name = "Numer pokoju")]
        public string RoomNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nie podano liczby łóżek w pokoju")]
        [Display(Name = "Liczba łóżek")]
        [Range(1, int.MaxValue, ErrorMessage = "Liczba łóżek musi być liczbą dodatnią")]
        public int RoomCapacity { get; set; }

        [Required(ErrorMessage = "Nie podano ceny noclegu")]
        [Display(Name = "Cena za noc")]
        [Range(0, int.MaxValue, ErrorMessage = "Cena musi być liczbą dodatnią")]
        public decimal Price { get; set; }
        public virtual ICollection<ReservedRoom>? ReservedRooms { get; set; }

        public override string ToString()
        {
            return "Pokój numer: " + RoomNumber + ", liczba łóżek: " + RoomCapacity + ", cena za noc: " + Price;
        }
    }
}
