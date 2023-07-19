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
    public class TelentAppliesController : Controller
    {
        private readonly TelentDbContext _context;

        public TelentAppliesController(TelentDbContext context)
        {
            _context = context;
        }

        // GET: TelentApplies
        public async Task<IActionResult> Index(string searchString, int? pageNumber)
        {
            var ta = _context.TelentApplies
                .Include(t => t.Category).Include(t => t.TelentReq)
                .AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                ta = ta.Where(s => s.Category.CatName.ToLower().Contains(searchString.ToLower())||
                s.TelentReq.ContactPersonName.ToLower().Contains(searchString.ToLower()));
            }

            if (searchString != null)
            {
                pageNumber = 1;
            }

            int pageSize = 18;
            var paginatedList = await PaginatedList<TelentApply>.CreateAsync(ta.AsNoTracking(), pageNumber ?? 1, pageSize);

            ViewBag.SearchString = searchString;

            return View(paginatedList);
        }

        // GET: TelentApplies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TelentApplies == null)
            {
                return NotFound();
            }

            var telentApply = await _context.TelentApplies
                .Include(t => t.Category)
                .Include(t => t.TelentReq)
                .FirstOrDefaultAsync(m => m.TelentApplyId == id);
            if (telentApply == null)
            {
                return NotFound();
            }

            return View(telentApply);
        }

        // GET: TelentApplies/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CatName");
            ViewData["TelentReqId"] = new SelectList(_context.TelentRequests, "TelentReqId", "ContactPersonName");
            return View();
        }

        // POST: TelentApplies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TelentApplyId,TelentReqId,CategoryId,Date,Status,Message")] TelentApply telentApply)
        {
            if (ModelState.IsValid)
            {
                telentApply.Date= DateTime.Now;
                _context.Add(telentApply);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", telentApply.CategoryId);
            ViewData["TelentReqId"] = new SelectList(_context.TelentRequests, "TelentReqId", "TelentReqId", telentApply.TelentReqId);
            return View(telentApply);
        }

        // GET: TelentApplies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TelentApplies == null)
            {
                return NotFound();
            }

            var telentApply = await _context.TelentApplies.FindAsync(id);
            if (telentApply == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CatName", telentApply.CategoryId);
            ViewData["TelentReqId"] = new SelectList(_context.TelentRequests, "TelentReqId", "ContactPersonName", telentApply.TelentReqId);
            return View(telentApply);
        }

        // POST: TelentApplies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TelentApplyId,TelentReqId,CategoryId,Date,Status,Message")] TelentApply telentApply)
        {
            if (id != telentApply.TelentApplyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(telentApply);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TelentApplyExists(telentApply.TelentApplyId))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", telentApply.CategoryId);
            ViewData["TelentReqId"] = new SelectList(_context.TelentRequests, "TelentReqId", "TelentReqId", telentApply.TelentReqId);
            return View(telentApply);
        }

        // GET: TelentApplies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TelentApplies == null)
            {
                return NotFound();
            }

            var telentApply = await _context.TelentApplies
                .Include(t => t.Category)
                .Include(t => t.TelentReq)
                .FirstOrDefaultAsync(m => m.TelentApplyId == id);
            if (telentApply == null)
            {
                return NotFound();
            }

            return View(telentApply);
        }

        // POST: TelentApplies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TelentApplies == null)
            {
                return Problem("Entity set 'TelentDbContext.TelentApplies'  is null.");
            }
            var telentApply = await _context.TelentApplies.FindAsync(id);
            if (telentApply != null)
            {
                _context.TelentApplies.Remove(telentApply);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TelentApplyExists(int id)
        {
          return (_context.TelentApplies?.Any(e => e.TelentApplyId == id)).GetValueOrDefault();
        }
    }
}
