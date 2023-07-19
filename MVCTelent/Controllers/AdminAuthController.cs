using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVCTelent.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace MVCTelent.Controllers
{
    public class AdminAuthController : Controller
    {
        // GET: AdminAuthController
        private readonly TelentDbContext _context;

        public AdminAuthController(TelentDbContext context)
        {
            _context = context;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        // GET: /AdminAuth/Login
        [HttpGet]
        public ActionResult Login()
        {

            return View();
        }
          // POST: /AdminAuth/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Admin admins, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Uname == admins.Uname && a.Password == admins.Password && a.Isactive == true);
                if (admin != null)
                {
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, admin.Name),
                new Claim(ClaimTypes.NameIdentifier, admin.AdminId.ToString()),
                // Add claims
            };
                    if (admin.Uname == "chirag")
                    {
                        claims.Add(new Claim(ClaimTypes.Role, "chirag")); // Assign "chirag" role
                    }

                    //  identity for the admin
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // Sign in the admin
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
                    return RedirectToAction("Index", "Home");
                    /* return RedirectToAction(nameof(Profile)); */
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View(admins);
        }

        // GET: /AdminAuth/Logout
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "AdminAuth");
        }

        public ActionResult Profile()
        {
            return View();
        }
        // GET: AdminAuthController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AdminAuthController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminAuthController/Create
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

        // GET: AdminAuthController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AdminAuthController/Edit/5
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

        // GET: AdminAuthController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AdminAuthController/Delete/5
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
