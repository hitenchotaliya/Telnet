    using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCTelent.Models;

namespace MVCTelent.Controllers
{
    public class UserDataController : Controller
    {
        private readonly TelentDbContext _context;
        private readonly IWebHostEnvironment HostEnvironment;

        public UserDataController(TelentDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            HostEnvironment = hostEnvironment;
        }
        // GET: UserDataController
        public async Task<IActionResult> Index()
        {
            string email = HttpContext.Session.GetString("CustomerEmail");
            string uemail = HttpContext.Session.GetString("UserEmail");

            ViewBag.CustomerEmail = email;
            ViewBag.UserEmail = uemail;

            return View();
        }

        [HttpGet]

        public async Task<IActionResult> UserProfile()
        {
            string uemail = HttpContext.Session.GetString("UserEmail");
            ViewBag.UserEmail = uemail;
            var user = _context.Users.FirstOrDefault(c => c.Email == uemail);
            if(user == null)
            {
                return NotFound();
            }
            // Set the UserId in session
            HttpContext.Session.SetInt32("UserId", user.UserId);
            List<Category> categories = await _context.Categories.ToListAsync();
            ViewBag.Categories = categories;
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GeneralPost(User model, IFormFile photo)
        {
           /* if (ModelState.IsValid)
            {*/

                var uemail = HttpContext.Session.GetString("UserEmail");

                if (string.IsNullOrEmpty(uemail))
                {
                    return RedirectToAction("Login", "UserAuth");
                }

                var user = _context.Users.FirstOrDefault(c => c.Email == uemail);

                if (user == null)
                {
                    return RedirectToAction("Login", "UserAuth");
                }

              
                user.Fname = model.Fname;
                user.Lname = model.Lname;
                user.Dob = model.Dob;
                user.Gender = model.Gender;
                user.Category = model.Category;

                try
                {
                    _context.Update(user);

                    if (photo != null && photo.Length > 0)
                    {
                        string filename = photo.FileName;
                        string filepath = Path.Combine(HostEnvironment.WebRootPath, "Image", filename);

                        using (var stream = new FileStream(filepath, FileMode.Create))
                        {
                            await photo.CopyToAsync(stream);
                        }

                        user.Img = filename;
                    }

                    await _context.SaveChangesAsync();

/*                session values with the new details*/
                 HttpContext.Session.SetString("UserLName", user.Lname);
                HttpContext.Session.SetString("UserFName", user.Fname);
                HttpContext.Session.SetString("UserGender", user.Gender);
                HttpContext.Session.SetString("UserImg", user.Img);
                return RedirectToAction("UserContact", "UserData");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction("UserContact", "UserData");
          /*  }
            return View(model);*/
        }

        [HttpGet]

        public async Task<IActionResult> UserContact()
        {
            string uemail = HttpContext.Session.GetString("UserEmail");
            ViewBag.UserEmail = uemail;
            var user = _context.Users.FirstOrDefault(c => c.Email == uemail);
            if (user == null)
            {
                return NotFound();
            }

            // Load states to populate dropdown
           /* List<State> states = await _context.States.ToListAsync();
            ViewBag.States = states;*/

            // Set the UserId in session
            HttpContext.Session.SetInt32("UserId", user.UserId);

            return View();
        }

      
        public JsonResult GetStates()
        {
            var states = _context.States.ToList();
            return Json(states);
        }
       
        public JsonResult GetCitiesByState(int stateId)
        {
            var cities = _context.Cities.Where(c => c.Sid == stateId).ToList();
            return Json(cities);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserContact(User model)
        {
            var uemail = HttpContext.Session.GetString("UserEmail");

            if (string.IsNullOrEmpty(uemail))
            {
                return RedirectToAction("Login", "UserAuth");
            }

            var user = _context.Users.FirstOrDefault(c => c.Email == uemail);

            if (user == null)
            {
                return RedirectToAction("Login", "UserAuth");
            }




            if (model.ContactNo != null)
            {
                user.ContactNo = model.ContactNo;
            }

           

            if (model.State != null)
            {
                user.State = model.State;
            }

            if (model.City != null)
            {
                user.City = model.City;
            }

            if (model.Address != null)
            {
                user.Address = model.Address;
            }


            try
            {
                _context.Update(user);
                await _context.SaveChangesAsync();

                // session values with the new details
                HttpContext.Session.SetString("UserContactNo", user.ContactNo);
                HttpContext.Session.SetString("UserAddress", user.Address);
                HttpContext.Session.SetString("UserCity", user.City);
                HttpContext.Session.SetString("UserState", user.State);

                // Return the updated user contact partial view
                return RedirectToAction("UserOtherDetail", "UserData");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(user.UserId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> UserOtherDetail(int id)
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserOtherDetail(UserProfileDetail model, IFormFile photo, IFormFile photo1, IFormFile photo2)
        {
            var uid = HttpContext.Session.GetInt32("UserId");

            if (uid == null)
            {
                return RedirectToAction("Login", "UserAuth");
            }

            var user = _context.UserProfileDetails.FirstOrDefault(c => c.UserId == uid);

            if (user == null)
            {
                // Create a new UserProfileDetail object
                user = new UserProfileDetail();
                user.UserId = uid.Value;
            }

            user.Description = model.Description;

            try
            {
                _context.Update(user);

                if (photo != null && photo.Length > 0)
                {
                    string filename = photo.FileName;
                    string filepath = Path.Combine(HostEnvironment.WebRootPath, "Image", filename);

                    using (var stream = new FileStream(filepath, FileMode.Create))
                    {
                        await photo.CopyToAsync(stream);
                    }

                    user.Experience = filename;
                }
                if (photo1 != null && photo1.Length > 0)
                {
                    string filename2 = photo1.FileName;
                    string filepathi = Path.Combine(HostEnvironment.WebRootPath, "Image", filename2);

                    using (var stream = new FileStream(filepathi, FileMode.Create))
                    {
                        await photo1.CopyToAsync(stream);
                    }

                    user.Certificate = filename2;
                }
                if (photo2 != null && photo2.Length > 0)
                {
                    string filename3 = photo2.FileName;
                    string filepathj = Path.Combine(HostEnvironment.WebRootPath, "Image", filename3);

                    using (var stream = new FileStream(filepathj, FileMode.Create))
                    {
                        await photo2.CopyToAsync(stream);
                    }

                    user.Education = filename3;
                }

                await _context.SaveChangesAsync();
                HttpContext.Session.SetString("UserEducation", user.Education);
                HttpContext.Session.SetString("UserCertificate", user.Certificate);
                HttpContext.Session.SetString("UserExperience", user.Experience);
              
                return RedirectToAction("Profile", "UserData");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (user.UserId.HasValue && !UserExists(user.UserId.Value))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        public ActionResult Profile(int id)
        {
            string uemail = HttpContext.Session.GetString("UserEmail");
            string Lname = HttpContext.Session.GetString("UserLName");
            string Fname = HttpContext.Session.GetString("UserFName");
            string Gender = HttpContext.Session.GetString("UserGender");
            string Img  = HttpContext.Session.GetString("UserImg");
            string ContactNo =  HttpContext.Session.GetString("UserContactNo");
            string Address = HttpContext.Session.GetString("UserAddress");
            string City = HttpContext.Session.GetString("UserCity");
            string State = HttpContext.Session.GetString("UserState");
            string Education =   HttpContext.Session.GetString("UserEducation");
            string Certificate = HttpContext.Session.GetString("UserCertificate");
            string Experience = HttpContext.Session.GetString("UserExperience");

            ViewBag.UserEmail = uemail;
            ViewBag.UserLName = Lname;
            ViewBag.UserFName = Fname;
            ViewBag.UserGender = Gender;
            ViewBag.UserImg = Img;
            ViewBag.UserContactNo = ContactNo;
            ViewBag.UserAddress = Address;
            ViewBag.UserCity = City;
            ViewBag.UserState = State;
            ViewBag.UserEducation = Education;
            ViewBag.UserCertificate = Certificate;
            ViewBag.UserExperience = Experience;

            var user = _context.Users.FirstOrDefault(c => c.Email == uemail);
            if (user == null)
            {
                return NotFound();
            }
            return View();
        }

        // GET: UserDataController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
        // GET: UserDataController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserDataController/Create
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

        // GET: UserDataController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserDataController/Edit/5
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

        // GET: UserDataController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserDataController/Delete/5
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
