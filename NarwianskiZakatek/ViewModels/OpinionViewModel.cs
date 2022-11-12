using System.ComponentModel.DataAnnotations;

namespace NarwianskiZakatek.ViewModels
{
    public class OpinionViewModel
    {
        [Display(Name = "Numer rezerwacji")]
        public string ReservationId { get; set; }

        [Required(ErrorMessage = "Nie wystawiono oceny")]
        [Range(0, 10, ErrorMessage = "Ocena musi być liczbą z zakresu od 0 do 10")]
        [Display(Name = "Ocena w skali od 0 do 10")]
        public int Rating { get; set; }

        [Display(Name = "Opis")]
        public string Opinion { get; set; } = string.Empty;

    }
}
