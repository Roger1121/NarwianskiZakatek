using Microsoft.AspNetCore.Mvc.Rendering;

namespace NarwianskiZakatek.ViewModels
{
    public class AvailableRooms
    {
        public List<SelectListItem> Rooms { get; set; } = new List<SelectListItem>();
    }
}
