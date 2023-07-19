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
    public class VideoGelleriesController : Controller
    {
        private readonly TelentDbContext _context;

        public VideoGelleriesController(TelentDbContext context)
        {
            _context = context;
        }

        // GET: VideoGelleries
        public async Task<IActionResult> Index(string searchString, int? pageNumber)
        {
            var vg = _context.VideoGelleries.Include(v => v.User)
                .AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                vg = vg.Where(s => s.User.Email.ToLower().Contains(searchString.ToLower()) || s.GelleryName.ToLower().Contains(searchString.ToLower())
                
                );
            }

            if (searchString != null)
            {
                pageNumber = 1;
            }

            int pageSize = 18;
            var paginatedList = await PaginatedList<VideoGellery>.CreateAsync(vg.AsNoTracking(), pageNumber ?? 1, pageSize);

            ViewBag.SearchString = searchString;

            return View(paginatedList);
        }


        // GET: VideoGelleries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.VideoGelleries == null)
            {
                return NotFound();
            }

            var videoGellery = await _context.VideoGelleries
                .Include(v => v.User)
                .FirstOrDefaultAsync(m => m.VideoGelleryId == id);
            if (videoGellery == null)
            {
                return NotFound();
            }

            return View(videoGellery);
        }

        // GET: VideoGelleries/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email");
            return View();
        }

        // POST: VideoGelleries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VideoGelleryId,GelleryName,UserId,CreatedDate")] VideoGellery videoGellery)
        {
            if (ModelState.IsValid)
            {
                videoGellery.CreatedDate = DateTime.Now;
                _context.Add(videoGellery);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", videoGellery.UserId);
            return View(videoGellery);
        }

        // GET: VideoGelleries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.VideoGelleries == null)
            {
                return NotFound();
            }

            var videoGellery = await _context.VideoGelleries.FindAsync(id);
            if (videoGellery == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", videoGellery.UserId);
            return View(videoGellery);
        }

        // POST: VideoGelleries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VideoGelleryId,GelleryName,UserId,CreatedDate")] VideoGellery videoGellery)
        {
            if (id != videoGellery.VideoGelleryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(videoGellery);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VideoGelleryExists(videoGellery.VideoGelleryId))
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
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", videoGellery.UserId);
            return View(videoGellery);
        }

        // GET: VideoGelleries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.VideoGelleries == null)
            {
                return NotFound();
            }

            var videoGellery = await _context.VideoGelleries
                .Include(v => v.User)
                .FirstOrDefaultAsync(m => m.VideoGelleryId == id);
            if (videoGellery == null)
            {
                return NotFound();
            }

            return View(videoGellery);
        }

        // POST: VideoGelleries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.VideoGelleries == null)
            {
                return Problem("Entity set 'TelentDbContext.VideoGelleries'  is null.");
            }
            var videoGellery = await _context.VideoGelleries.FindAsync(id);
            if (videoGellery != null)
            {
                _context.VideoGelleries.Remove(videoGellery);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VideoGelleryExists(int id)
        {
          return (_context.VideoGelleries?.Any(e => e.VideoGelleryId == id)).GetValueOrDefault();
        }
    }
}
