using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCTelent.Models;

namespace MVCTelent.Controllers
{
    public class UserActivityController : Controller
    {
        private readonly TelentDbContext _context;
        private readonly IWebHostEnvironment HostEnvironment;

        public UserActivityController(TelentDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            HostEnvironment = hostEnvironment;
        }
        // GET: UserActivityController
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult JobList()
        {
            string uemail = HttpContext.Session.GetString("UserEmail");
            ViewBag.UserEmail = uemail;
            var user = _context.Users.FirstOrDefault(c => c.Email == uemail);
            if (user == null)
            {
                return NotFound();
            }
            return View();
        }
        public ActionResult CompaniesList()
        {
            string uemail = HttpContext.Session.GetString("UserEmail");
            ViewBag.UserEmail = uemail;
            var user = _context.Users.FirstOrDefault(c => c.Email == uemail);
            if (user == null)
            {
                return NotFound();
            }
            // Retrieve data from the "Customer" table
            List<Customer> customers = _context.Customers.ToList(); // Assuming you have an instance of your DbContext named "db" and a Customer model class

            // Pass the data to the CompaniesList view
            return View(customers);

        }
        public ActionResult BlogList()
        {
            string uemail = HttpContext.Session.GetString("UserEmail");
            ViewBag.UserEmail = uemail;
            string cemail = HttpContext.Session.GetString("CustomerEmail");
            ViewBag.CustomerEmail = cemail;
            return View();
        }
        public ActionResult CompanyDetails()
        {
            string uemail = HttpContext.Session.GetString("UserEmail");
            ViewBag.UserEmail = uemail;
            var user = _context.Users.FirstOrDefault(c => c.Email == uemail);
            if (user == null)
            {
                return NotFound();
            }
            return View();
        }
        // GET: UserActivityController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserActivityController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserActivityController/Create
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

        // GET: UserActivityController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserActivityController/Edit/5
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

        // GET: UserActivityController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserActivityController/Delete/5
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
