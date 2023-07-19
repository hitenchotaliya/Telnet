using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using MVCTelent.Models;

namespace MVCTelent.Controllers
{
    public class UserProfileDetailsController : Controller
    {
        private readonly TelentDbContext _context;
        private readonly IWebHostEnvironment hostEnvironment;

        public UserProfileDetailsController(TelentDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this.hostEnvironment = hostEnvironment;
        }

        // GET: UserProfileDetails
        public async Task<IActionResult> Index(string searchString, int? pageNumber)
        {
            var upd = _context.UserProfileDetails.Include(u => u.User)
                .AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                upd = upd.Where(s => s.User.Email.ToLower().Contains(searchString.ToLower()));
            }

            if (searchString != null)
            {
                pageNumber = 1;
            }

            int pageSize = 18;
            var paginatedList = await PaginatedList<UserProfileDetail>.CreateAsync(upd.AsNoTracking(), pageNumber ?? 1, pageSize);

            ViewBag.SearchString = searchString;

            return View(paginatedList);
        }


        // GET: UserProfileDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.UserProfileDetails == null)
            {
                return NotFound();
            }

            var userProfileDetail = await _context.UserProfileDetails
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.UserProfileDetailId == id);
            if (userProfileDetail == null)
            {
                return NotFound();
            }

            return View(userProfileDetail);
        }

        // GET: UserProfileDetails/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email");
            return View();
        }

        // POST: UserProfileDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserProfileDetailId,UserId,Education,Certificate,Experience,Description,CreatedDate")] UserProfileDetail userProfileDetail,IFormFile photo, IFormFile photo1, IFormFile photo2)
        {
            if (ModelState.IsValid)
            {
                if (photo != null)
                {

                    string filename = photo.FileName;
                    string filepath = Path.Combine(hostEnvironment.WebRootPath, "UserProfileDetails", filename);

                    using (var stream = new FileStream(filepath, FileMode.Create))
                    {

                        await photo.CopyToAsync(stream);
                    }
                    userProfileDetail.Education = filename;

                }
                if (photo1 != null)
                {
                    string filename = photo1.FileName;
                    string filepath = Path.Combine(hostEnvironment.WebRootPath, "UserProfileDetails", filename);

                    using (var stream = new FileStream(filepath, FileMode.Create))
                    {

                        await photo1.CopyToAsync(stream);
                    }
                    userProfileDetail.Certificate = filename;

                }
                if (photo2 != null)
                {
                    string filename = photo2.FileName;
                    string filepath = Path.Combine(hostEnvironment.WebRootPath, "UserProfileDetails", filename);

                    using (var stream = new FileStream(filepath, FileMode.Create))
                    {

                        await photo2.CopyToAsync(stream);
                    }
                    userProfileDetail.Experience = filename;

                }
                userProfileDetail.CreatedDate = DateTime.Now;
                _context.Add(userProfileDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", userProfileDetail.UserId);
            return View(userProfileDetail);
        }

        // GET: UserProfileDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.UserProfileDetails == null)
            {
                return NotFound();
            }

            var userProfileDetail = await _context.UserProfileDetails.FindAsync(id);
            if (userProfileDetail == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", userProfileDetail.UserId);
            return View(userProfileDetail);
        }

        // POST: UserProfileDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserProfileDetailId,UserId,Education,Certificate,Experience,Description,CreatedDate")] UserProfileDetail userProfileDetail,IFormFile photo, IFormFile photo1, IFormFile photo2)
        {
            if (id != userProfileDetail.UserProfileDetailId)
            {
                return NotFound();
            }

           /* if (ModelState.IsValid)
            {*/
                var existingImage = await _context.UserProfileDetails.FindAsync(id);
                existingImage.UserId = userProfileDetail.UserId;
                existingImage.CreatedDate = userProfileDetail.CreatedDate;
                existingImage.Description = userProfileDetail.Description;

                try
                {
              
                    if (photo != null)
                    {
                        string filename = photo.FileName;
                        string filepath = Path.Combine(hostEnvironment.WebRootPath, "UserProfileDetails", filename);

                        using (var stream = new FileStream(filepath, FileMode.Create))
                        {

                            await photo.CopyToAsync(stream);
                        }
                        // category.CatImg = filename;

                        existingImage.Education = filename;
                        _context.Update(existingImage);

                    }
                    if (photo1 != null)
                    {
                        string filename = photo1.FileName;
                        string filepath = Path.Combine(hostEnvironment.WebRootPath, "UserProfileDetails", filename);

                        using (var stream = new FileStream(filepath, FileMode.Create))
                        {

                            await photo1.CopyToAsync(stream);
                        }
                        // category.CatImg = filename;

                        existingImage.Certificate = filename;
                        _context.Update(existingImage);

                    }
                    if (photo2 != null)
                    {
                        string filename = photo2.FileName;
                        string filepath = Path.Combine(hostEnvironment.WebRootPath, "UserProfileDetails", filename);

                        using (var stream = new FileStream(filepath, FileMode.Create))
                        {

                            await photo2.CopyToAsync(stream);
                        }
                        // category.CatImg = filename;

                        existingImage.Experience = filename;
                        _context.Update(existingImage);

                    }
                    _context.Update(existingImage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserProfileDetailExists(userProfileDetail.UserProfileDetailId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            /*}*/
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", userProfileDetail.UserId);
            return View(userProfileDetail);
        }

        // GET: UserProfileDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.UserProfileDetails == null)
            {
                return NotFound();
            }

            var userProfileDetail = await _context.UserProfileDetails
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.UserProfileDetailId == id);
            if (userProfileDetail == null)
            {
                return NotFound();
            }

            return View(userProfileDetail);
        }

        // POST: UserProfileDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.UserProfileDetails == null)
            {
                return Problem("Entity set 'TelentDbContext.UserProfileDetails'  is null.");
            }
            var userProfileDetail = await _context.UserProfileDetails.FindAsync(id);
            if (userProfileDetail != null)
            {
                _context.UserProfileDetails.Remove(userProfileDetail);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserProfileDetailExists(int id)
        {
          return (_context.UserProfileDetails?.Any(e => e.UserProfileDetailId == id)).GetValueOrDefault();
        }
    }
}
