using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MVCTelent.Models;

namespace MVCTelent.Controllers
{
    public class VideoGelleryVideosController : Controller
    {
        private readonly TelentDbContext _context;
        private readonly IWebHostEnvironment hostEnvironment;   

        public VideoGelleryVideosController(TelentDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this.hostEnvironment = hostEnvironment;
        }

        // GET: VideoGelleryVideos
        public async Task<IActionResult> Index(string searchString, int? pageNumber)
        {
            var vgv = _context.VideoGelleryVideos.Include(v => v.VideoGellery)
                .AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                vgv = vgv.Where(s => s.VideoGellery.GelleryName.ToLower().Contains(searchString.ToLower()));
            }

            if (searchString != null)
            {
                pageNumber = 1;
            }

            int pageSize = 18;
            var paginatedList = await PaginatedList<VideoGelleryVideo>.CreateAsync(vgv.AsNoTracking(), pageNumber ?? 1, pageSize);

            ViewBag.SearchString = searchString;

            return View(paginatedList);
        }

        // GET: VideoGelleryVideos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.VideoGelleryVideos == null)
            {
                return NotFound();
            }

            var videoGelleryVideo = await _context.VideoGelleryVideos
                .Include(v => v.VideoGellery)
                .FirstOrDefaultAsync(m => m.VideoGelleryVideoId == id);
            if (videoGelleryVideo == null)
            {
                return NotFound();
            }

            return View(videoGelleryVideo);
        }

        // GET: VideoGelleryVideos/Create
        public IActionResult Create()
        {
            ViewData["VideoGelleryId"] = new SelectList(_context.VideoGelleries, "VideoGelleryId", "GelleryName");
            return View();
        }

        // POST: VideoGelleryVideos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VideoGelleryVideoId,VideoGelleryId,VideoLink,Description")] VideoGelleryVideo videoGelleryVideo,IFormFile video)
        {
            if (ModelState.IsValid)
            {
                if (video != null)
                {
                    string filename = video.FileName;
                    string filepath = Path.Combine(hostEnvironment.WebRootPath, "UserVideo", filename);

                    using (var stream = new FileStream(filepath, FileMode.Create))
                    {

                        await video.CopyToAsync(stream);
                    }
                    videoGelleryVideo.VideoLink = filename;

                }
                _context.Add(videoGelleryVideo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["VideoGelleryId"] = new SelectList(_context.VideoGelleries, "VideoGelleryId", "VideoGelleryId", videoGelleryVideo.VideoGelleryId);
            return View(videoGelleryVideo);
        }

        // GET: VideoGelleryVideos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.VideoGelleryVideos == null)
            {
                return NotFound();
            }

            var videoGelleryVideo = await _context.VideoGelleryVideos.FindAsync(id);
            if (videoGelleryVideo == null)
            {
                return NotFound();
            }
            ViewData["VideoGelleryId"] = new SelectList(_context.VideoGelleries, "VideoGelleryId", "GelleryName", videoGelleryVideo.VideoGelleryId);
            return View(videoGelleryVideo);
        }

        // POST: VideoGelleryVideos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VideoGelleryVideoId,VideoGelleryId,VideoLink,Description")] VideoGelleryVideo videoGelleryVideo,IFormFile video)
        {
            if (id != videoGelleryVideo.VideoGelleryVideoId)
            {
                return NotFound();
            }

            /*if (ModelState.IsValid)
            {*/
                var existingVideo = await _context.VideoGelleryVideos.FindAsync(id);
                existingVideo.VideoGelleryId = videoGelleryVideo.VideoGelleryId;
                existingVideo.Description = videoGelleryVideo.Description;
                try
                {
                    if (video != null)
                    {
                        string filename = video.FileName;
                        string filepath = Path.Combine(hostEnvironment.WebRootPath, "UserVideo", filename);

                        using (var stream = new FileStream(filepath, FileMode.Create))
                        {

                            await video.CopyToAsync(stream);
                        }
                        // category.CatImg = filename;

                        existingVideo.VideoLink = filename;
                        _context.Update(existingVideo);

                    }
                    _context.Update(existingVideo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VideoGelleryVideoExists(videoGelleryVideo.VideoGelleryVideoId))
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
            ViewData["VideoGelleryId"] = new SelectList(_context.VideoGelleries, "VideoGelleryId", "VideoGelleryId", videoGelleryVideo.VideoGelleryId);
            return View(videoGelleryVideo);
        }

        // GET: VideoGelleryVideos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.VideoGelleryVideos == null)
            {
                return NotFound();
            }

            var videoGelleryVideo = await _context.VideoGelleryVideos
                .Include(v => v.VideoGellery)
                .FirstOrDefaultAsync(m => m.VideoGelleryVideoId == id);
            if (videoGelleryVideo == null)
            {
                return NotFound();
            }

            return View(videoGelleryVideo);
        }

        // POST: VideoGelleryVideos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.VideoGelleryVideos == null)
            {
                return Problem("Entity set 'TelentDbContext.VideoGelleryVideos'  is null.");
            }
            var videoGelleryVideo = await _context.VideoGelleryVideos.FindAsync(id);
            if (videoGelleryVideo != null)
            {
                _context.VideoGelleryVideos.Remove(videoGelleryVideo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VideoGelleryVideoExists(int id)
        {
          return (_context.VideoGelleryVideos?.Any(e => e.VideoGelleryVideoId == id)).GetValueOrDefault();
        }
    }
}
