
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HelpDeskWeb.Data;
using HelpDeskWeb.Models;

namespace HelpDeskWeb.Controllers
{
    public class TicketsController : Controller
    {
        private readonly AppDbContext _context;

        public TicketsController(AppDbContext context)
        {
            _context = context;
        }

        // Bütün müraciətlərin siyahısı
        public async Task<IActionResult> Index()
        {
            var tickets = await _context.Tickets
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            return View(tickets);
        }

        // Yeni müraciət səhifəsi
        public IActionResult Create()
        {
            return View();
        }

        // Yeni müraciətin yadda saxlanılması
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ticket ticket)
        {
            if (!ModelState.IsValid)
            {
                return View(ticket);
            }

            ticket.CreatedAt = DateTime.Now;
            ticket.Status = "New";

            _context.Tickets.Add(ticket);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Müraciətin detalları
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var ticket = await _context.Tickets
                .FirstOrDefaultAsync(x => x.Id == id);

            if (ticket == null)
                return NotFound();

            return View(ticket);
        }

        // Redaktə səhifəsi
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var ticket = await _context.Tickets
                .FindAsync(id);

            if (ticket == null)
                return NotFound();

            return View(ticket);
        }

        // Redaktənin yadda saxlanılması
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Ticket ticket)
        {
            if (id != ticket.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(ticket);

            var existing = await _context.Tickets.FindAsync(id);

            if (existing == null)
                return NotFound();

            existing.Title = ticket.Title;
            existing.Description = ticket.Description;
            existing.Category = ticket.Category;
            existing.Priority = ticket.Priority;
            existing.Status = ticket.Status;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Silmə səhifəsi
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var ticket = await _context.Tickets
                .FirstOrDefaultAsync(x => x.Id == id);

            if (ticket == null)
                return NotFound();

            return View(ticket);
        }

        // Silmə əməliyyatı
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);

            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
