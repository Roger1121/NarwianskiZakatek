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
                while (!context.Database.CanConnect())
                {

                }
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
                            Surname = "Kowalski",
                            LockoutEnabled = true
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
                            Surname = "Kowalski",
                            LockoutEnabled = true
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
                            Surname = "Kowalski",
                            LockoutEnabled = true
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
                            Content = "Gospodarstwo posiada pokoje o różnej wielkości, które pozwolą na komfortowy nocleg zarówno wieloosobowym rodzinom" +
                            " pragnącym poznac wiejskie życie, jak i singlom szukającym spokojnego odpoczynku i relaksu. Każdy pokój posiada własną łazienkę," +
                            " a na każdym piętrze budynku znajduje się aneks kuchenny. Goście naszego gospodarstwa posiadają również dostęp do bezpłatnej sieci" +
                            " wi-fi i telewizji."
                        });
                        context.Descriptions.Add(new Description()
                        {
                            Title = "Restauracja",
                            Content = "Na terenie gospodarstwa funkcjonuje restauracja serwująca dania regionalne, zaopatrywana wyłącznie w produkty pochodzące" +
                            " z hodowli i upraw ekologicznych. Nasza restauracja oferuje również możliwość zapewnienia cateringu, obsługi imprez okolicznościowych" +
                            " i wynajmu lokalu. Nasi klienci mogą liczyć na wysoką jakość usług, atrakcyjne ceny i transparentność w kwestii pochodzenia produktów."
                        });
                        context.Descriptions.Add(new Description()
                        {
                            Title = "Atrakcje",
                            PhotoUrl = "2b9b80fc-d85f-47f0-a06f-465f0e080706.JPG",
                            Content = "Gospodarstwo zapewnia liczne atrakcje stałe oraz sezonowe. Dzięki współpracy z okolicznymi przedsiębiorstwami, szkołami oraz" +
                            " instytucjami publicznymi, jesteśmy w stanie zapewnić naszym gościom udział w wielu lokalnych przedsięwzięciach odbywających się poza" +
                            " granicami naszego gospodarstwa za darmo, lub po obniżonej cenie. Nasi goście mogą liczyć na atrakcyjne ceny na kursy jazdy konnej," +
                            " fotografii, przeloty balonem, czy wycieczki po Narwiańskim Parku Narodowym. Ponadto aktywnie bierzemy udział w okolicznych imprezach" +
                            " takich, jak Dzień Ogórka w Kruszewie, Jarmark Dominikański w Choroszczy oraz Biesiada miodowa w Tykocinie. Relacja z wydarzeń zawsze" +
                            " jest na bieżąco umieszczana na stronie głównej gospodarstwa."
                        });
                        context.Descriptions.Add(new Description()
                        {
                            Title = "Okolica",
                            PhotoUrl = "9a2c3ed5-7b4e-4ba4-987c-0fb9e0fa5a6d.JPG",
                            Content = "Nasze gospodarstwo połopżone jest na uboczu, dzięki czemu nasi goście mogą liczyć na spokojny i pozbawiony nieproszonych gości" +
                            " pobyt. Jednocześnie w niewielkiej odległości od gospodarstwa znajdują się liczne, warte odwiedzenia miejsca, takie jak kładka przez" +
                            " Narwiański Park Narodowy ze Śliwna do Waniewa, zerwany most i wieża widokowa w Kruszewie czy trasa rowerowa z Pańk do Rzędzian. Bliskie" +
                            " sąsiedztwo lasów i rzeki pozwala naszym klientom na bliski kontakt z naturą, spacery, wycieczki rowerowe, wypady na ryby, grzyby, sesje" +
                            " fotograficzne i wiele innych."
                        });
                        context.Descriptions.Add(new Description()
                        {
                            Title = "O nas",
                            PhotoUrl = "d1bba959-f4e6-470d-a1fb-ce98527588d7.JPG",
                            Content = "Jesteśmy rodzinnym gospodarstwem agroturystycznym założonym jako rozwinięcie realnie działającego gospodarstwa rolnego. Naszym" +
                            " celem jest zachęcenie klientów do przebywania w bliskim kontakcie z naturą i wsparcie działalności lokalnych gospodarstw i przedsiębiorstw" +
                            " poprzed dystrybucję wyrobów regionalnych i ekologicznych oraz reklamę usług. Niniejsza witryna internetowa powstała na Wydziale Informatyki" +
                            " Politechniki Białostockiej"
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
                            Price = 210,
                            IsActive = true
                        });
                        context.Rooms.Add(new Room()
                        {
                            RoomNumber = "011",
                            RoomCapacity = 2,
                            Price = 140,
                            IsActive = true
                        });
                        context.Rooms.Add(new Room()
                        {
                            RoomNumber = "012",
                            RoomCapacity = 2,
                            Price = 140,
                            IsActive = true
                        });
                        context.Rooms.Add(new Room()
                        {
                            RoomNumber = "110",
                            RoomCapacity = 5,
                            Price = 350,
                            IsActive = true
                        });
                        context.Rooms.Add(new Room()
                        {
                            RoomNumber = "111",
                            RoomCapacity = 1,
                            Price = 70,
                            IsActive = true
                        });
                        context.Rooms.Add(new Room()
                        {
                            RoomNumber = "112",
                            RoomCapacity = 1,
                            Price = 70,
                            IsActive = true
                        });
                        context.Rooms.Add(new Room()
                        {
                            RoomNumber = "210",
                            RoomCapacity = 1,
                            Price = 70,
                            IsActive = false
                        });
                        context.Rooms.Add(new Room()
                        {
                            RoomNumber = "211",
                            RoomCapacity = 1,
                            Price = 70,
                            IsActive = false
                        });
                        context.Rooms.Add(new Room()
                        {
                            RoomNumber = "212",
                            RoomCapacity = 1,
                            Price = 70,
                            IsActive = false
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
                            Message = "Prosimy o zachowanie kultury i poszanowanie praw innych gości podczas pobytu w gospodarstwie." +
                            " Kolejne skargi będą skutkowały blokadą konta i zakazem pobytu.",
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
                            BeginDate = new DateTime(2022, 12, 1),
                            EndDate = new DateTime(2022, 12, 6),
                            Price = price
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
                            Price = price
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
                            BeginDate = new DateTime(2022, 11, 19),
                            EndDate = new DateTime(2022, 11, 21),
                            Price = price
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
                            BeginDate = new DateTime(2022, 12, 8),
                            EndDate = new DateTime(2022, 12, 14),
                            Price = price
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
                            Price = price
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

                        //user 7-13
                        for (int i = 1; i < 7; i++)
                        {
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
                                BeginDate = new DateTime(2023, i, 1),
                                EndDate = new DateTime(2023, i, 3),
                                Price = price
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
                            Price = price
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
                            Price = price
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
                            Price = price
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
                            Price = price
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
                            Price = price
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
                            Title = "Nowa strona gospodarstwa",
                            DateCreated = new DateTime(2022, 8, 3),
                            PhotoUrl = "1fcc6a5a-3154-4c53-a962-da994e5672d3.JPG",
                            Content = "Pragniemy przedstawić wam naszą nową stronę internetową, która powstała w ramach pracy inżynierskiej" +
                            " we współpracy z Politechniką Białostocką. Korzystając z nowej witryny możecie dowiedzieć się więcej o naszym gospodarstwie," +
                            " zarezerwować pobyt, a nawet zostawić nam ocenę (do czego gorąco zachęcamy). Liczymy, że nowy serwis ułatwi wam kontakt z naszymi" +
                            " pracownikami i pozwoli na dalszy rozwój gospodarstwa. Jeżeli macie jakieś uwagi odnośnie działania strony, będziemy wdzięczni" +
                            " za kontakt i wszelkie propozycjwe usprawnień."
                        });
                        context.Posts.Add(new Post()
                        {
                            Title = "Prace remontowe",
                            DateCreated = new DateTime(2022, 8, 10),
                            PhotoUrl = "5808f117-4e78-49c2-8c9e-0e76026ce24b.JPG",
                            Content = "Witajcie. Jak zapewne niektórzy z gości mogli już zauważyć, w naszym gospodarstwie zaczynają się prace remontowe związane z wymianami" +
                            " dachów na budynkach gospodarczych. Z tego też powodu jesteśmy zmuszeni ograniczyć liczbę odwiedzających, za co bardzo serdecznie przepraszamy." +
                            " Liczymy, że te niedogodności nie potrwają długo i wkrótce będziemy mogli wrócić do normalnego trybu pracy. Wszystkim gościom, których pobyt" +
                            " będziemy zmuszeni anulować lub przesunąć (pomimo widniejących na stronie wolnych miejsc) postaramy się zrekompensować niedogodności poprzez" +
                            " obniżenie ceny lub rezerwację nocy w jednym z zaprzyjaźnionych gospodarstw agroturystycznych w okolicy. Jeszcze raz przepraszamy za utrudnienia."
                        });
                        context.Posts.Add(new Post()
                        {
                            Title = "Sejmiki bocianów",
                            DateCreated = new DateTime(2022, 8, 16),
                            PhotoUrl = "801b69a7-6b3e-4605-baf4-b4ce4d2d659f.JPG",
                            Content = "W tym roku w okolicach naszego gospodarstwa możemy obserwować przepiękne zebrania bocianów szykujących się do odlotu" +
                            " nazywane Bocianimi sejmikami. Zebrania takie mogą liczyc nawet ok. 50 osobników w różnym wieku. Mają one na celu wspólne przygotowanie" +
                            " do długiego, liczącego niejednokrotnie nawet 10000 km lotu. Bociany przed podróżą muszą uzupełnić zapasy i nabrać sił. Już niedługo przyjdzie" +
                            " nam pożegnać pierwsze, najstarsze bociany, które nie miały w tym roku młodych. Następnie dołączą do nich młode, urodzone w tym roku osobniki," +
                            " a jako ostatni odlecą ich rodzice. Cieszymy się, że w tym roku bociany wybrały sobie do przygotowań okolicę naszego gospodarstwa i życzymy im," +
                            " aby wszystkie dotarły bezpiecznie do celu, i wróciły do nas na wiosnę."
                        });
                        context.Posts.Add(new Post()
                        {
                            Title = "Once in a blue moon",
                            DateCreated = new DateTime(2022, 9, 25),
                            PhotoUrl = "2ca60500-7cb1-48b1-bade-62a54353d8a9.JPG",
                            Content = "Niebieska pełnia, czyli druga pełnia księżyca występująca w jednym miesiącu, lub trzecia z czterech występujących w czasie jednej pory roku." +
                            " Jest to zjawisko na tyle rzadkie, że w kulturze angielskiej doczekało się nawet swojego powiedzenia. Zwrot \"once in a blue moon\" (raz na niebieski księżyc" +
                            " - tłum. autora) oznacza wydarzenie, które odbywa się bardzo rzadko. Faktycznie, ostatnia niebieska pełnia miała miejsce 22 sierpnia, a na kolejną przyjdzie" +
                            " nam czekać ponad 2 lata, bo aż do 31 sierpnia 2023."
                        });
                        context.Posts.Add(new Post()
                        {
                            Title = "Nocne warsztaty fotograficzne",
                            DateCreated = new DateTime(2022, 9, 1),
                            PhotoUrl = "441f0284-4df8-4d6e-b8ac-38bb960d5673.jpg",
                            Content = "Pomimo niesprzyjającej ostatnio pogody, udało nam się, we współpracy z zaprzyjaźnionym fotografem, zrealizować nocne warsztaty fotograficzne." +
                            " Uczestnicy mieli okazję dowiedzieć się, jak działa obiektyw aparatu oraz poznać podstawowe zasady kompozycji kadru i doboru ekspozycji." +
                            " Następnie ich wiedza została przetestowana w ekstremalnych warunkach oświetleniowych, a efekty możecie ocenić sami. Zwieńczeniem warsztatów" +
                            " fotograficznych było wprowadzenie do obróbki zdjęć w programach takich, jak Gimp, czy Canon Digital Photo Professional. Dziękujemy wszystkim" +
                            " zainteresowanym za udział i liczymy, że podczas następnej edycji warsztatów będzie was jeszcze więcej."
                        });
                        context.Posts.Add(new Post()
                        {
                            Title = "Na grzyby...",
                            DateCreated = new DateTime(2022, 9, 9),
                            PhotoUrl = "69491762-d0a2-47ed-b920-864bd7bba243.jpg",
                            Content = "Przełom sierpnia i września, to doskonały czas, aby udać się do lasu w poszukiwaniu grzybów. Prawdziwki, podgrzybki czy kurki, są to grzyby," +
                            " które pospolicie występują w naszych lasach, a przyrządzane na setki różnych sposobów od lat wzbogacają smak potraw znajdujących się na naszych stołach." +
                            " Zupa kurkowa, kaszotto z borowikami czy kanie smażone w bułce tartej to tylko pojedyncze przykłady dań zawierających te \"małe leśne skurczybyki\"." +
                            " Ze względu na trwający już sezon, potrawy grzybowe powracają również do menu naszej restauracji i będą tam figurowały przez najblizsze tygodnie." +
                            " Zapraszamy do zapoznania się ze zaktualizowaną ofertą restauracji i życzymy smacznego."
                        });
                        context.Posts.Add(new Post()
                        {
                            Title = "Z archiwum NZ",
                            DateCreated = new DateTime(2022, 9, 18),
                            PhotoUrl = "e3c1b9a5-f6db-46c0-8d03-0b5487f3f089.jpg",
                            Content = "Często zdarza się, że wydarzeń w naszym gospodarstwie jest tak dużo, że nie jesteśmy w stanie opowiedzieć wam o wszystkich. Część postów trafia wtedy" +
                            " do naszego lokalnego archiwum, dzięki czemu możemy dzielić się nimi z wami w czasie, gdy dzieje się mniej. Jako że ostatnimi czasy pogoda wyjątkowo nie sprzyja" +
                            " spędzaniu czasu na dworze, zapraszamy do lektury posta od Narwiańskiego Zakątka. Dzisiaj, tak bardziej historycznie, przyjrzymy się trochę bliżej kościołowi" +
                            " farnemu w Białymstoku... a konkretnie XX-wiecznej dobudówce do XVII-wiecznego kościoła farnego. W ramach polityki rusyfikacji ludności polskiej nie pozwalano na" +
                            " budowę nowej świątyni. Wierni dostali jednak od władzy carskiej zgodę na rozbudowę istniejącego kościoła ufundowanego przez Wiesiołowskich. Przybudówka, powstająca" +
                            " w miejscu prezbiterium Starego Kościoła, rozrosła się jednak do tego stopnia, że de facto zaczęto ją wykorzystywać jako pełnoprawny kościół. W ten sposób powstał" +
                            " budynek, który znamy dzisiaj jako bazylikę archikatedralną Wniebowzięcia Najświętszej Maryi Panny w Białymstoku."
                        });
                        context.Posts.Add(new Post()
                        {
                            Title = "Uwaga! Łosie",
                            DateCreated = new DateTime(2022, 9, 27),
                            PhotoUrl = "b8109a75-693d-4ae6-bfe0-c6ead1af5911.jpg",
                            Content = "Moi drodzy. W związku z trwającym sezonem jesienno-zimowym apelujemy do was o ostrożność na drogach. Coraz częściej docierają do nas informacje" +
                            " o spotykanych przez was dzikich zwierzętach, w szczególności łosiach. Przypominamy, że zderzenie samochodu z łosiem może skończyć się bardzo tragicznie" +
                            " zarówno dla kierowcy samochodu, jak i jego pasażerów. Dodatkowo gęste mgły często ograniczają widoczność i znacznie skracają czas na reakcję po dostrzeżeniu" +
                            " zwierzęcia. Kierowco, jeżeli widzisz znak ostrzegawczy, zdejmij nogę z gazu, on nie stoi tam bez powodu. Bardzo cieszymy się z liczby odwiedzających nas gości," +
                            " ale bardzo zależy nam, abyście wszyscy dotarli do nas bezpiecznie. Trzymajcie się ciepło i do zobaczenia."
                        });
                        context.Posts.Add(new Post()
                        {
                            Title = "Historycznie - parafia w Choroszczy",
                            DateCreated = new DateTime(2022, 10, 12),
                            PhotoUrl = "ea230ca0-f403-414d-8563-d691c6692548.jpg",
                            Content = "Dzisiaj ponownie mamy dla was posta z serii o lokalnej historii, jednak tym razem będzie on dotyczył parafii w Choroszczy. Pierwsze wzmianki o istniejącej" +
                            " w Choroszczy parafii rzymskokatolickiej z drewnianym kościołem pochodzą z 22 października 1459 roku. W roku 1507 król Zygmunt Stary nadaje Choroszczy prawa miejskie." +
                            " W 1654 roku wojewoda trocki, Mikołaj Stefan Pac, herbu Gozdawa funduje w Choroszczy drewniany klasztor, przy kościele zbudowanym przez jego rodziców Stefana i" +
                            " Marcjannę Paców i powierza prowadzenie parafii ojcom dominikanom. Po dwóch pożarach w 1683 i 1707, hetman wielki koronny, kasztelan krakowski Jan Klemens Branicki" +
                            " w 1756 roku buduje tu murowany klasztor i kościół, konsekrowany w 1770 roku. W 1832 roku dominikanie zostali przez rząd carski wysiedleni do Różanegostoku, a parafia" +
                            " przeszła pod zarząd księży diecezjalnych. W 1915 roku podczas działań wojennych kościół został ponownie uszkodzony, a w 1938 roku z nieustalonych przyczyn spłonęło" +
                            " prawie całe wnętrze kościoła i dach. W roku 1944 wojska niemieckie wysadziły wieżę, która zniszczyła sklepienie świątyni. Kościół odbudowano w latach 1945 - 1947" +
                            " i w takiej formie przetrwał on już do chwili obecnej."
                        });
                        context.Posts.Add(new Post()
                        {
                            Title = "Historycznie - kościół Św. Jadwigi Królowej",
                            DateCreated = new DateTime(2022, 10, 22),
                            PhotoUrl = "28c0ce45-aa9e-4286-8bcf-6ca69d6259c6.jpg",
                            Content = "Dzisiaj znowu wracamy do serii postów historycznych i tak... znowu będziemy opowiadać historię pewnego kościoła - kościoła Św. Jadwigi Królowej. Jego budowa" +
                            " została zlecona w kwietniu 1983 roku. Początkowo planowano nadanie mu tytułu Św. Maksymiliana Marii Kolbego, jednak w maju 1983 Kuria przesłała informację, iż" +
                            " pierwszeństwo w otrzymaniu tego tytułu ma budujący się już kościół na Pietraszach. Ostatecznie kościołowi i parafii w osiedlu Słoneczny Stok nadano tytuł bł. Jadwigi" +
                            " Królowej. \nKościół posiada dwie oddzielone od siebie i niezależne części - górną i dolną.Dolna część została oddana do użytku przed ukończeniem całej budowy, aby można" +
                            " w niej było odprawiać nabożeństwa."
                        });
                        context.Posts.Add(new Post()
                        {
                            Title = "Warsztaty florystyczne",
                            DateCreated = new DateTime(2022, 10, 30),
                            PhotoUrl = "5e93e30b-1c15-4156-b319-0294d5d30cc5.jpg",
                            Content = "Dzięki uprzejmości znajomej kwiaciarki nasze gospodarstwo agroturystyczne miało przyjemność organizować w ostatnim czasie warsztaty florystyczne. Szczególne" +
                            " zainteresowanie wzbudziły one wśród pań, które miały okazję zapoznac się ze sposobami kompozycji bukietów i wiązanek, a dla niektórych okazały się prawdziwą lekcją" +
                            " cierpliwości i pokory. Wbrew pozorom, wiązanie bukietu wcale nie należy do prostych czynności i wymaga niezwykłej precyzji, staranności i zaangażowania. Szczególnie" +
                            " panowie biorący udział w warsztatach na pewno już nigdy nie spojrzą w ten sam sposób na bukiet zamawiany w kwiaciarni dla ukochanej."
                        });
                        context.SaveChanges();
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
