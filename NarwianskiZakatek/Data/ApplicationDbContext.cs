using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NarwianskiZakatek.Models;
using System.Reflection.Emit;
using System.Reflection.Metadata;

namespace NarwianskiZakatek.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public ApplicationDbContext() : base(new DbContextOptions<ApplicationDbContext>()) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<AppUser>().ToTable("Users");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            builder.Entity<IdentityRole>().ToTable("Roles");
        }

        public virtual DbSet<Description>? Descriptions { get; set; }
        public virtual DbSet<Post>? Posts { get; set; }
        public virtual DbSet<Reservation>? Reservations { get; set; }
        public virtual DbSet<ReservedRoom>? ReservedRooms { get; set; }
        public virtual DbSet<Room>? Rooms { get; set; }
        public virtual DbSet<Warning>? Warnings { get; set; }
    }
}