using Microsoft.AspNetCore.Identity;

namespace NarwianskiZakatek.Models
{
    public class AppUser : IdentityUser
    {
        public bool IsLocked { get; set; }
        public string City { get; set; } = string.Empty;
        public string? Street { get; set; }
        public string BuildingNumber { get; set; } = string.Empty;
        public string? LocalNumber { get; set; }
        public string PostalCode { get; set; } = string.Empty;
        public string PostCity { get; set; } = string.Empty;
        public virtual ICollection<Reservation>? Reservations { get; set; }
        public virtual ICollection<Warning>? Warnings { get; set; }

        public string GetAddress() 
        {
            string address =  City + "\r\n" + Street + BuildingNumber;
            if(LocalNumber != null)
            {
                address += "/" + LocalNumber;
            }
            address += "\r\n" + PostalCode + PostCity;
            return address;
        }
    }
}
