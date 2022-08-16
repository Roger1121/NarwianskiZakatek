using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NarwianskiZakatek.Data;
using NarwianskiZakatek.Models;

namespace NarwianskiZakatek.Controllers
{
    public class DescriptionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DescriptionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Descriptions
        public async Task<IActionResult> Index()
        {
              return _context.Descriptions != null ? 
                          View(await _context.Descriptions.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Descriptions'  is null.");
        }

        // GET: Descriptions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Descriptions == null)
            {
                return NotFound();
            }

            var description = await _context.Descriptions
                .FirstOrDefaultAsync(m => m.DescriptionId == id);
            if (description == null)
            {
                return NotFound();
            }

            return View(description);
        }

        // GET: Descriptions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Descriptions == null)
            {
                return NotFound();
            }

            var description = await _context.Descriptions.FindAsync(id);
            if (description == null)
            {
                return NotFound();
            }
            return View(description);
        }

        // POST: Descriptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DescriptionId,Title,Content,PhotoUrl")] Description description)
        {
            if (id != description.DescriptionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(description);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DescriptionExists(description.DescriptionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(description);
        }

        private bool DescriptionExists(int id)
        {
          return (_context.Descriptions?.Any(e => e.DescriptionId == id)).GetValueOrDefault();
        }
    }
}
