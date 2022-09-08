using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NarwianskiZakatek.Data;

namespace NarwianskiZakatek.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using(var context = new ApplicationDbContext(serviceProvider.GetRequiredService<
                    DbContextOptions<ApplicationDbContext>>()))
            {
                if (!context.Roles.Any())
                {
                    context.Roles.Add(new IdentityRole("User"));
                    context.Roles.Add(new IdentityRole("Employee"));
                    context.Roles.Add(new IdentityRole("Admin"));
                    context.SaveChanges();
                }
            }
        }
    }
}
