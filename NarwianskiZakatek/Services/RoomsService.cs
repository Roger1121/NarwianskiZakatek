using Microsoft.EntityFrameworkCore;
using NarwianskiZakatek.Data;
using NarwianskiZakatek.Models;
using NarwianskiZakatek.Services;

namespace NarwianskiZakatek.Repositories
{
    public class RoomsService : IRoomsService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _sender;

        public RoomsService(ApplicationDbContext context, IEmailService sender)
        {
            _context = context;
            _sender = sender;
        }

        public List<Room> GetRooms()
        {
            return _context.Rooms.OrderBy(r => r.RoomNumber).ToList();
        }
        public async Task<Room> Get(int id)
        {
            return await _context.Rooms.FindAsync(id);
        }

        public async Task AddRoom(Room room)
        {
            _context.Add(room);
            await _context.SaveChangesAsync();
        }

        public async Task<string> UpdateRoom(Room room)
        {
            string message = "Pokój został zaktualizowany.";
            var oldRoom = _context.Rooms.Where(r => r.RoomId == room.RoomId).AsNoTracking().First();
            if (oldRoom.IsActive && !room.IsActive)
            {
                var reservationIds = _context.ReservedRooms.Where(rr => rr.RoomId == oldRoom.RoomId).Select(rr => rr.ReservationId).ToList();
                var reservations = _context.Reservations.Where(r => reservationIds.Contains(r.ReservationId) && r.BeginDate > DateTime.Today).Include(r => r.User).ToList();
                for (int i = 0; i < reservations.Count; i++)
                {
                    reservations[i].IsCancelled = true;
                    _sender.CancelReservationAsync(reservations[i].User.Email, reservations[i]);
                }
                message = "Pokój został zaktualizowany. Pokój został wyłączony z użycia, a wszystkie nadchodzące rezerwacje tego pokoju zostały odwołane.";
            }

            _context.Update(room);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<bool> Delete(Room room)
        {
            bool canDelete = room?.ReservedRooms?.Count == 0;
            if (room != null && canDelete)
            {
                _context.Rooms.Remove(room);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public bool RoomExists(int id)
        {
            return (_context.Rooms?.Any(e => e.RoomId == id)).GetValueOrDefault();
        }
    }
}
