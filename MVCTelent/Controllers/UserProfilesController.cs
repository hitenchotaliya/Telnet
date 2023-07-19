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
    public class UserProfilesController : Controller
    {
        private readonly TelentDbContext _context;

        public UserProfilesController(TelentDbContext context)
        {
            _context = context;
        }

        // GET: UserProfiles
        public async Task<IActionResult> Index(string searchString, int? pageNumber)
        {
            var up = _context.UserProfiles.Include(u => u.ImageGellery).Include(u => u.User).Include(u => u.VideoGellery)
                .AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                up = up.Where(s => s.User.Email.ToLower().Contains(searchString.ToLower()));
            }

            if (searchString != null)
            {
                pageNumber = 1;
            }

            int pageSize = 18;
            var paginatedList = await PaginatedList<UserProfile>.CreateAsync(up.AsNoTracking(), pageNumber ?? 1, pageSize);

            ViewBag.SearchString = searchString;

            return View(paginatedList);
        }


        // GET: UserProfiles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.UserProfiles == null)
            {
                return NotFound();
            }

            var userProfile = await _context.UserProfiles
                .Include(u => u.ImageGellery)
                .Include(u => u.User)
                .Include(u => u.VideoGellery)
                .FirstOrDefaultAsync(m => m.UserProfileId == id);
            if (userProfile == null)
            {
                return NotFound();
            }

            return View(userProfile);
        }

        // GET: UserProfiles/Create
        public IActionResult Create()
        {
            ViewData["ImageGelleryId"] = new SelectList(_context.ImageGelleries, "ImageGelleryId", "GelleryName");
            ViewData["VideoGelleryId"] = new SelectList(_context.VideoGelleries, "VideoGelleryId", "GelleryName");
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email");
            return View();
        }

        // POST: UserProfiles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserProfileId,UserId,ImageGelleryId,VideoGelleryId")] UserProfile userProfile)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userProfile);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ImageGelleryId"] = new SelectList(_context.ImageGelleries, "ImageGelleryId", "ImageGelleryId", userProfile.ImageGelleryId);
            ViewData["VideoGelleryId"] = new SelectList(_context.VideoGelleries, "VideoGelleryId", "VideoGelleryId", userProfile.VideoGelleryId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", userProfile.UserId);
            return View(userProfile);
        }

        // GET: UserProfiles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.UserProfiles == null)
            {
                return NotFound();
            }

            var userProfile = await _context.UserProfiles.FindAsync(id);
            if (userProfile == null)
            {
                return NotFound();
            }
            ViewData["ImageGelleryId"] = new SelectList(_context.ImageGelleries, "ImageGelleryId", "GelleryName", userProfile.ImageGelleryId);
            ViewData["VideoGelleryId"] = new SelectList(_context.VideoGelleries, "VideoGelleryId", "GelleryName", userProfile.VideoGelleryId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", userProfile.UserId);
            return View(userProfile);
        }

        // POST: UserProfiles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserProfileId,UserId,ImageGelleryId,VideoGelleryId")] UserProfile userProfile)
        {
            if (id != userProfile.UserProfileId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userProfile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserProfileExists(userProfile.UserProfileId))
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
            ViewData["ImageGelleryId"] = new SelectList(_context.ImageGelleries, "ImageGelleryId", "ImageGelleryId", userProfile.ImageGelleryId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", userProfile.UserId);
            return View(userProfile);
        }

        // GET: UserProfiles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.UserProfiles == null)
            {
                return NotFound();
            }

            var userProfile = await _context.UserProfiles
                .Include(u => u.ImageGellery)
                .Include(u => u.User)
                 .Include(u => u.VideoGellery)
                .FirstOrDefaultAsync(m => m.UserProfileId == id);
            if (userProfile == null)
            {
                return NotFound();
            }

            return View(userProfile);
        }

        // POST: UserProfiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.UserProfiles == null)
            {
                return Problem("Entity set 'TelentDbContext.UserProfiles'  is null.");
            }
            var userProfile = await _context.UserProfiles.FindAsync(id);
            if (userProfile != null)
            {
                _context.UserProfiles.Remove(userProfile);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserProfileExists(int id)
        {
          return (_context.UserProfiles?.Any(e => e.UserProfileId == id)).GetValueOrDefault();
        }
    }
}
