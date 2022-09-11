using Microsoft.AspNetCore.Identity;

namespace NarwianskiZakatek.Models
{
    public class AppUser : IdentityUser
    {
        public bool IsLocked { get; set; }
        public string City { get; set; }
        public string? Street { get; set; }
        public string BuildingNumber { get; set; }
        public string? LocalNumber { get; set; }
        public string PostalCode { get; set; }
        public string PostCity { get; set; }
        public virtual ICollection<Reservation>? Reservations { get; set; }
        public virtual ICollection<Warning>? Warnings { get; set; }
    }
}
