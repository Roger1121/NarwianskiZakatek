using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace NarwianskiZakatek.ViewModels
{
    public class WarningViewModel
    {
        [Display(Name = "Login")]
        public string UserName {  get; set; }
        [Display(Name = "Treść ostrzeżenia")]
        public string Message { get; set; }
    }
}
