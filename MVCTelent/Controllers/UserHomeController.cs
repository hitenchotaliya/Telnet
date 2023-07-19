using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using MVCTelent.Models;

namespace MVCTelent.Controllers
{
    public class UserHomeController : Controller
    {

        private readonly TelentDbContext _context;
        private readonly IWebHostEnvironment HostEnvironment;

        public UserHomeController(TelentDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            HostEnvironment = hostEnvironment;
        }
        // GET: UserHomeController
        public ActionResult Index()
        {

            string email = HttpContext.Session.GetString("CustomerEmail");
            string uemail = HttpContext.Session.GetString("UserEmail");

            ViewBag.CustomerEmail = email;
            ViewBag.UserEmail = uemail;
            
            return View();

        }
        public ActionResult Login(int id)
        {
            return View();
        }
        public ActionResult Home()
        {
            return View();
        }
        [HttpGet]
        
        public async Task<IActionResult> CreateProfileGeneralGet()
        {
            string email = HttpContext.Session.GetString("CustomerEmail");
            ViewBag.CustomerEmail = email;
            var customer = _context.Customers.FirstOrDefault(c => c.Email == email);
            if (customer == null)
            {
                return NotFound();
            }

            var model = new ProfileViewModel
            {
                Customer = customer
            };

            return View(customer);

        }






        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProfileGeneralPost(Customer model, IFormFile photo)
        {
            if (ModelState.IsValid)
            {

                var email = HttpContext.Session.GetString("CustomerEmail");

                if (string.IsNullOrEmpty(email))
                {
                    return RedirectToAction("Login", "UserAuth");
                }

                var customer = _context.Customers.FirstOrDefault(c => c.Email == email);

                if (customer == null)
                {
                    return RedirectToAction("Login", "UserAuth");
                }

                customer.Name = model.Name;
                customer.CreatedDate = model.CreatedDate;
                customer.ContactNo = model.ContactNo;
                customer.Address = model.Address;
                customer.City = model.City;
                customer.State = model.State;

                try
                {
                    _context.Update(customer);

                    if (photo != null && photo.Length > 0)
                    {
                        string filename = photo.FileName;
                        string filepath = Path.Combine(HostEnvironment.WebRootPath, "Image", filename);

                        using (var stream = new FileStream(filepath, FileMode.Create))
                        {
                            await photo.CopyToAsync(stream);
                        }

                        customer.Img = filename;
                    }

                    await _context.SaveChangesAsync();

                    // session values with the new details
                    HttpContext.Session.SetString("CustomerName", customer.Name);
                    HttpContext.Session.SetString("CustomerContactNo", customer.ContactNo);
                    HttpContext.Session.SetString("CustomerAddress", customer.Address);
                    HttpContext.Session.SetString("CustomerCity", customer.City);
                    HttpContext.Session.SetString("CustomerState", customer.State);
                    HttpContext.Session.SetString("CustomerImg", customer.Img);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction("Profile", "UserHome");
            }

            return View(model);
        }
        private bool CustomerExists(int id)
        {
            return (_context.Customers?.Any(e => e.CustomerId == id)).GetValueOrDefault();
        }
        // GET: UserHomeController/Details/5
        public ActionResult Profile()
        {
            string email = HttpContext.Session.GetString("CustomerEmail");
            string name = HttpContext.Session.GetString("CustomerName");
            string img = HttpContext.Session.GetString("CustomerImg");
            string number = HttpContext.Session.GetString("CustomerContactNo");
            string adrs = HttpContext.Session.GetString("CustomerAddress");
            string city = HttpContext.Session.GetString("CustomerCity");
            string state = HttpContext.Session.GetString("CustomerState");


            ViewBag.CustomerEmail = email;
            ViewBag.CustomerName= name;
            ViewBag.CustomerImg= img;
            ViewBag.CustomerContactNo = number;
            ViewBag.CustomerAddress = adrs;
            ViewBag.CustomerCity = city;
            ViewBag.CustomerState = state;

            return View();
        }

        // GET: UserHomeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserHomeController/Create
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

        // GET: UserHomeController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserHomeController/Edit/5
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

        // GET: UserHomeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserHomeController/Delete/5
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
