using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NarwianskiZakatek.Models;

namespace NarwianskiZakatek.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Description>? Descriptions { get; set; }
        public DbSet<Post>? Posts { get; set; }
        public DbSet<Reservation>? Reservations { get; set; }
        public DbSet<ReservedRoom>? ReservedRooms { get; set; }
        public DbSet<Room>? Rooms { get; set; }
        public DbSet<Warning>? Warnings { get; set; }
    }
}