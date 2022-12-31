using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NarwianskiZakatek.Data;
using NarwianskiZakatek.Models;
using NarwianskiZakatek.Repositories;
using NarwianskiZakatek.Services;
using NarwianskiZakatek.ViewModels;

namespace NarwianskiZakatek.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly IReservationsService _service;
        private readonly IUsersService _userService;

        public ReservationsController(IReservationsService service, IUsersService userService)
        {
            _service = service;
            _userService = userService;
        }

        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Index(int? pageNumber, string? sortOrder, DateTime? beginFrom, DateTime? beginTo, DateTime? endFrom, DateTime? endTo, decimal? priceFrom, decimal? priceTo)
        {
            var username = HttpContext.User.Identity?.Name;
            ViewBag.Warnings = _userService.GetUserWarnings(username);

            ViewBag.BeginFrom = beginFrom;
            ViewBag.BeginTo = beginTo;
            ViewBag.EndFrom = endFrom;
            ViewBag.EndTo = endTo;
            ViewBag.PriceFrom = priceFrom;
            ViewBag.PriceTo = priceTo;
            ViewBag.SortOrder = sortOrder;

            return View(await _service.GetReservationsByParams(10, pageNumber, sortOrder, beginFrom, beginTo, endFrom, endTo, priceFrom, priceTo, null));
        }

        [Authorize(Roles = "Admin,Employee")]
        public IActionResult DetailsFull(string id)
        {
            Reservation reservation = _service.GetReservation(id);
            ViewBag.Rooms = _service.GetRoomsByReservation(reservation);
            return View(reservation);
        }

        [HttpGet]
        [Authorize(Roles = "User,Admin,Employee")]
        public IActionResult Date(string? message)
        {
            ViewBag.Message = message;
            return View();
        }
        [HttpGet]
        [Authorize(Roles = "User,Admin,Employee")]
        public async Task<IActionResult> MyReservations(string? message, int? pageNumber, string? sortOrder, DateTime? beginFrom, DateTime? beginTo, DateTime? endFrom, DateTime? endTo, decimal? priceFrom, decimal? priceTo)
        {
            var user = _userService.GetUserByUsername(HttpContext.User.Identity.Name);
            ViewBag.Message = message;
            ViewBag.SortOrder = sortOrder;
            ViewBag.BeginFrom = beginFrom;
            ViewBag.BeginTo = beginTo;
            ViewBag.EndFrom = endFrom;
            ViewBag.EndTo = endTo;
            ViewBag.PriceFrom = priceFrom;
            ViewBag.PriceTo = priceTo;

            return View(await _service.GetReservationsByParams(10, pageNumber, sortOrder, beginFrom, beginTo, endFrom, endTo, priceFrom, priceTo, user.Id));
        }

        [HttpPost]
        [Authorize(Roles = "User,Admin,Employee")]
        [ValidateAntiForgeryToken]
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
            ViewBag.RoomList = _service.FindAvailableRooms(beginDate, endDate);
            return View(new RoomsViewModel()
            {
                BeginDate = beginDate,
                EndDate = endDate
            });
        }

        [HttpPost]
        [Authorize(Roles = "User,Admin,Employee")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmAsync(RoomsViewModel roomList)
        {
            _service.AddReservation(roomList, HttpContext.User.Identity.Name);
            return RedirectToAction("MyReservations", new { message = "Rezerwacja została złożona." });
        }

        [HttpGet]
        [Authorize(Roles = "User,Admin,Employee")]
        public IActionResult Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var reservation = _service.GetReservation(id);
            if (reservation == null)
            {
                return NotFound();
            }
            if (HttpContext.User.Identity.Name != reservation.User.UserName)
            {
                return LocalRedirect("/Identity/Account/AccessDenied");
            }
            return View(reservation);
        }

        [HttpPost]
        [Authorize(Roles = "User,Admin,Employee")]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(string id)
        {
            var reservation = _service.GetReservation(id);
            if (reservation == null)
            {
                return NotFound();
            }
            if (HttpContext.User.Identity.Name != reservation.User.UserName)
            {
                return LocalRedirect("/Identity/Account/AccessDenied");
            }
            _service.CancelReservation(reservation);
            return RedirectToAction("MyReservations", new { message = "Rezerwacja została anulowana." });
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Employee")]
        public IActionResult DeleteAny(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var reservation = _service.GetReservation(id);
            return reservation == null ? NotFound() : View(reservation);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Employee")]
        [ActionName("DeleteAny")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteAnyPost(string id)
        {
            var reservation = _service.GetReservation(id);
            _service.CancelReservation(reservation);
            return RedirectToAction("Index", new { message = "Rezerwacja została anulowana." });
        }

        [Authorize(Roles = "User,Admin,Employee")]
        public IActionResult Rate(string id)
        {
            var reservation = _service.GetReservation(id);
            if (HttpContext.User.Identity.Name != reservation.User.UserName)
            {
                return LocalRedirect("/Identity/Account/AccessDenied");
            }
            return HttpContext.User.Identity.Name != reservation.User.UserName ?
                LocalRedirect("/Identity/Account/AccessDenied") :
                View(new OpinionViewModel()
                {
                    ReservationId = id
                });
        }

        [HttpPost]
        [Authorize(Roles = "User,Admin,Employee")]
        [ValidateAntiForgeryToken]
        public IActionResult Rate(OpinionViewModel opinionViewModel)
        {
            var reservation = _service.GetReservation(opinionViewModel.ReservationId);
            if (HttpContext.User.Identity.Name != reservation.User.UserName)
            {
                return LocalRedirect("/Identity/Account/AccessDenied");
            }
            _service.RateReservation(opinionViewModel, reservation);
            return RedirectToAction("MyReservations", new { message = "Dziękujemy za udzielenie opinii." });
        }

        [Authorize(Roles = "User,Admin,Employee")]
        public IActionResult Details(string id)
        {
            Reservation reservation = _service.GetReservation(id);
            ViewBag.Rooms = _service.GetRoomsByReservation(reservation);
            return HttpContext.User.Identity.Name != reservation.User.UserName ?
                LocalRedirect("/Identity/Account/AccessDenied") : View(reservation);
        }
    }
}
