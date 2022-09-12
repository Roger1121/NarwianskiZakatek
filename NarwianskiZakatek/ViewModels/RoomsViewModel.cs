using NarwianskiZakatek.CustomAnnotations;
using System.ComponentModel.DataAnnotations;

namespace NarwianskiZakatek.ViewModels
{
    public class RoomsViewModel
    {
        [Required(ErrorMessage = "Nie podano daty rozpoczęcia")]
        [Display(Name = "Data rozpoczęcia")]
        public DateTime BeginDate { get; set; }

        [Required(ErrorMessage = "Nie podano daty zakończenia")]
        [Display(Name = "Data zakończenia")]
        public DateTime EndDate { get; set; }

        [CollectionNotNullOrEmpty(ErrorMessage = "Nie wybranożadnego pokoju")]
        public List<int> Rooms { get; set; } = new List<int>();
    }
}
