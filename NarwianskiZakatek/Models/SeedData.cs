using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NarwianskiZakatek.Data;

namespace NarwianskiZakatek.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(serviceProvider.GetRequiredService<
                    DbContextOptions<ApplicationDbContext>>()))
            {
                if (!context.Roles.Any())
                {
                    context.Roles.Add(new IdentityRole()
                    {
                        Name = "User",
                        NormalizedName = "USER"
                    });
                    context.Roles.Add(new IdentityRole()
                    {
                        Name = "Employee",
                        NormalizedName = "EMPLOYEE"
                    });
                    context.Roles.Add(new IdentityRole()
                    {
                        Name = "Admin",
                        NormalizedName = "ADMIN"
                    });
                    context.SaveChanges();
                }
                if (!context.Users.Any())
                {
                    context.Users.Add(new AppUser()
                    {
                        UserName = "leszczynski_szymon@wp.pl",
                        NormalizedUserName = "LESZCZYNSKI_SZYMON@WP.PL",
                        Email = "leszczynski_szymon@wp.pl",
                        NormalizedEmail = "LESZCZYNSKI_SZYMON@WP.PL",
                        EmailConfirmed = true,
                        PasswordHash = "AQAAAAEAACcQAAAAEDe9LsLy6xxzvfbsvrUzRSNUXRp0YwKuzvbAU02CnhAGhHgLOXMfaqShadKmkujA+A==",
                        SecurityStamp = "GFG64WLSCBWK22CVGPPDMNJA4GOA6RPB",
                        ConcurrencyStamp = "6d39cc48-1616-4c58-8001-c694eaeeb107",
                        PhoneNumber = "123456789"
                    });
                    context.SaveChanges();
                    string userId = context.Users.Where(r => r.UserName == "leszczynski_szymon@wp.pl").ToList().FirstOrDefault().Id;
                    context.UserRoles.Add( new IdentityUserRole<string>()
                    {
                        UserId = userId,
                        RoleId = context.Roles.Where(r => r.Name == "User").ToList().FirstOrDefault().Id
                    });context.UserRoles.Add( new IdentityUserRole<string>()
                    {
                        UserId = userId,
                        RoleId = context.Roles.Where(r => r.Name == "Employee").ToList().FirstOrDefault().Id
                    });context.UserRoles.Add( new IdentityUserRole<string>()
                    {
                        UserId = userId,
                        RoleId = context.Roles.Where(r => r.Name == "Admin").ToList().FirstOrDefault().Id
                    });
                    context.SaveChanges();
                }
            }
        }
    }
}
