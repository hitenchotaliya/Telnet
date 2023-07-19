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
    public class TelentRequestsController : Controller
    {
        private readonly TelentDbContext _context;

        public TelentRequestsController(TelentDbContext context)
        {
            _context = context;
        }

        // GET: TelentRequests
        public async Task<IActionResult> Index(string searchString, int? pageNumber)
        {
            var tr = _context.TelentRequests
               .Include(t => t.Category).Include(t => t.Customer)
                .AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                tr = tr.Where(s => s.Customer.Email.ToLower().Contains(searchString.ToLower()) || s.Category.CatName.ToLower().Contains(searchString.ToLower()));
            }

            if (searchString != null)
            {
                pageNumber = 1;
            }

            int pageSize = 18;
            var paginatedList = await PaginatedList<TelentRequest>.CreateAsync(tr.AsNoTracking(), pageNumber ?? 1, pageSize);

            ViewBag.SearchString = searchString;

            return View(paginatedList);
        }


        // GET: TelentRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TelentRequests == null)
            {
                return NotFound();
            }

            var telentRequest = await _context.TelentRequests
                .Include(t => t.Category)
                .Include(t => t.Customer)
                .FirstOrDefaultAsync(m => m.TelentReqId == id);
            if (telentRequest == null)
            {
                return NotFound();
            }

            return View(telentRequest);
        }

        // GET: TelentRequests/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CatName");
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Email");
            return View();
        }

        // POST: TelentRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TelentReqId,FromDate,ToDate,CategoryId,CustomerId,NoOfPerson,Address,Description,Amount,ContactPersonName")] TelentRequest telentRequest)
        {
            if (ModelState.IsValid)
            {
                _context.Add(telentRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", telentRequest.CategoryId);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", telentRequest.CustomerId);
            return View(telentRequest);
        }

        // GET: TelentRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TelentRequests == null)
            {
                return NotFound();
            }

            var telentRequest = await _context.TelentRequests.FindAsync(id);
            if (telentRequest == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CatName", telentRequest.CategoryId);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Email", telentRequest.CustomerId);
            return View(telentRequest);
        }

        // POST: TelentRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TelentReqId,FromDate,ToDate,CategoryId,CustomerId,NoOfPerson,Address,Description,Amount,ContactPersonName")] TelentRequest telentRequest)
        {
            if (id != telentRequest.TelentReqId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(telentRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TelentRequestExists(telentRequest.TelentReqId))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", telentRequest.CategoryId);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", telentRequest.CustomerId);
            return View(telentRequest);
        }

        // GET: TelentRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TelentRequests == null)
            {
                return NotFound();
            }

            var telentRequest = await _context.TelentRequests
                .Include(t => t.Category)
                .Include(t => t.Customer)
                .FirstOrDefaultAsync(m => m.TelentReqId == id);
            if (telentRequest == null)
            {
                return NotFound();
            }

            return View(telentRequest);
        }

        // POST: TelentRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TelentRequests == null)
            {
                return Problem("Entity set 'TelentDbContext.TelentRequests'  is null.");
            }
            var telentRequest = await _context.TelentRequests.FindAsync(id);
            if (telentRequest != null)
            {
                _context.TelentRequests.Remove(telentRequest);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TelentRequestExists(int id)
        {
          return (_context.TelentRequests?.Any(e => e.TelentReqId == id)).GetValueOrDefault();
        }
    }
}
