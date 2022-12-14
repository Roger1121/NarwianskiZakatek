using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NarwianskiZakatek.Data;
using NarwianskiZakatek.Models;
using NarwianskiZakatek.Services;
using System.Data;

namespace NarwianskiZakatek.Controllers
{
    public class RoomsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _sender;

        public RoomsController(ApplicationDbContext context, IEmailService sender)
        {
            _context = context;
            _sender = sender;
        }

        // GET: Rooms
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Index(string? message)
        {
            ViewBag.Message = message;
            return _context.Rooms != null ?
                        View(await _context.Rooms.OrderBy(r => r.RoomNumber).ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Rooms'  is null.");
        }

        // GET: Rooms/Create
        [Authorize(Roles = "Admin,Employee")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Rooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Create([Bind("RoomNumber,RoomCapacity,Price,IsActive")] Room room)
        {
            if (ModelState.IsValid)
            {
                _context.Add(room);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            return View(room);
        }

        // GET: Rooms/Edit/5
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Rooms == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            return View(room);
        }

        // POST: Rooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Edit(int id, [Bind("RoomId,RoomNumber,RoomCapacity,Price,IsActive")] Room room)
        {
            if (id != room.RoomId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string message = "Pokój został zaktualizowany.";
                try
                {
                    var oldRoom = _context.Rooms.Where(r => r.RoomId == id).AsNoTracking().First();
                    if(oldRoom.IsActive && !room.IsActive)
                    {
                        var reservationIds = _context.ReservedRooms.Where(rr => rr.RoomId == oldRoom.RoomId).Select(rr => rr.ReservationId).ToList();
                        var reservations = _context.Reservations.Where(r => reservationIds.Contains(r.ReservationId)).Include(r => r.User).ToList();
                        for (int i = 0; i < reservations.Count; i++)
                        {
                            reservations[i].IsCancelled = true;
                            _sender.CancelReservationAsync(reservations[i].User.Email, reservations[i]);
                        }
                        message = "Pokój został zaktualizowany. Pokój został wyłączony z użycia, a wszystkie nadchodzące rezerwacje tego pokoju zostały odwołane.";
                    }
                    
                    _context.Update(room);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists(room.RoomId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", new
                {
                    message = message
                });
            }
            return View(room);
        }

        // GET: Rooms/Delete/5
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Rooms == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .FirstOrDefaultAsync(m => m.RoomId == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string message = "Pokój nie został usunięty, gdyż istnieją w systemie powiązane z nim rezerwacje." +
                " Jeżeli chcesz wycofać pokój z użytku, zmień jego dostępność w oknie edycji pokoju.";
            if (_context.Rooms == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Rooms'  is null.");
            }
            var room = await _context.Rooms.FindAsync(id);
            bool canDelete = room?.ReservedRooms?.Count == 0;
            if (room != null && canDelete)
            {
                _context.Rooms.Remove(room);
                message = "Pomyślnie usunięto pokój o numerze " + room.RoomNumber + ".";
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new
            {
                message = message
            });
        }

        private bool RoomExists(int id)
        {
            return (_context.Rooms?.Any(e => e.RoomId == id)).GetValueOrDefault();
        }
    }
}
