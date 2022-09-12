using System.ComponentModel.DataAnnotations;

namespace NarwianskiZakatek.ViewModels
{
    public class OpinionViewModel
    {
        public int ReservationId { get; set; }

        [Required]
        [Range(0, 10)]
        [Display(Name = "Ocena w skali od 0 do 10")]
        public int Rating { get; set; }

        [Required]
        [Display(Name = "Opis")]
        public string Opinion { get; set; }

    }
}
