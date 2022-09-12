using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NarwianskiZakatek.Data;
using Npgsql;

namespace NarwianskiZakatek.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(serviceProvider.GetRequiredService<
                    DbContextOptions<ApplicationDbContext>>()))
            {
                try
                {
                    #region add roles
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
                    #endregion

                    #region add users
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
                            PhoneNumber = "123456789",
                            City = "Białystok",
                            Street = "Wiejska",
                            BuildingNumber = "45a",
                            PostalCode = "15-352",
                            PostCity = "Białystok"
                        });
                        context.SaveChanges();
                        string userId = context.Users.Where(r => r.UserName == "leszczynski_szymon@wp.pl").ToList().FirstOrDefault().Id;
                        context.UserRoles.Add(new IdentityUserRole<string>()
                        {
                            UserId = userId,
                            RoleId = context.Roles.Where(r => r.Name == "User").ToList().FirstOrDefault().Id
                        });
                        context.UserRoles.Add(new IdentityUserRole<string>()
                        {
                            UserId = userId,
                            RoleId = context.Roles.Where(r => r.Name == "Employee").ToList().FirstOrDefault().Id
                        });
                        context.UserRoles.Add(new IdentityUserRole<string>()
                        {
                            UserId = userId,
                            RoleId = context.Roles.Where(r => r.Name == "Admin").ToList().FirstOrDefault().Id
                        });
                        context.SaveChanges();

                        context.Users.Add(new AppUser()
                        {
                            UserName = "employee@test.pl",
                            NormalizedUserName = "EMPLOYEE@TEST.PL",
                            Email = "employee@test.pl",
                            NormalizedEmail = "EMPLOYEE@TEST.PL",
                            EmailConfirmed = true,
                            PasswordHash = "AQAAAAEAACcQAAAAEJwNo3gck8NrGarCI2t3yHCpEX2/wUppwQhQQBtI/IwNdKsCtzWkoaNJaGOR/MpxZg==",
                            SecurityStamp = "N4S2GQNOKBGA477TJKQB7A4KYCT4OCQ3",
                            ConcurrencyStamp = "ab642b36-c5e9-4c7a-b088-2015d606a8e0",
                            PhoneNumber = "123456789",
                            City = "Białystok",
                            Street = "Wiejska",
                            BuildingNumber = "45a",
                            PostalCode = "15-352",
                            PostCity = "Białystok"
                        });
                        context.SaveChanges();
                        userId = context.Users.Where(r => r.UserName == "employee@test.pl").ToList().FirstOrDefault().Id;
                        context.UserRoles.Add(new IdentityUserRole<string>()
                        {
                            UserId = userId,
                            RoleId = context.Roles.Where(r => r.Name == "User").ToList().FirstOrDefault().Id
                        });
                        context.UserRoles.Add(new IdentityUserRole<string>()
                        {
                            UserId = userId,
                            RoleId = context.Roles.Where(r => r.Name == "Employee").ToList().FirstOrDefault().Id
                        });
                        context.SaveChanges();

                        context.Users.Add(new AppUser()
                        {
                            UserName = "user@test.pl",
                            NormalizedUserName = "USER@TEST.PL",
                            Email = "user@test.pl",
                            NormalizedEmail = "USER@TEST.PL",
                            EmailConfirmed = true,
                            PasswordHash = "AQAAAAEAACcQAAAAELKBPXv8HNDhwzwO7eMO6WVzaKDAqnMvrPGcrMo11/wYuFO6ik4Xx31FdlrgKjm7yg==",
                            SecurityStamp = "2GW4WYDZN5BGMA2K4N5IJ24W5HGGZC6G",
                            ConcurrencyStamp = "3f8a113b-44b7-493d-be2e-c2542d1d007f",
                            PhoneNumber = "123456789",
                            City = "Białystok",
                            Street = "Wiejska",
                            BuildingNumber = "45a",
                            PostalCode = "15-352",
                            PostCity = "Białystok"
                        });
                        context.SaveChanges();
                        userId = context.Users.Where(r => r.UserName == "user@test.pl").ToList().FirstOrDefault().Id;
                        context.UserRoles.Add(new IdentityUserRole<string>()
                        {
                            UserId = userId,
                            RoleId = context.Roles.Where(r => r.Name == "User").ToList().FirstOrDefault().Id
                        });
                        context.SaveChanges();
                    }
                    #endregion

                    #region add descriptions
                    if (!context.Descriptions.Any())
                    {
                        context.Descriptions.Add(new Description()
                        {
                            Title = "Noclegi",
                            Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer vel iaculis dolor. Praesent congue sem ut arcu cursus imperdiet et quis ipsum. Donec lacinia neque quis diam convallis egestas. Aenean a ullamcorper nunc. Donec efficitur enim sit amet elit commodo, vitae consequat urna vehicula. Nunc faucibus nec leo eget pulvinar. Nulla fermentum urna vel massa efficitur ornare. Donec eget blandit elit. Donec auctor pellentesque tempus. Sed sapien neque, lacinia ac tincidunt quis, consequat et magna. Nullam vestibulum elit quis erat viverra pharetra. Praesent in scelerisque lectus, sed lobortis nulla."
                        });
                        context.Descriptions.Add(new Description()
                        {
                            Title = "Restauracja",
                            Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer vel iaculis dolor. Praesent congue sem ut arcu cursus imperdiet et quis ipsum. Donec lacinia neque quis diam convallis egestas. Aenean a ullamcorper nunc. Donec efficitur enim sit amet elit commodo, vitae consequat urna vehicula. Nunc faucibus nec leo eget pulvinar. Nulla fermentum urna vel massa efficitur ornare. Donec eget blandit elit. Donec auctor pellentesque tempus. Sed sapien neque, lacinia ac tincidunt quis, consequat et magna. Nullam vestibulum elit quis erat viverra pharetra. Praesent in scelerisque lectus, sed lobortis nulla."
                        });
                        context.Descriptions.Add(new Description()
                        {
                            Title = "Atrakcje",
                            Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer vel iaculis dolor. Praesent congue sem ut arcu cursus imperdiet et quis ipsum. Donec lacinia neque quis diam convallis egestas. Aenean a ullamcorper nunc. Donec efficitur enim sit amet elit commodo, vitae consequat urna vehicula. Nunc faucibus nec leo eget pulvinar. Nulla fermentum urna vel massa efficitur ornare. Donec eget blandit elit. Donec auctor pellentesque tempus. Sed sapien neque, lacinia ac tincidunt quis, consequat et magna. Nullam vestibulum elit quis erat viverra pharetra. Praesent in scelerisque lectus, sed lobortis nulla."
                        });
                        context.Descriptions.Add(new Description()
                        {
                            Title = "Okolica",
                            Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer vel iaculis dolor. Praesent congue sem ut arcu cursus imperdiet et quis ipsum. Donec lacinia neque quis diam convallis egestas. Aenean a ullamcorper nunc. Donec efficitur enim sit amet elit commodo, vitae consequat urna vehicula. Nunc faucibus nec leo eget pulvinar. Nulla fermentum urna vel massa efficitur ornare. Donec eget blandit elit. Donec auctor pellentesque tempus. Sed sapien neque, lacinia ac tincidunt quis, consequat et magna. Nullam vestibulum elit quis erat viverra pharetra. Praesent in scelerisque lectus, sed lobortis nulla."
                        });
                        context.Descriptions.Add(new Description()
                        {
                            Title = "O nas",
                            Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer vel iaculis dolor. Praesent congue sem ut arcu cursus imperdiet et quis ipsum. Donec lacinia neque quis diam convallis egestas. Aenean a ullamcorper nunc. Donec efficitur enim sit amet elit commodo, vitae consequat urna vehicula. Nunc faucibus nec leo eget pulvinar. Nulla fermentum urna vel massa efficitur ornare. Donec eget blandit elit. Donec auctor pellentesque tempus. Sed sapien neque, lacinia ac tincidunt quis, consequat et magna. Nullam vestibulum elit quis erat viverra pharetra. Praesent in scelerisque lectus, sed lobortis nulla."
                        });
                        
                        context.SaveChanges();
                    }
                    #endregion

                    #region add rooms
                    if (!context.Rooms.Any())
                    {
                        context.Rooms.Add(new Room()
                        {
                            RoomNumber = "010",
                            RoomCapacity = 3,
                            Price = 210
                        });
                        context.Rooms.Add(new Room()
                        {
                            RoomNumber = "011",
                            RoomCapacity = 2,
                            Price = 140
                        });
                        context.Rooms.Add(new Room()
                        {
                            RoomNumber = "012",
                            RoomCapacity = 2,
                            Price = 140
                        });
                        context.Rooms.Add(new Room()
                        {
                            RoomNumber = "110",
                            RoomCapacity = 5,
                            Price = 350
                        });
                        context.Rooms.Add(new Room()
                        {
                            RoomNumber = "111",
                            RoomCapacity = 1,
                            Price = 70
                        });
                        context.Rooms.Add(new Room()
                        {
                            RoomNumber = "112",
                            RoomCapacity = 1,
                            Price = 70
                        });
                        context.SaveChanges();
                    }
                    #endregion
                }
                catch (NpgsqlException ex)
                {
                    return;
                }
            }
        }
    }
}
