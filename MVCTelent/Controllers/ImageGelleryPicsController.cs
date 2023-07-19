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
    public class ImageGelleryPicsController : Controller
    {
        private readonly TelentDbContext _context;
        private readonly IWebHostEnvironment hostEnvironment;

        public ImageGelleryPicsController(TelentDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this.hostEnvironment = hostEnvironment;
        }

        // GET: ImageGelleryPics
        public async Task<IActionResult> Index(string searchString, int? pageNumber)
        {
            var imp = _context.ImageGelleryPics
                .Include(x => x.ImageGellery)
                .AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                imp = imp.Where(s => s.PicName.ToLower().Contains(searchString.ToLower()) ||
                 s.Description.ToLower().Contains(searchString.ToLower())
                );
            }

            if (searchString != null)
            {
                pageNumber = 1;
            }

            int pageSize = 18;
            var paginatedList = await PaginatedList<ImageGelleryPic>.CreateAsync(imp.AsNoTracking(), pageNumber ?? 1, pageSize);

            ViewBag.SearchString = searchString;

            return View(paginatedList);
        }

        // GET: ImageGelleryPics/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ImageGelleryPics == null)
            {
                return NotFound();
            }

            var imageGelleryPic = await _context.ImageGelleryPics
                .Include(i => i.ImageGellery)
                .FirstOrDefaultAsync(m => m.ImageGelleryPicId == id);
            if (imageGelleryPic == null)
            {
                return NotFound();
            }

            return View(imageGelleryPic);
        }

        // GET: ImageGelleryPics/Create
        public IActionResult Create()
        {
            ViewData["ImageGelleryId"] = new SelectList(_context.ImageGelleries, "ImageGelleryId", "GelleryName");
            return View();
        }

        // POST: ImageGelleryPics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ImageGelleryPicId,PicName,Description,ImageGelleryId")] ImageGelleryPic imageGelleryPic,IFormFile photo)
        {
            if (ModelState.IsValid)
            {
                if (photo != null)
                {
                    string filename = photo.FileName;
                    string filepath = Path.Combine(hostEnvironment.WebRootPath, "UserImage", filename);

                    using (var stream = new FileStream(filepath, FileMode.Create))
                    {

                        await photo.CopyToAsync(stream);
                    }
                    imageGelleryPic.PicName = filename;

                }
               
                _context.Add(imageGelleryPic);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ImageGelleryId"] = new SelectList(_context.ImageGelleries, "ImageGelleryId", "ImageGelleryId", imageGelleryPic.ImageGelleryId);
            return View(imageGelleryPic);
        }

        // GET: ImageGelleryPics/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ImageGelleryPics == null)
            {
                return NotFound();
            }

            var imageGelleryPic = await _context.ImageGelleryPics.FindAsync(id);
            if (imageGelleryPic == null)
            {
                return NotFound();
            }
            ViewData["ImageGelleryId"] = new SelectList(_context.ImageGelleries, "ImageGelleryId", "GelleryName", imageGelleryPic.ImageGelleryId);
            return View(imageGelleryPic);
        }

        // POST: ImageGelleryPics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ImageGelleryPicId,PicName,Description,ImageGelleryId")] ImageGelleryPic imageGelleryPic, IFormFile photo)
        {
            if (id != imageGelleryPic.ImageGelleryPicId)
            {
                return NotFound();
            }

            /*if (ModelState.IsValid)
            {*/
                var existingImage = await _context.ImageGelleryPics.FindAsync(id);
                existingImage.Description = imageGelleryPic.Description;
                existingImage.ImageGelleryId = imageGelleryPic.ImageGelleryId;
                try
                {
                    if (photo != null)
                    {
                        string filename = photo.FileName;
                        string filepath = Path.Combine(hostEnvironment.WebRootPath, "UserImage", filename);

                        using (var stream = new FileStream(filepath, FileMode.Create))
                        {

                            await photo.CopyToAsync(stream);
                        }
                        // category.CatImg = filename;

                        existingImage.PicName = filename;
                        _context.Update(existingImage);

                    }
                    _context.Update(existingImage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImageGelleryPicExists(imageGelleryPic.ImageGelleryPicId))
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
            ViewData["ImageGelleryId"] = new SelectList(_context.ImageGelleries, "ImageGelleryId", "ImageGelleryId", imageGelleryPic.ImageGelleryId);
            return View(imageGelleryPic);
        }

        // GET: ImageGelleryPics/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ImageGelleryPics == null)
            {
                return NotFound();
            }

            var imageGelleryPic = await _context.ImageGelleryPics
                .Include(i => i.ImageGellery)
                .FirstOrDefaultAsync(m => m.ImageGelleryPicId == id);
            if (imageGelleryPic == null)
            {
                return NotFound();
            }

            return View(imageGelleryPic);
        }

        // POST: ImageGelleryPics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ImageGelleryPics == null)
            {
                return Problem("Entity set 'TelentDbContext.ImageGelleryPics'  is null.");
            }
            var imageGelleryPic = await _context.ImageGelleryPics.FindAsync(id);
            if (imageGelleryPic != null)
            {
                _context.ImageGelleryPics.Remove(imageGelleryPic);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ImageGelleryPicExists(int id)
        {
          return (_context.ImageGelleryPics?.Any(e => e.ImageGelleryPicId == id)).GetValueOrDefault();
        }
    }
}



