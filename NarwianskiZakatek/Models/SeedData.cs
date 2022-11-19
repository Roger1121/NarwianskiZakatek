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
            using (var context = new ApplicationDbContext(serviceProvider.
                GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                try
                {
                    #region add roles
                    if (context.Roles != null && !context.Roles.Any())
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
                    if (context.Users != null && !context.Users.Any())
                    {
                        context.Users.Add(new AppUser()
                        {
                            UserName = "narwianskizakatek@gmail.com",
                            NormalizedUserName = "NARWIANSKIZAKATEK@GMAIL.COM",
                            Email = "narwianskizakatek@gmail.com",
                            NormalizedEmail = "NARWIANSKIZAKATEK@GMAIL.COM",
                            EmailConfirmed = true,
                            PasswordHash = "AQAAAAEAACcQAAAAEDe9LsLy6xxzvfbsvrUzRSNUXRp0YwKuzvbAU02CnhAGhHgLOXMfaqShadKmkujA+A==",
                            SecurityStamp = "GFG64WLSCBWK22CVGPPDMNJA4GOA6RPB",
                            ConcurrencyStamp = "6d39cc48-1616-4c58-8001-c694eaeeb107",
                            PhoneNumber = "123456789",
                            City = "Białystok",
                            Street = "Wiejska",
                            BuildingNumber = "45a",
                            PostalCode = "15-352",
                            PostCity = "Białystok",
                            Name = "Jan",
                            Surname = "Kowalski"
                        });
                        context.SaveChanges();
                        string userId = context.Users.Where(r => r.UserName == "narwianskizakatek@gmail.com").ToList().First().Id;
                        context.UserRoles.Add(new IdentityUserRole<string>()
                        {
                            UserId = userId,
                            RoleId = context.Roles.Where(r => r.Name == "User").ToList().First().Id
                        });
                        context.UserRoles.Add(new IdentityUserRole<string>()
                        {
                            UserId = userId,
                            RoleId = context.Roles.Where(r => r.Name == "Employee").ToList().First().Id
                        });
                        context.UserRoles.Add(new IdentityUserRole<string>()
                        {
                            UserId = userId,
                            RoleId = context.Roles.Where(r => r.Name == "Admin").ToList().First().Id
                        });
                        context.SaveChanges();

                        context.Users.Add(new AppUser()
                        {
                            UserName = "nzemployee279@gmail.com",
                            NormalizedUserName = "NZEMPLOYEE279@GMAIL.COM",
                            Email = "nzemployee279@gmail.com",
                            NormalizedEmail = "NZEMPLOYEE279@GMAIL.COM",
                            EmailConfirmed = true,
                            PasswordHash = "AQAAAAEAACcQAAAAEJwNo3gck8NrGarCI2t3yHCpEX2/wUppwQhQQBtI/IwNdKsCtzWkoaNJaGOR/MpxZg==",
                            SecurityStamp = "N4S2GQNOKBGA477TJKQB7A4KYCT4OCQ3",
                            ConcurrencyStamp = "ab642b36-c5e9-4c7a-b088-2015d606a8e0",
                            PhoneNumber = "123456789",
                            City = "Białystok",
                            Street = "Wiejska",
                            BuildingNumber = "45a",
                            PostalCode = "15-352",
                            PostCity = "Białystok",
                            Name = "Jan",
                            Surname = "Kowalski"
                        });
                        context.SaveChanges();
                        userId = context.Users.Where(r => r.UserName == "nzemployee279@gmail.com").ToList().First().Id;
                        context.UserRoles.Add(new IdentityUserRole<string>()
                        {
                            UserId = userId,
                            RoleId = context.Roles.Where(r => r.Name == "User").ToList().First().Id
                        });
                        context.UserRoles.Add(new IdentityUserRole<string>()
                        {
                            UserId = userId,
                            RoleId = context.Roles.Where(r => r.Name == "Employee").ToList().First().Id
                        });
                        context.SaveChanges();

                        context.Users.Add(new AppUser()
                        {
                            UserName = "nzuser639@gmail.com",
                            NormalizedUserName = "NZUSER639@GMAIL.COM",
                            Email = "nzuser639@gmail.com",
                            NormalizedEmail = "NZUSER639@GMAIL.COM",
                            EmailConfirmed = true,
                            PasswordHash = "AQAAAAEAACcQAAAAELKBPXv8HNDhwzwO7eMO6WVzaKDAqnMvrPGcrMo11/wYuFO6ik4Xx31FdlrgKjm7yg==",
                            SecurityStamp = "2GW4WYDZN5BGMA2K4N5IJ24W5HGGZC6G",
                            ConcurrencyStamp = "3f8a113b-44b7-493d-be2e-c2542d1d007f",
                            PhoneNumber = "123456789",
                            City = "Białystok",
                            Street = "Wiejska",
                            BuildingNumber = "45a",
                            PostalCode = "15-352",
                            PostCity = "Białystok",
                            Name = "Jan",
                            Surname = "Kowalski"
                        });
                        context.SaveChanges();
                        userId = context.Users.Where(r => r.UserName == "nzuser639@gmail.com").ToList().First().Id;
                        context.UserRoles.Add(new IdentityUserRole<string>()
                        {
                            UserId = userId,
                            RoleId = context.Roles.Where(r => r.Name == "User").ToList().First().Id
                        });
                        context.SaveChanges();
                    }
                    #endregion

                    #region add descriptions
                    if (context.Descriptions != null && !context.Descriptions.Any())
                    {
                        context.Descriptions.Add(new Description()
                        {
                            Title = "Noclegi",
                            Content = "Gospodarstwo posiada pokoje o różnej wielkości, które pozwolą na komfortowy nocleg zarówno wieloosobowym rodzinom pragnącym poznac wiejskie życie, jak i singlom szukającym spokojnego odpoczynku i relaksu. Każdy pokój posiada własną łazienkę, a na każdym piętrze budynku znajduje się aneks kuchenny. Goście naszego gospodarstwa posiadają również dostęp do bezpłatnej sieci wi-fi i telewizji."
                        });
                        context.Descriptions.Add(new Description()
                        {
                            Title = "Restauracja",
                            Content = "Na terenie gospodarstwa funkcjonuje restauracja serwująca dania regionalne, zaopatrywana wyłącznie w produkty pochodzące z hodowli i upraw ekologicznych. Nasza restauracja oferuje również możliwość zapewnienia cateringu, obsługi imprez okolicznościowych i wynajmu lokalu. Nasi klienci mogą liczyć na wysoką jakość usług, atrakcyjne ceny i transparentność w kwestii pochodzenia produktów."
                        });
                        context.Descriptions.Add(new Description()
                        {
                            Title = "Atrakcje",
                            PhotoUrl = "2b9b80fc-d85f-47f0-a06f-465f0e080706.JPG",
                            Content = "Gospodarstwo zapewnia liczne atrakcje stałe oraz sezonowe. Dzięki współpracy z okolicznymi przedsiębiorstwami, szkołami oraz instytucjami publicznymi, jesteśmy w stanie zapewnić naszym gościom udział w wielu lokalnych przedsięwzięciach odbywających się poza granicami naszego gospodarstwa za darmo, lub po obniżonej cenie. Nasi goście mogą liczyć na atrakcyjne ceny na kursy jazdy konnej, fotografii, przeloty balonem, czy wycieczki po Narwiańskim Parku Narodowym. Ponadto aktywnie bierzemy udział w okolicznych imprezach takich, jak Dzień Ogórka w Kruszewie, Jarmark Dominikański w Choroszczy oraz Biesiada miodowa w Tykocinie. Relacja z wydarzeń zawsze jest na bieżąco umieszczana na stronie głównej gospodarstwa."
                        });
                        context.Descriptions.Add(new Description()
                        {
                            Title = "Okolica",
                            PhotoUrl = "9a2c3ed5-7b4e-4ba4-987c-0fb9e0fa5a6d.JPG",
                            Content = "Nasze gospodarstwo połopżone jest na uboczu, dzięki czemu nasi goście mogą liczyć na spokojny i pozbawiony nieproszonych gości pobyt. Jednocześnie w niewielkiej odległości od gospodarstwa znajdują się liczne, warte odwiedzenia miejsca, takie jak kładka przez Narwiański Park Narodowy ze Śliwna do Waniewa, zerwany most i wieża widokowa w Kruszewie czy trasa rowerowa z Pańk do Rzędzian. Bliskie sąsiedztwo lasów i rzeki pozwala naszym klientom na bliski kontakt z naturą, spacery, wycieczki rowerowe, wypady na ryby, grzyby, sesje fotograficzne i wiele innych."
                        });
                        context.Descriptions.Add(new Description()
                        {
                            Title = "O nas",
                            PhotoUrl = "d1bba959-f4e6-470d-a1fb-ce98527588d7.JPG",
                            Content = "Jesteśmy rodzinnym gospodarstwem agroturystycznym założonym jako rozwinięcie realnie działającego gospodarstwa rolnego. Naszym celem jest zachęcenie klientów do przebywania w bliskim kontakcie z naturą i wsparcie działalności lokalnych gospodarstw i przedsiębiorstw poprzed dystrybucję wyrobów regionalnych i ekologicznych oraz reklamę usług."
                        });

                        context.SaveChanges();
                    }
                    #endregion

                    #region add rooms
                    if (context.Rooms != null && !context.Rooms.Any())
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

                    #region add warnings
                    if (context.Warnings != null && !context.Warnings.Any())
                    {
                        AppUser user = context.Users.Where(u => u.UserName == "nzuser639@gmail.com").First();
                        context.Warnings.Add(new Warning()
                        {
                            Message = "Prosimy o zachowanie kultury i poszanowanie praw innych gości podczas pobytu w gospodarstwie. Kolejne skargi będą skutkowały blokadą konta i zakazem pobytu.",
                            User = user,
                            UserId = user.Id,
                            WasDisplayed = false
                        });
                        context.Warnings.Add(new Warning()
                        {
                            Message = "Prosimy o uregulowanie zaległej płatności za rezerwację z dnia 04.11.2022. Termin płatności minął 18.11.2022",
                            User = user,
                            UserId = user.Id,
                            WasDisplayed = false
                        });
                        context.SaveChanges();
                    }
                    #endregion

                    #region add reservations
                    if (context.Reservations != null && context.ReservedRooms != null && !context.Reservations.Any())
                    {
                        // 1
                        AppUser user = context.Users.Where(u => u.UserName == "nzuser639@gmail.com").First();
                        var rooms = context.Rooms.ToList().GetRange(0, 3);
                        decimal price = 0;
                        foreach (var room in rooms)
                        {
                            price += room.Price;
                        }
                        price *= 7;
                        Reservation reservation = new Reservation()
                        {
                            User = user,
                            UserId = user.Id,
                            BeginDate = new DateTime(2022, 11, 5),
                            EndDate = new DateTime(2022, 11, 12),
                            Price = price,
                            Opinion = "Beznadziejne miejsce, obsługa do bani, nigdy więcej tu nie przyjadę.",
                            Rating = 2
                        };
                        context.Reservations.Add(reservation);

                        foreach (var room in rooms)
                        {
                            context.ReservedRooms.Add(new ReservedRoom()
                            {
                                ReservationId = reservation.ReservationId,
                                RoomId = room.RoomId
                            });
                        }
                        context.SaveChanges();

                        // 2
                        rooms = context.Rooms.ToList().GetRange(4, 1);
                        price = 0;
                        foreach (var room in rooms)
                        {
                            price += room.Price;
                        }
                        price *= 7;
                        reservation = new Reservation()
                        {
                            User = user,
                            UserId = user.Id,
                            BeginDate = new DateTime(2022, 9, 1),
                            EndDate = new DateTime(2022, 9, 6),
                            Price = price,
                            Opinion = ""
                        };
                        context.Reservations.Add(reservation);

                        foreach (var room in rooms)
                        {
                            context.ReservedRooms.Add(new ReservedRoom()
                            {
                                ReservationId = reservation.ReservationId,
                                RoomId = room.RoomId
                            });
                        }
                        context.SaveChanges();

                        // 3
                        rooms = context.Rooms.ToList().GetRange(5, 1);
                        price = 0;
                        foreach (var room in rooms)
                        {
                            price += room.Price;
                        }
                        price *= 7;
                        reservation = new Reservation()
                        {
                            User = user,
                            UserId = user.Id,
                            BeginDate = new DateTime(2022, 6, 6),
                            EndDate = new DateTime(2022, 6, 13),
                            Price = price,
                            Opinion = ""
                        };
                        context.Reservations.Add(reservation);

                        foreach (var room in rooms)
                        {
                            context.ReservedRooms.Add(new ReservedRoom()
                            {
                                ReservationId = reservation.ReservationId,
                                RoomId = room.RoomId
                            });
                        }
                        context.SaveChanges();

                        // 4
                        rooms = context.Rooms.ToList().GetRange(2, 4);
                        price = 0;
                        foreach (var room in rooms)
                        {
                            price += room.Price;
                        }
                        price *= 7;
                        reservation = new Reservation()
                        {
                            User = user,
                            UserId = user.Id,
                            BeginDate = new DateTime(2022, 7, 8),
                            EndDate = new DateTime(2022, 7, 10),
                            Price = price,
                            Opinion = ""
                        };
                        context.Reservations.Add(reservation);

                        foreach (var room in rooms)
                        {
                            context.ReservedRooms.Add(new ReservedRoom()
                            {
                                ReservationId = reservation.ReservationId,
                                RoomId = room.RoomId
                            });
                        }
                        context.SaveChanges();

                        // 5
                        rooms = context.Rooms.ToList().GetRange(0, 3);
                        price = 0;
                        foreach (var room in rooms)
                        {
                            price += room.Price;
                        }
                        price *= 7;
                        reservation = new Reservation()
                        {
                            User = user,
                            UserId = user.Id,
                            BeginDate = new DateTime(2022, 8, 8),
                            EndDate = new DateTime(2022, 8, 14),
                            Price = price,
                            Opinion = ""
                        };
                        context.Reservations.Add(reservation);

                        foreach (var room in rooms)
                        {
                            context.ReservedRooms.Add(new ReservedRoom()
                            {
                                ReservationId = reservation.ReservationId,
                                RoomId = room.RoomId
                            });
                        }
                        context.SaveChanges();

                        // 6
                        rooms = context.Rooms.ToList().GetRange(0, 4);
                        price = 0;
                        foreach (var room in rooms)
                        {
                            price += room.Price;
                        }
                        price *= 7;
                        reservation = new Reservation()
                        {
                            User = user,
                            UserId = user.Id,
                            BeginDate = new DateTime(2022, 4, 1),
                            EndDate = new DateTime(2022, 4, 3),
                            Price = price,
                            Opinion = ""
                        };
                        context.Reservations.Add(reservation);

                        foreach (var room in rooms)
                        {
                            context.ReservedRooms.Add(new ReservedRoom()
                            {
                                ReservationId = reservation.ReservationId,
                                RoomId = room.RoomId
                            });
                        }
                        context.SaveChanges();

                        // 7
                        user = context.Users.Where(u => u.UserName == "nzemployee279@gmail.com").First();
                        rooms = context.Rooms.ToList().GetRange(3, 2);
                        price = 0;
                        foreach (var room in rooms)
                        {
                            price += room.Price;
                        }
                        price *= 10;

                        reservation = new Reservation()
                        {
                            User = user,
                            UserId = user.Id,
                            BeginDate = new DateTime(2022, 10, 7),
                            EndDate = new DateTime(2022, 10, 17),
                            Price = price,
                            Opinion = "Świetne miejsce, gospodarstwo zapewnia mnóstwo atrakcji i sposobów na spędzenie czasu,  i na pewno jeszcze tu wrócimy.",
                            Rating = 10
                        };
                        context.Reservations.Add(reservation);

                        foreach (var room in rooms)
                        {
                            context.ReservedRooms.Add(new ReservedRoom()
                            {
                                ReservationId = reservation.ReservationId,
                                RoomId = room.RoomId
                            });
                        }
                        context.SaveChanges();

                        // 8
                        rooms = context.Rooms.ToList().GetRange(4, 1);
                        price = 0;
                        foreach (var room in rooms)
                        {
                            price += room.Price;
                        }
                        price *= 7;
                        reservation = new Reservation()
                        {
                            User = user,
                            UserId = user.Id,
                            BeginDate = new DateTime(2022, 9, 7),
                            EndDate = new DateTime(2022, 9, 10),
                            Price = price,
                            Opinion = ""
                        };
                        context.Reservations.Add(reservation);

                        foreach (var room in rooms)
                        {
                            context.ReservedRooms.Add(new ReservedRoom()
                            {
                                ReservationId = reservation.ReservationId,
                                RoomId = room.RoomId
                            });
                        }
                        context.SaveChanges();

                        // 9
                        rooms = context.Rooms.ToList().GetRange(2, 1);
                        price = 0;
                        foreach (var room in rooms)
                        {
                            price += room.Price;
                        }
                        price *= 7;
                        reservation = new Reservation()
                        {
                            User = user,
                            UserId = user.Id,
                            BeginDate = new DateTime(2022, 6, 6),
                            EndDate = new DateTime(2022, 6, 13),
                            Price = price,
                            Opinion = ""
                        };
                        context.Reservations.Add(reservation);

                        foreach (var room in rooms)
                        {
                            context.ReservedRooms.Add(new ReservedRoom()
                            {
                                ReservationId = reservation.ReservationId,
                                RoomId = room.RoomId
                            });
                        }
                        context.SaveChanges();

                        // 10
                        rooms = context.Rooms.ToList().GetRange(0, 1);
                        price = 0;
                        foreach (var room in rooms)
                        {
                            price += room.Price;
                        }
                        price *= 7;
                        reservation = new Reservation()
                        {
                            User = user,
                            UserId = user.Id,
                            BeginDate = new DateTime(2022, 7, 8),
                            EndDate = new DateTime(2022, 7, 10),
                            Price = price,
                            Opinion = ""
                        };
                        context.Reservations.Add(reservation);

                        foreach (var room in rooms)
                        {
                            context.ReservedRooms.Add(new ReservedRoom()
                            {
                                ReservationId = reservation.ReservationId,
                                RoomId = room.RoomId
                            });
                        }
                        context.SaveChanges();

                        // 11
                        rooms = context.Rooms.ToList().GetRange(3, 3);
                        price = 0;
                        foreach (var room in rooms)
                        {
                            price += room.Price;
                        }
                        price *= 7;
                        reservation = new Reservation()
                        {
                            User = user,
                            UserId = user.Id,
                            BeginDate = new DateTime(2022, 8, 8),
                            EndDate = new DateTime(2022, 8, 14),
                            Price = price,
                            Opinion = ""
                        };
                        context.Reservations.Add(reservation);

                        foreach (var room in rooms)
                        {
                            context.ReservedRooms.Add(new ReservedRoom()
                            {
                                ReservationId = reservation.ReservationId,
                                RoomId = room.RoomId
                            });
                        }
                        context.SaveChanges();

                        // 12
                        rooms = context.Rooms.ToList().GetRange(2, 3);
                        price = 0;
                        foreach (var room in rooms)
                        {
                            price += room.Price;
                        }
                        price *= 7;
                        reservation = new Reservation()
                        {
                            User = user,
                            UserId = user.Id,
                            BeginDate = new DateTime(2022, 4, 8),
                            EndDate = new DateTime(2022, 4, 11),
                            Price = price,
                            Opinion = ""
                        };
                        context.Reservations.Add(reservation);

                        foreach (var room in rooms)
                        {
                            context.ReservedRooms.Add(new ReservedRoom()
                            {
                                ReservationId = reservation.ReservationId,
                                RoomId = room.RoomId
                            });
                        }
                        context.SaveChanges();
                    }
                    #endregion

                    #region add posts
                    if (context.Posts != null && !context.Posts.Any())
                    {
                        context.Posts.Add(new Post()
                        {
                            Title = "",
                            DateCreated = new DateTime(2022, 9, 9),
                            PhotoUrl = "",
                            Content = ""
                        });
                    }
                    #endregion
                }
                catch (NpgsqlException)
                {
                    return;
                }
            }
        }
    }
}
