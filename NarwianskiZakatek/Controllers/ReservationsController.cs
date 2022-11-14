using Foolproof;
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
using System.Drawing.Printing;

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
        public async Task<IActionResult> Index(int? pageNumber, string? sortOrder, DateTime? beginFrom, DateTime? beginTo, DateTime? endFrom, DateTime? endTo, decimal? priceFrom, decimal? priceTo)
        {
            var reservations = from r in _context.Reservations select r;

            ViewBag.BeginFrom = beginFrom;
            ViewBag.BeginTo = beginTo;
            ViewBag.EndFrom = endFrom;
            ViewBag.EndTo = endTo;
            ViewBag.PriceFrom = priceFrom;
            ViewBag.PriceTo = priceTo;

            if(beginFrom != null)
            {
                reservations = reservations.Where(r => r.BeginDate >= beginFrom);
            }
            if (beginTo != null)
            {
                reservations = reservations.Where(r => r.BeginDate <= beginTo);
            }
            if (endFrom != null)
            {
                reservations = reservations.Where(r => r.EndDate >= endFrom);
            }
            if (endTo != null)
            {
                reservations = reservations.Where(r => r.EndDate <= endTo);
            }
            if (priceFrom != null)
            {
                reservations = reservations.Where(r => r.Price >= priceFrom);
            }
            if (priceTo != null)
            {
                reservations = reservations.Where(r => r.Price <= priceTo);
            }

            ViewBag.SortOrder = sortOrder;
            
            switch (sortOrder)
            {
                case "begin":
                    reservations = reservations.OrderBy(r => r.BeginDate);
                    break;
                case "begin_desc":
                    reservations = reservations.OrderByDescending(r => r.BeginDate);
                    break;
                case "end":
                    reservations = reservations.OrderBy(r => r.EndDate);
                    break;
                case "end_desc":
                    reservations = reservations.OrderByDescending(r => r.EndDate);
                    break;
                case "price":
                    reservations = reservations.OrderBy(r => r.Price);
                    break;
                case "price desc":
                    reservations = reservations.OrderByDescending(r => r.Price);
                    break;
                default:
                    ViewBag.SortOrder = "begin";
                    reservations = reservations.OrderBy(r => r.BeginDate);
                    break;
            }

            int pageSize = 3;
            return _context.Reservations != null ?
                View(await PaginatedList<Reservation>.CreateAsync(reservations.AsNoTracking(), pageNumber ?? 1, pageSize)) :
                          Problem("Entity set 'ApplicationDbContext.Users'  is null.");
        }

        [Authorize(Roles = "Admin,Employee")]
        public IActionResult DetailsFull(string id)
        {
            Reservation reservation = _context.Reservations.Where(m => m.ReservationId == id).Include(r => r.ReservedRooms).Include(r => r.User).First();
            List<int> reservedRooms = reservation.ReservedRooms.Select(r => r.RoomId).Distinct().ToList();
            ViewBag.Rooms = _context.Rooms.Where(r => reservedRooms.Contains(r.RoomId)).ToList();
            return View(reservation);
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public IActionResult Date(string? message)
        {
            ViewBag.Message = message;
            return View();
        }
        [HttpGet]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> MyReservations(string? message, int? pageNumber, string? sortOrder, DateTime? beginFrom, DateTime? beginTo, DateTime? endFrom, DateTime? endTo, decimal? priceFrom, decimal? priceTo)
        {
            var user = _context.Users.Where(u => u.UserName == HttpContext.User.Identity.Name).First();
            ViewBag.Message = message;
            int pageSize = 3;

            ViewBag.SortOrder = sortOrder;
            var reservations = _context.Reservations.Where(r => r.UserId == user.Id);

            ViewBag.BeginFrom = beginFrom;
            ViewBag.BeginTo = beginTo;
            ViewBag.EndFrom = endFrom;
            ViewBag.EndTo = endTo;
            ViewBag.PriceFrom = priceFrom;
            ViewBag.PriceTo = priceTo;

            if (beginFrom != null)
            {
                reservations = reservations.Where(r => r.BeginDate >= beginFrom);
            }
            if (beginTo != null)
            {
                reservations = reservations.Where(r => r.BeginDate <= beginTo);
            }
            if (endFrom != null)
            {
                reservations = reservations.Where(r => r.EndDate >= endFrom);
            }
            if (endTo != null)
            {
                reservations = reservations.Where(r => r.EndDate <= endTo);
            }
            if (priceFrom != null)
            {
                reservations = reservations.Where(r => r.Price >= priceFrom);
            }
            if (priceTo != null)
            {
                reservations = reservations.Where(r => r.Price <= priceTo);
            }

            switch (sortOrder)
            {
                case "begin":
                    reservations = reservations.OrderBy(r => r.BeginDate);
                    break;
                case "begin_desc":
                    reservations = reservations.OrderByDescending(r => r.BeginDate);
                    break;
                case "end":
                    reservations = reservations.OrderBy(r => r.EndDate);
                    break;
                case "end_desc":
                    reservations = reservations.OrderByDescending(r => r.EndDate);
                    break;
                case "price":
                    reservations = reservations.OrderBy(r => r.Price);
                    break;
                case "price desc":
                    reservations = reservations.OrderByDescending(r => r.Price);
                    break;
                default:
                    ViewBag.SortOrder = "begin";
                    reservations = reservations.OrderBy(r => r.BeginDate);
                    break;
            }

            return _context.Reservations != null ?
                View(await PaginatedList<Reservation>.CreateAsync(reservations.AsNoTracking(), pageNumber ?? 1, pageSize)) :
                          Problem("Entity set 'ApplicationDbContext.Users'  is null.");
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public IActionResult New(DateTime beginDate, DateTime endDate)
        {
            if (beginDate <= DateTime.Now)
            {
                return RedirectToAction("Date", new { message = "Nie możesz rezerwować pobytu z datą przeszłą." });
            }
            if (endDate <= beginDate)
            {
                return RedirectToAction("Date", new { message = "Data zakończenia rezerwacji nie może być wcześniejsza, niż data rozpoczęcia." });
            }
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

        [HttpGet]
        [Authorize(Roles = "User")]
        public IActionResult Delete(string? id)
        {
            if (id == null || _context.Reservations == null)
            {
                return NotFound();
            }
            var reservation = _context.Reservations.FirstOrDefault(m => m.ReservationId == id);

            if (reservation == null || HttpContext.User.Identity.Name != _context.Users.Where(u => u.Id == reservation.UserId).First().Name)
            {
                return NotFound();
            }

            return View(reservation);
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(string id)
        {
            if (_context.Reservations == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Reservations'  is null.");
            }
            var reservation = _context.Reservations.Find(id);
            if (reservation == null || HttpContext.User.Identity.Name != _context.Users.Where(u => u.Id == reservation.UserId).First().Name)
            {
                return RedirectToAction("MyReservations", new { message = "Rezerwacja nie została znaleziona lub nie masz uprawnień do anulowania tej rezerwacji." });
            }
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
                var user = _context.Users.Where(u => u.UserName == HttpContext.User.Identity.Name).First();
                _sender.CancelReservationAsync(user.Email, reservation);
            }
            _context.SaveChanges();
            return RedirectToAction("MyReservations", new { message = "Rezerwacja została anulowana." });
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Employee")]
        public IActionResult DeleteAny(string? id)
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
        [ActionName("DeleteAny")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteAnyPost(string id)
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
        public IActionResult Rate(string reservationId)
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
        public IActionResult Details(string id)
        {
            var currentUserId = _context.Users.Where(u => u.UserName == HttpContext.User.Identity.Name).First().Id;
            List<Reservation> reservations = _context.Reservations.Where(m => m.ReservationId == id && m.UserId == currentUserId).Include(r => r.ReservedRooms).ToList();
            if (reservations == null)
            {
                return NotFound();
            }
            var reservation = reservations.FirstOrDefault();
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
