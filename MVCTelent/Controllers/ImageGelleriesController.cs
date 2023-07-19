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
    public class ImageGelleriesController : Controller
    {
        private readonly TelentDbContext _context;

        public ImageGelleriesController(TelentDbContext context)
        {
            _context = context;
        }

        // GET: ImageGelleries
        public async Task<IActionResult> Index(string searchString, int? pageNumber)
        {
            var im = _context.ImageGelleries
                .Include(x => x.User)
                .AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                im = im.Where(s => s.GelleryName.ToLower().Contains(searchString.ToLower()) ||
     s.User.Email.ToLower().Contains(searchString.ToLower()));
            }

            if (searchString != null)
            {
                pageNumber = 1;
            }

            int pageSize = 18;
            var paginatedList = await PaginatedList<ImageGellery>.CreateAsync(im.AsNoTracking(), pageNumber ?? 1, pageSize);

            ViewBag.SearchString = searchString;

            return View(paginatedList);
        }

        
        // GET: ImageGelleries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ImageGelleries == null)
            {
                return NotFound();
            }

            var imageGellery = await _context.ImageGelleries
                .Include(i => i.User)
                .FirstOrDefaultAsync(m => m.ImageGelleryId == id);
            if (imageGellery == null)
            {
                return NotFound();
            }

            return View(imageGellery);
        }

        // GET: ImageGelleries/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email");
            return View();
        }

        // POST: ImageGelleries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ImageGelleryId,GelleryName,UserId,CreatedDate")] ImageGellery imageGellery)
        {
            if (ModelState.IsValid)
            {
                imageGellery.CreatedDate = DateTime.Now;
                _context.Add(imageGellery);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", imageGellery.UserId);
            return View(imageGellery);
        }

        // GET: ImageGelleries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ImageGelleries == null)
            {
                return NotFound();
            }

            var imageGellery = await _context.ImageGelleries.FindAsync(id);
            if (imageGellery == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", imageGellery.UserId);
            return View(imageGellery);
        }

        // POST: ImageGelleries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ImageGelleryId,GelleryName,UserId,CreatedDate")] ImageGellery imageGellery)
        {
            if (id != imageGellery.ImageGelleryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(imageGellery);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImageGelleryExists(imageGellery.ImageGelleryId))
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
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", imageGellery.UserId);
            return View(imageGellery);
        }

        // GET: ImageGelleries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ImageGelleries == null)
            {
                return NotFound();
            }

            var imageGellery = await _context.ImageGelleries
                .Include(i => i.User)
                .FirstOrDefaultAsync(m => m.ImageGelleryId == id);
            if (imageGellery == null)
            {
                return NotFound();
            }

            return View(imageGellery);
        }

        // POST: ImageGelleries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ImageGelleries == null)
            {
                return Problem("Entity set 'TelentDbContext.ImageGelleries'  is null.");
            }
            var imageGellery = await _context.ImageGelleries.FindAsync(id);
            if (imageGellery != null)
            {
                _context.ImageGelleries.Remove(imageGellery);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ImageGelleryExists(int id)
        {
          return (_context.ImageGelleries?.Any(e => e.ImageGelleryId == id)).GetValueOrDefault();
        }
    }
}
