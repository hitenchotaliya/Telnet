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
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;


namespace MVCTelent.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly TelentDbContext _context;
        private readonly IWebHostEnvironment HostEnvironment;

        public CategoriesController(TelentDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this.HostEnvironment = hostEnvironment;
        }

        // GET: Categories
        public async Task<IActionResult> Index(string searchString, int? pageNumber)
        {

            var categories = _context.Categories.AsQueryable();


			if (!String.IsNullOrEmpty(searchString))
			{
				categories = categories.Where(s => s.CatName!.ToLower().Contains(searchString.ToLower()));
			}

			if (searchString != null)
			{
				pageNumber = 1;
			}
			int pageSize = 12;
            var paginatedList = await PaginatedList<Category>.CreateAsync(categories.AsNoTracking(), pageNumber ?? 1, pageSize);

            ViewBag.SearchString = searchString; // Add this line to store the search query in the ViewBag

            return View(paginatedList);


            /*	return View(categories);*/

        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]


		public async Task<IActionResult> Create([Bind("CategoryId,CatName,Isactive")] Category category, IFormFile photo)
		{
			if (ModelState.IsValid)
			{
				if (photo != null)
				{
					string filename = photo.FileName;
					string filepath = Path.Combine(HostEnvironment.WebRootPath, "Image", filename);

					using (var stream = new FileStream(filepath, FileMode.Create))
					{
						// Load the image
						var image = Image.Load(photo.OpenReadStream());

						// Resize the image while maintaining aspect ratio and cropping to a square
						image.Mutate(x => x.Resize(new ResizeOptions
						{
							Size = new Size(200, 200),
							Mode = ResizeMode.Crop
						}));

						// Save the resized image to the server
						image.Save(stream, new JpegEncoder());
					}

					category.CatImg = filename;
				}

				_context.Add(category);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}

			return View(category);
		}

		// GET: Categories/Edit/5
		public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,CatName,CatImg,Isactive")] Category category, IFormFile photo)
        {
            if (id != category.CategoryId)
            {
                return NotFound();
            }

           /* if (ModelState.IsValid)
            {*/
                var existingImage = await _context.Categories.FindAsync(id);
                existingImage.CatName = category.CatName;
                existingImage.Isactive = category.Isactive;
                try
                {
                    if (photo != null)
                    {
                        string filename = photo.FileName;
                        string filepath = Path.Combine(HostEnvironment.WebRootPath, "Image", filename);

                        using (var stream = new FileStream(filepath, FileMode.Create))
                        {

                            await photo.CopyToAsync(stream);
                        }
                        // category.CatImg = filename;

                        existingImage.CatImg = filename;
                        _context.Update(existingImage);

                    }
                    _context.Update(existingImage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.CategoryId))
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
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Categories == null)
            {
                return Problem("Entity set 'TelentDbContext.Categories'  is null.");
            }
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
          return (_context.Categories?.Any(e => e.CategoryId == id)).GetValueOrDefault();
        }
    }
}
