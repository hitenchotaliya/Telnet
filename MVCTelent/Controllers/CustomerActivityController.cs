using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCTelent.Models;

namespace MVCTelent.Controllers
{
    public class CustomerActivityController : Controller
    {

        private readonly TelentDbContext _context;
        private readonly IWebHostEnvironment HostEnvironment;

        public CustomerActivityController(TelentDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            HostEnvironment = hostEnvironment;
        }
        // GET: CustomerActivityController
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PostJob()
        {
            string email = HttpContext.Session.GetString("CustomerEmail");
            ViewBag.CustomerEmail = email;
            var customer = _context.Customers.FirstOrDefault(c => c.Email == email);
            if (customer == null)
            {
                return NotFound();
            }
            return View();
        }
        public ActionResult CandidatesList(int id)
        {
            string email = HttpContext.Session.GetString("CustomerEmail");
            ViewBag.CustomerEmail = email;
            var customer = _context.Customers.FirstOrDefault(c => c.Email == email);
            return View();
        }
        public ActionResult Applicants(int id)
        {
            string email = HttpContext.Session.GetString("CustomerEmail");
            ViewBag.CustomerEmail = email;
            var customer = _context.Customers.FirstOrDefault(c => c.Email == email);
            return View();
        }
        
        // GET: CustomerActivityController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CustomerActivityController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CustomerActivityController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CustomerActivityController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CustomerActivityController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CustomerActivityController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CustomerActivityController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
