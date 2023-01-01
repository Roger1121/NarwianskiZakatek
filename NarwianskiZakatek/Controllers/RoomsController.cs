using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NarwianskiZakatek.Data;
using NarwianskiZakatek.Models;
using NarwianskiZakatek.Repositories;
using NarwianskiZakatek.Services;
using System.Data;

namespace NarwianskiZakatek.Controllers
{
    public class RoomsController : Controller
    {
        private readonly IRoomsService _service;

        public RoomsController(IRoomsService repository)
        {
            _service = repository;
        }

        // GET: Rooms
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Index(string? message)
        {
            ViewBag.Message = message;
            return View(_service.GetRooms());
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
                _service.AddRoom(room);
                return RedirectToAction(nameof(Index));
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            return View(room);
        }

        // GET: Rooms/Edit/5
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var room = await _service.Get((int)id);
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
                string message;
                try
                {
                    message = await _service.UpdateRoom(room);
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction("Index", new { message = message });
            }
            return View(room);
        }

        // GET: Rooms/Delete/5
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var room = await _service.Get((int)id);
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

            var room = await _service.Get((int)id);
            if (await _service.Delete(room))
            {
                message = "Pomyślnie usunięto pokój o numerze " + room.RoomNumber + ".";
            }
            return RedirectToAction("Index", new { message = message });
        }
    }
}
