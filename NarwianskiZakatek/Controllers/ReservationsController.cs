using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NarwianskiZakatek.Data;
using NarwianskiZakatek.Models;
using NarwianskiZakatek.Services;
using NarwianskiZakatek.ViewModels;
using NuGet.Protocol.Plugins;

namespace NarwianskiZakatek.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _sender;

        public ReservationsController(ApplicationDbContext context, IEmailService sender)
        {
            _context = context;
            _sender = sender;
        }

        [Authorize(Roles = "Admin,Employee")]
        public IActionResult Index()
        {
            return View(_context.Reservations.ToList());
        }

        [Authorize(Roles = "Admin,Employee")]
        public IActionResult DetailsFull(int id)
        {
            Reservation reservation = _context.Reservations.Where(m => m.ReservationId == id).Include(r => r.ReservedRooms).Include(r => r.User).First();
            List<int> reservedRooms = reservation.ReservedRooms.Select(r => r.RoomId).Distinct().ToList();
            ViewBag.Rooms = _context.Rooms.Where(r => reservedRooms.Contains(r.RoomId)).ToList();
            return View(reservation);
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
        public async Task<IActionResult> ConfirmAsync(RoomsViewModel roomList)
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
            foreach (int roomId in roomList.Rooms)
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
            _sender.ConfirmReservationAsync(user.Email, reservation);
            return RedirectToAction("MyReservations", new { message = "Rezerwacja została złożona." });
        }

        [Authorize(Roles = "User")]
        public IActionResult Delete(int? id)
        {
            if (id == null || _context.Reservations == null)
            {
                return NotFound();
            }

            var reservation = _context.Reservations.FirstOrDefault(m => m.ReservationId == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            if (_context.Reservations == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Reservations'  is null.");
            }
            var reservation = _context.Reservations.Find(id);
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
                var user = _context.Users.Where(u => u.UserName == HttpContext.User.Identity.Name).First();
                _sender.CancelReservationAsync(user.Email, reservation);
            }
            _context.SaveChanges();
            return RedirectToAction("MyReservations", new { message = "Rezerwacja została anulowana." });
        }
        [Authorize(Roles = "Admin,Employee")]
        public IActionResult DeleteAny(int? id)
        {
            if (id == null || _context.Reservations == null)
            {
                return NotFound();
            }

            var reservation = _context.Reservations.FirstOrDefault(m => m.ReservationId == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Employee")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteAny(int id)
        {
            if (_context.Reservations == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Reservations'  is null.");
            }
            var reservation = _context.Reservations.Find(id);
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
            }

            _context.SaveChanges();
            return RedirectToAction("Index", new { message = "Rezerwacja została anulowana." });
        }

        [Authorize(Roles = "User")]
        public IActionResult Rate(int reservationId)
        {
            return View(new OpinionViewModel()
            {
                ReservationId = reservationId
            });
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public IActionResult Rate(OpinionViewModel opinion)
        {
            //save opinion
            var reservation = _context.Reservations.Where(r => r.ReservationId == opinion.ReservationId).First();
            reservation.Opinion = opinion.Opinion;
            reservation.Rating = opinion.Rating;
            _context.SaveChanges();
            return RedirectToAction("MyReservations", new { message = "Dziękujemy za udzielenie opinii." });
        }

        [Authorize(Roles = "User")]
        public IActionResult Details(int id)
        {
            Reservation reservation = _context.Reservations.Where(m => m.ReservationId == id).Include(r => r.ReservedRooms).First();
            List<int> reservedRooms = reservation.ReservedRooms.Select(r => r.RoomId).Distinct().ToList();
            ViewBag.Rooms = _context.Rooms.Where(r => reservedRooms.Contains(r.RoomId)).ToList();
            return View(reservation);
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
