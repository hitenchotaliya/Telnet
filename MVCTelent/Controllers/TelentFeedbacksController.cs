using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCTelent.Models;

namespace MVCTelent.Controllers
{
    public class TelentFeedbacksController : Controller
    {
        private readonly TelentDbContext _context;

        public TelentFeedbacksController(TelentDbContext context)
        {
            _context = context;
        }

        // GET: TelentFeedbacks
        public async Task<IActionResult> Index(string searchString, int? pageNumber)
        {
            var tf = _context.TelentFeedbacks
                .Include(t => t.Category).Include(t => t.Customer)
                .AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                tf = tf.Where(s => s.Category.CatName.ToLower().Contains(searchString.ToLower()) ||
     s.Customer.Email.ToLower().Contains(searchString.ToLower()));
            }

            if (searchString != null)
            {
                pageNumber = 1;
            }

            int pageSize = 18;
            var paginatedList = await PaginatedList<TelentFeedback>.CreateAsync(tf.AsNoTracking(), pageNumber ?? 1, pageSize);

            ViewBag.SearchString = searchString;

            return View(paginatedList);
        }


        // GET: TelentFeedbacks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TelentFeedbacks == null)
            {
                return NotFound();
            }

            var telentFeedback = await _context.TelentFeedbacks
                .Include(t => t.Category)
                .Include(t => t.Customer)
                .FirstOrDefaultAsync(m => m.TelentFeedbackId == id);
            if (telentFeedback == null)
            {
                return NotFound();
            }

            return View(telentFeedback);
        }

        // GET: TelentFeedbacks/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CatName");
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Email");
            return View();
        }

        // POST: TelentFeedbacks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TelentFeedbackId,CategoryId,Review,Rating,CustomerId")] TelentFeedback telentFeedback)
        {
            if (ModelState.IsValid)
            {
                _context.Add(telentFeedback);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", telentFeedback.CategoryId);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", telentFeedback.CategoryId);
            return View(telentFeedback);
        }

        // GET: TelentFeedbacks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TelentFeedbacks == null)
            {
                return NotFound();
            }

            var telentFeedback = await _context.TelentFeedbacks.FindAsync(id);
            if (telentFeedback == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CatName", telentFeedback.CategoryId);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Email", telentFeedback.CategoryId);
            return View(telentFeedback);
        }

        // POST: TelentFeedbacks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TelentFeedbackId,CategoryId,Review,Rating,CustomerId")] TelentFeedback telentFeedback)
        {
            if (id != telentFeedback.TelentFeedbackId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(telentFeedback);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TelentFeedbackExists(telentFeedback.TelentFeedbackId))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", telentFeedback.CategoryId);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", telentFeedback.CategoryId);
            return View(telentFeedback);
        }

        // GET: TelentFeedbacks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TelentFeedbacks == null)
            {
                return NotFound();
            }

            var telentFeedback = await _context.TelentFeedbacks
                .Include(t => t.Category)
                .Include(t => t.Customer)
                .FirstOrDefaultAsync(m => m.TelentFeedbackId == id);
            if (telentFeedback == null)
            {
                return NotFound();
            }

            return View(telentFeedback);
        }

        // POST: TelentFeedbacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TelentFeedbacks == null)
            {
                return Problem("Entity set 'TelentDbContext.TelentFeedbacks'  is null.");
            }
            var telentFeedback = await _context.TelentFeedbacks.FindAsync(id);
            if (telentFeedback != null)
            {
                _context.TelentFeedbacks.Remove(telentFeedback);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TelentFeedbackExists(int id)
        {
          return (_context.TelentFeedbacks?.Any(e => e.TelentFeedbackId == id)).GetValueOrDefault();
        }
    }
}
