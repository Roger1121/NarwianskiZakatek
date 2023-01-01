using Microsoft.EntityFrameworkCore;
using NarwianskiZakatek.Data;
using NarwianskiZakatek.Models;
using NarwianskiZakatek.Services;
using NarwianskiZakatek.ViewModels;
using System.Web.Mvc;

namespace NarwianskiZakatek.Repositories
{
    public class ReservationsService : IReservationsService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _sender;

        public ReservationsService(ApplicationDbContext context, IEmailService sender)
        {
            _context = context;
            _sender = sender;
        }

        public async Task<PaginatedList<Reservation>> GetReservationsByParams(int? pageSize, int? pageNumber, string? sortOrder, DateTime? beginFrom, DateTime? beginTo, DateTime? endFrom, DateTime? endTo, decimal? priceFrom, decimal? priceTo, string? userId)
        {
            var reservations = _context.Reservations.Select(r => r);
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
            if (userId != null)
            {
                reservations = reservations.Where(r => r.UserId == userId);
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
                    reservations = reservations.OrderBy(r => r.BeginDate);
                    break;
            }
            return PaginatedList<Reservation>.Create(reservations.Include(r => r.User).AsNoTracking(), pageNumber ?? 1, pageSize ?? 10);
        }

        public Reservation GetReservation(string id)
        {
            return _context.Reservations.Where(m => m.ReservationId == id)
                .Include(r => r.ReservedRooms).Include(r => r.User).First();
        }

        public List<SelectListItem> FindAvailableRooms(DateTime beginDate, DateTime endDate)
        {
            var reservedRooms = GetIdsOfReservedRooms(beginDate, endDate);
            var rooms = _context.Rooms?.Where(r => !reservedRooms.Contains(r.RoomId));
            var list = rooms?.Select(r => new SelectListItem { Value = r.RoomId.ToString(), Text = r.ToString() }).ToList();
            return list ?? new List<SelectListItem>();
        }

        private List<int> GetIdsOfReservedRooms(DateTime beginDate, DateTime endDate)
        {
            List<ReservedRoom> reservedRooms = _context.ReservedRooms
                .Where(r => !r.Reservation.IsCancelled && ((r.Reservation.BeginDate >= beginDate && r.Reservation.BeginDate <= endDate)
                         || (r.Reservation.EndDate >= beginDate && r.Reservation.BeginDate <= endDate))).ToList();
            return reservedRooms.Select(r => r.RoomId).Distinct().ToList();
        }

        public async Task AddReservation(RoomsViewModel roomList, string userName)
        {
            var user = _context.Users.Where(u => u.UserName == userName).First();
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
                    Room = _context.Rooms.Find(roomId),
                    Reservation = reservation
                });
                price += _context.Rooms.Where(r => r.RoomId == roomId).First().Price;
            }
            reservation.Price = price * (roomList.EndDate.AddDays(1) - roomList.BeginDate).Days;
            _context.SaveChanges();
            _sender.ConfirmReservationAsync(user.Email, reservation);
        }

        public void CancelReservation(Reservation reservation)
        {
            reservation.IsCancelled = true;
            _context.SaveChanges();
            var user = _context.Users.Where(u => u.Id == reservation.UserId).First();
            _sender.CancelReservationAsync(user.Email, reservation);
        }

        public void RateReservation(OpinionViewModel opinionViewModel, Reservation reservation)
        {
            reservation.Opinion = opinionViewModel.Opinion;
            reservation.Rating = opinionViewModel.Rating;
            _context.SaveChanges();
        }

        public List<Room> GetRoomsByReservation(Reservation reservation)
        {
            List<int> reservedRooms = reservation.ReservedRooms.Select(r => r.RoomId).Distinct().ToList();
            return _context.Rooms.Where(r => reservedRooms.Contains(r.RoomId)).ToList();
        }
    }
}
