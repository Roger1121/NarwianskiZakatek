using Microsoft.AspNetCore.Identity;

namespace NarwianskiZakatek.Models
{
    public class AppUser : IdentityUser
    {
        public bool IsLocked { get; set; }
        public virtual ICollection<Reservation>? Reservations { get; set; }
        public virtual ICollection<Warning>? Warnings { get; set; }
    }
}
