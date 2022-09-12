using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NarwianskiZakatek.Data;
using NarwianskiZakatek.Models;
using NarwianskiZakatek.ViewModels;

namespace NarwianskiZakatek.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReservationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles ="Admin,Employee")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public IActionResult Date()
        {
            return View();
        }
        [HttpGet]
        [Authorize(Roles = "User")]
        public IActionResult MyReservations(string? message)
        {
            var user = _context.Users.Where(u => u.UserName == HttpContext.User.Identity.Name).First();
            ViewBag.Message = message;
            return View(_context.Reservations.Where(r => r.UserId == user.Id));
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public IActionResult New(DateTime beginDate, DateTime endDate)
        {
            ViewBag.RoomList = FindAvailableRooms(beginDate, endDate);
            return View(new RoomsViewModel()
            {
                BeginDate = beginDate,
                EndDate = endDate
            });
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public IActionResult Confirm(RoomsViewModel roomList)
        {
            var user = _context.Users.Where(u => u.UserName == HttpContext.User.Identity.Name).First();
            Reservation reservation = new Reservation()
            {
                BeginDate = roomList.BeginDate,
                EndDate = roomList.EndDate,
                User = user
            };
            _context.Add(reservation);
            _context.SaveChanges();
            decimal price = 0;
            foreach(int roomId in roomList.Rooms)
            {
                _context.Add(new ReservedRoom()
                {
                    RoomId = roomId,
                    Reservation = reservation
                });
                price += _context.Rooms.Where(r => r.RoomId == roomId).First().Price;
            }
            reservation.Price = price * (roomList.EndDate.AddDays(1) - roomList.BeginDate).Days;
            _context.SaveChanges();
            return RedirectToAction("MyReservations", new { message = "Rezerwacja została złożona." });
        }

        private List<SelectListItem> FindAvailableRooms(DateTime beginDate, DateTime endDate)
        {
            var reservedRooms = GetIdsOfReservedRooms(beginDate, endDate);
            var rooms = _context.Rooms.Where(r => !reservedRooms.Contains(r.RoomId));
            var list = rooms.Select(r => new SelectListItem { Value = r.RoomId.ToString(), Text = r.ToString() }).ToList();
            return list;
        }

        private List<int> GetIdsOfReservedRooms(DateTime beginDate, DateTime endDate)
        {
            List<ReservedRoom> reservedRooms = _context.ReservedRooms
                .Where(r => (r.Reservation.BeginDate >= beginDate && r.Reservation.BeginDate <= endDate)
                         || (r.Reservation.EndDate >= beginDate && r.Reservation.BeginDate <= endDate)).ToList();
            return reservedRooms.Select(r => r.RoomId).Distinct().ToList();
        }
    }
}
