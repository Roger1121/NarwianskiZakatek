using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using NarwianskiZakatek.Data;
using NarwianskiZakatek.Models;

namespace NarwianskiZakatekUnitTests
{
    public class MockData
    {
        public Mock<ApplicationDbContext> Context;
        public List<Post> posts;
        public List<Description> descriptions;
        public List<AppUser> appUsers;
        public List<IdentityRole> roles;
        public List<IdentityUserRole<string>> userRoles;
        public List<Warning> warnings;
        public List<Reservation> reservations;
        public List<Room> rooms;
        public List<ReservedRoom> reservedRooms;

        public MockData()
        {
            descriptions = new List<Description>()
            {
                 new Description { Content = "Test description 1", DescriptionId = 1 , Title = "Test description 1"},
                new Description { Content = "Test description 2", DescriptionId = 2 , Title = "Test description 2"},
                new Description { Content = "Test description 3", DescriptionId = 3 , Title = "Test description 3"}
            };
            appUsers = new List<AppUser>()
            {
                new AppUser { UserName = "Admin@email.com", Email = "Admin@email.com", NormalizedEmail = "ADMIN@EMAIL.COM", NormalizedUserName = "ADMIN@EMAIL.COM", Id = "1", IsLocked = false, PhoneNumber = "111222333"},
                new AppUser { UserName = "Employee@email.com", Email = "Employee@email.com", NormalizedEmail = "EMPLOYEE@EMAIL.COM", NormalizedUserName = "EMPLOYEE@EMAIL.COM", Id = "2", IsLocked = false, PhoneNumber = "123123123"},
                new AppUser { UserName = "User1@email.com", Email = "User1@email.com", NormalizedEmail = "USER1@EMAIL.COM", NormalizedUserName = "USER1@EMAIL.COM", Id = "3", IsLocked = false, PhoneNumber = "321321321"},
                new AppUser { UserName = "User2@email.com", Email = "User2@email.com", NormalizedEmail = "USER2@EMAIL.COM", NormalizedUserName = "USER2@EMAIL.COM", Id = "4", IsLocked = true, PhoneNumber = "111222333"}
            };
            roles = new List<IdentityRole>()
            {
                new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN"},
                new IdentityRole { Id = "2", Name = "Employee", NormalizedName = "EMPLOYEE"},
                new IdentityRole { Id = "3", Name = "User", NormalizedName = "USER"}
            };
            userRoles = new List<IdentityUserRole<string>>()
            {
                new IdentityUserRole<string> { RoleId = "1", UserId = "1"},
                new IdentityUserRole<string> { RoleId = "2", UserId = "2"},
                new IdentityUserRole<string> { RoleId = "3", UserId = "3"},
                new IdentityUserRole<string> { RoleId = "3", UserId = "4"},
            };
            reservations = new List<Reservation>()
            {
                new Reservation { ReservationId = "1", UserId = "2", BeginDate = DateTime.Now.AddDays(-5), EndDate = DateTime.Now.AddDays(-3), Price = 1100, Opinion = "Test",  Rating = 2},
                new Reservation { ReservationId = "2", UserId = "2", BeginDate = DateTime.Now.AddDays(5), EndDate = DateTime.Now.AddDays(8), Price = 2100},
                new Reservation { ReservationId = "3", UserId = "2", BeginDate = DateTime.Now.AddDays(-1), EndDate = DateTime.Now.AddDays(2), Price = 200},
                new Reservation { ReservationId = "4", UserId = "3", BeginDate = DateTime.Now.AddDays(-2), EndDate = DateTime.Now.AddDays(1), Price = 200},
                new Reservation { ReservationId = "5", UserId = "3", BeginDate = DateTime.Now.AddDays(-5), EndDate = DateTime.Now.AddDays(-3), Price = 80, IsCancelled = true},
                new Reservation { ReservationId = "6", UserId = "3", BeginDate = DateTime.Now.AddDays(2), EndDate = DateTime.Now.AddDays(4), Price = 150, IsCancelled = true},
                new Reservation { ReservationId = "7", UserId = "4", BeginDate = DateTime.Now.AddDays(-2), EndDate = DateTime.Now.AddDays(-1), Price = 300, Opinion = "Test",  Rating = 2},
                new Reservation { ReservationId = "8", UserId = "4", BeginDate = DateTime.Now.AddDays(-4), EndDate = DateTime.Now.AddDays(-2), Price = 400}
            };
            rooms = new List<Room>()
            {
                new Room { IsActive = true, Price = 100, RoomCapacity = 3, RoomNumber = "010", RoomId = 1},
                new Room { IsActive = true, Price = 100, RoomCapacity = 3, RoomNumber = "011", RoomId = 2},
                new Room { IsActive = true, Price = 100, RoomCapacity = 3, RoomNumber = "012", RoomId = 3},
                new Room { IsActive = true, Price = 100, RoomCapacity = 3, RoomNumber = "110", RoomId = 4},
                new Room { IsActive = true, Price = 100, RoomCapacity = 3, RoomNumber = "111", RoomId = 5},
                new Room { IsActive = true, Price = 100, RoomCapacity = 3, RoomNumber = "112", RoomId = 6},
                new Room { IsActive = true, Price = 100, RoomCapacity = 3, RoomNumber = "210", RoomId = 7},
                new Room { IsActive = true, Price = 100, RoomCapacity = 3, RoomNumber = "211", RoomId = 8},
                new Room { IsActive = false, Price = 100, RoomCapacity = 3, RoomNumber = "212", RoomId = 9},
            };
            reservedRooms = new List<ReservedRoom>()
            {
                new ReservedRoom { ReservationId = "1", RoomId = 1},
                new ReservedRoom { ReservationId = "1", RoomId = 2},
                new ReservedRoom { ReservationId = "2", RoomId = 3},
                new ReservedRoom { ReservationId = "2", RoomId = 4},
                new ReservedRoom { ReservationId = "3", RoomId = 1},
                new ReservedRoom { ReservationId = "4", RoomId = 5},
                new ReservedRoom { ReservationId = "5", RoomId = 6},
                new ReservedRoom { ReservationId = "6", RoomId = 7},
                new ReservedRoom { ReservationId = "7", RoomId = 8},
                new ReservedRoom { ReservationId = "8", RoomId = 9}
            };
            warnings = new List<Warning>()
            {
                new Warning { Message = "XXX", UserId = "2", WasDisplayed = true},
                new Warning { Message = "XYZ", UserId = "2", WasDisplayed = false},
                new Warning { Message = "ABC", UserId = "3", WasDisplayed = false},
                new Warning { Message = "BCA", UserId = "4", WasDisplayed = false},
                new Warning { Message = "CCB", UserId = "4", WasDisplayed = true}
            };
            posts = new List<Post>()
            {
                new Post { Content = "Post 1", Title = "Post 1", DateCreated = DateTime.Now.AddDays(-12), PostId = 1},
                new Post { Content = "Post 2", Title = "Post 2", DateCreated = DateTime.Now.AddDays(-11), PostId = 2},
                new Post { Content = "Post 3", Title = "Post 3", DateCreated = DateTime.Now.AddDays(-10), PostId = 3},
                new Post { Content = "Post 4", Title = "Post 4", DateCreated = DateTime.Now.AddDays(-9), PostId = 4},
                new Post { Content = "Post 5", Title = "Post 5", DateCreated = DateTime.Now.AddDays(-8), PostId = 5},
                new Post { Content = "Post 6", Title = "Post 6", DateCreated = DateTime.Now.AddDays(-7), PostId = 6},
                new Post { Content = "Post 7", Title = "Post 7", DateCreated = DateTime.Now.AddDays(-6), PostId = 7},
                new Post { Content = "Post 8", Title = "Post 8", DateCreated = DateTime.Now.AddDays(-5), PostId = 8},
                new Post { Content = "Post 9", Title = "Post 9", DateCreated = DateTime.Now.AddDays(-4), PostId = 9},
                new Post { Content = "Post 10", Title = "Post 10", DateCreated = DateTime.Now.AddDays(-3), PostId = 10},
                new Post { Content = "Post 11", Title = "Post 11", DateCreated = DateTime.Now.AddDays(-2), PostId = 11}
            };

            var mockDescriptions = new Mock<DbSet<Description>>();
            mockDescriptions.As<IQueryable<Description>>().Setup(m => m.Provider).Returns(descriptions.AsQueryable().Provider);
            mockDescriptions.As<IQueryable<Description>>().Setup(m => m.Expression).Returns(descriptions.AsQueryable().Expression);
            mockDescriptions.As<IQueryable<Description>>().Setup(m => m.ElementType).Returns(descriptions.AsQueryable().ElementType);
            mockDescriptions.As<IQueryable<Description>>().Setup(m => m.GetEnumerator()).Returns(descriptions.AsQueryable().GetEnumerator());

            var mockUsers = new Mock<DbSet<AppUser>>();
            mockUsers.As<IQueryable<AppUser>>().Setup(m => m.Provider).Returns(appUsers.AsQueryable().Provider);
            mockUsers.As<IQueryable<AppUser>>().Setup(m => m.Expression).Returns(appUsers.AsQueryable().Expression);
            mockUsers.As<IQueryable<AppUser>>().Setup(m => m.ElementType).Returns(appUsers.AsQueryable().ElementType);
            mockUsers.As<IQueryable<AppUser>>().Setup(m => m.GetEnumerator()).Returns(appUsers.AsQueryable().GetEnumerator());

            var mockRoles = new Mock<DbSet<IdentityRole>>();
            mockRoles.As<IQueryable<IdentityRole>>().Setup(m => m.Provider).Returns(roles.AsQueryable().Provider);
            mockRoles.As<IQueryable<IdentityRole>>().Setup(m => m.Expression).Returns(roles.AsQueryable().Expression);
            mockRoles.As<IQueryable<IdentityRole>>().Setup(m => m.ElementType).Returns(roles.AsQueryable().ElementType);
            mockRoles.As<IQueryable<IdentityRole>>().Setup(m => m.GetEnumerator()).Returns(roles.AsQueryable().GetEnumerator());

            var mockUserRoles = new Mock<DbSet<IdentityUserRole<string>>>();
            mockUserRoles.As<IQueryable<IdentityUserRole<string>>>().Setup(m => m.Provider).Returns(userRoles.AsQueryable().Provider);
            mockUserRoles.As<IQueryable<IdentityUserRole<string>>>().Setup(m => m.Expression).Returns(userRoles.AsQueryable().Expression);
            mockUserRoles.As<IQueryable<IdentityUserRole<string>>>().Setup(m => m.ElementType).Returns(userRoles.AsQueryable().ElementType);
            mockUserRoles.As<IQueryable<IdentityUserRole<string>>>().Setup(m => m.GetEnumerator()).Returns(userRoles.AsQueryable().GetEnumerator());

            var mockPosts = new Mock<DbSet<Post>>();
            mockPosts.As<IQueryable<Post>>().Setup(m => m.Provider).Returns(posts.AsQueryable().Provider);
            mockPosts.As<IQueryable<Post>>().Setup(m => m.Expression).Returns(posts.AsQueryable().Expression);
            mockPosts.As<IQueryable<Post>>().Setup(m => m.ElementType).Returns(posts.AsQueryable().ElementType);
            mockPosts.As<IQueryable<Post>>().Setup(m => m.GetEnumerator()).Returns(posts.AsQueryable().GetEnumerator());

            var mockReservations = new Mock<DbSet<Reservation>>();
            mockReservations.As<IQueryable<Reservation>>().Setup(m => m.Provider).Returns(reservations.AsQueryable().Provider);
            mockReservations.As<IQueryable<Reservation>>().Setup(m => m.Expression).Returns(reservations.AsQueryable().Expression);
            mockReservations.As<IQueryable<Reservation>>().Setup(m => m.ElementType).Returns(reservations.AsQueryable().ElementType);
            mockReservations.As<IQueryable<Reservation>>().Setup(m => m.GetEnumerator()).Returns(reservations.AsQueryable().GetEnumerator());

            var mockReservedRooms = new Mock<DbSet<ReservedRoom>>();
            mockReservedRooms.As<IQueryable<ReservedRoom>>().Setup(m => m.Provider).Returns(reservedRooms.AsQueryable().Provider);
            mockReservedRooms.As<IQueryable<ReservedRoom>>().Setup(m => m.Expression).Returns(reservedRooms.AsQueryable().Expression);
            mockReservedRooms.As<IQueryable<ReservedRoom>>().Setup(m => m.ElementType).Returns(reservedRooms.AsQueryable().ElementType);
            mockReservedRooms.As<IQueryable<ReservedRoom>>().Setup(m => m.GetEnumerator()).Returns(reservedRooms.AsQueryable().GetEnumerator());

            var mockRooms = new Mock<DbSet<Room>>();
            mockRooms.As<IQueryable<Room>>().Setup(m => m.Provider).Returns(rooms.AsQueryable().Provider);
            mockRooms.As<IQueryable<Room>>().Setup(m => m.Expression).Returns(rooms.AsQueryable().Expression);
            mockRooms.As<IQueryable<Room>>().Setup(m => m.ElementType).Returns(rooms.AsQueryable().ElementType);
            mockRooms.As<IQueryable<Room>>().Setup(m => m.GetEnumerator()).Returns(rooms.AsQueryable().GetEnumerator());

            var mockWarnings = new Mock<DbSet<Warning>>();
            mockWarnings.As<IQueryable<Warning>>().Setup(m => m.Provider).Returns(warnings.AsQueryable().Provider);
            mockWarnings.As<IQueryable<Warning>>().Setup(m => m.Expression).Returns(warnings.AsQueryable().Expression);
            mockWarnings.As<IQueryable<Warning>>().Setup(m => m.ElementType).Returns(warnings.AsQueryable().ElementType);
            mockWarnings.As<IQueryable<Warning>>().Setup(m => m.GetEnumerator()).Returns(warnings.AsQueryable().GetEnumerator());

            var mockContext = new Mock<ApplicationDbContext>();
            mockContext.SetupGet(c => c.Descriptions).Returns(mockDescriptions.Object);
            mockContext.SetupGet(c => c.Posts).Returns(mockPosts.Object);
            mockContext.SetupGet(c => c.Users).Returns(mockUsers.Object);
            mockContext.SetupGet(c => c.Roles).Returns(mockRoles.Object);
            mockContext.SetupGet(c => c.UserRoles).Returns(mockUserRoles.Object);
            mockContext.SetupGet(c => c.Reservations).Returns(mockReservations.Object);
            mockContext.SetupGet(c => c.ReservedRooms).Returns(mockReservedRooms.Object);
            mockContext.SetupGet(c => c.Rooms).Returns(mockRooms.Object);
            mockContext.SetupGet(c => c.Warnings).Returns(mockWarnings.Object);
            Context = mockContext;
        }
    }
}
