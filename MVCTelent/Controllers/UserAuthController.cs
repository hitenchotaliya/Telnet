using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MVCTelent.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using System.Net.Mail;
using System.Net;

using BCryptNet = BCrypt.Net.BCrypt;
using System.Net.Mail;
using System.Text.Encodings.Web;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;
using Microsoft.AspNetCore.Identity;

namespace MVCTelent.Controllers
{
    public class UserAuthController : Controller
    {
        // GET: UserAuthController
        private readonly TelentDbContext _context;
     //   private readonly UserManager<User> _userManager;
        public UserAuthController(TelentDbContext context)
        {
            _context = context;
        }
        private string EncryptPassword(string password)
        {
            string salt = BCryptNet.GenerateSalt();

            string hashedPassword = BCryptNet.HashPassword(password, salt);

            return hashedPassword;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistrationModel model,string options )
        {
           /* if (ModelState.IsValid)
            {*/
                if (options == "Candidate")
                {
                string activationCode = Guid.NewGuid().ToString();

                var user = new User
                    {
                        Email = model.Email,
                        Password = EncryptPassword(model.Password),
                        CreatedDate = DateTime.Now,
                        IsPaid = false,

                };

                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
                string returnUrl = null;
                returnUrl ??= Url.Content("~/");
                var callbackUrl = Url.Action(
                           "AccountActivated",
                             "UserAuth",
                    new { userId = user.UserId, code = activationCode, returnUrl = returnUrl },
                           protocol: Request.Scheme);
                await SendConfirmationEmail(model.Email,
                  "Confirm your email",
                  $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                HttpContext.Session.SetString("FirstLoginUser", "true");
                return RedirectToAction("emailconfirm", "UserAuth");
            }
                else if (options == "Employer")
                {
                string activationCode = Guid.NewGuid().ToString();
                var customer = new Customer
                {
                    Email = model.Email,
                    Password = EncryptPassword(model.Password),
                    CreatedDate = DateTime.Now,
                    Isactive = false,
                    
                };

                    _context.Customers.Add(customer);
                    await _context.SaveChangesAsync();
                string returnUrl = null;
                returnUrl ??= Url.Content("~/");
                var callbackUrl = Url.Action(
                           "AccountActivated",
                             "UserAuth",
                            new { customerId = customer.CustomerId, code = activationCode, returnUrl = returnUrl },
                           protocol: Request.Scheme);
              await  SendConfirmationEmail(model.Email,
                "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                HttpContext.Session.SetString("FirstLogin", "true");
                return RedirectToAction("emailconfirm", "UserAuth");
                }
            /*}*/

            return View(model);
        }
        public async Task<IActionResult> emailconfirm()
        {
           
            return View();
        }
       
      

        private async Task<bool> SendConfirmationEmail(string email, string subject, string confirmlink)
        {
            
            try
            {
                MailMessage message = new MailMessage();
                System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient();
                message.From = new MailAddress("chotaliyahiten8@gmail.com");
                message.To.Add(email);
                message.Subject = subject;
                message.IsBodyHtml = true;
                message.Body = confirmlink;


                smtpClient.Port = 587;
                smtpClient.Host = "smtp.gmail.com";

                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential("chotaliyahiten8@gmail.com", "ymyerhqimqyideva");
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Send(message);
                return true;
            }
            catch (Exception)
            {

                return false;
            }


            /*var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Hiten", "chotaliyahiten@gmail.com"));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = "Email Confirmation";

            var bodyBuilder = new BodyBuilder();
            var confirmationLink = $"http://localhost:7261/Account/ActivateAccount?code={activationCode}&customerId={{CustomerId}}\"";
            bodyBuilder.HtmlBody = $"<p>Please confirm your email by clicking <a href='{confirmationLink}'>here</a>.</p>";

            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("chotaliyahiten8@gmail.com", "ymyerhqimqyideva");
                client.Send(message);
                client.Disconnect(true);
            }*/
        }
       
        [HttpGet]
        public async Task<IActionResult> AccountActivated(int customerId,int userId) 
        {

            if (customerId == 0  && userId == 0)
            {
                return RedirectToPage("/Index");
            }
            if (userId != 0)
            {
                var user = await _context.Users.FindAsync(userId);
                if (user != null)
                {
                    user.IsPaid = true; // Set IsActive to true when email is confirmed
                    await _context.SaveChangesAsync();
                    return View();
                }
            }
            else
            {
                var customer = await _context.Customers.FindAsync(customerId);
                if (customer != null)
                {
                    customer.Isactive = true; // Set IsActive to true when email is confirmed
                    await _context.SaveChangesAsync();
                    return View();
                }
            }

            ViewData["Error"] = "There is an error";
            return View();
            /*var customer = await _context.Customers.FindAsync(customerId);
          

            try
            { 
                if(customer.CustomerId == customerId)
                {
                    customer.Isactive = true; // Set IsActive to true when email is confirmed 

                    await _context.SaveChangesAsync();
                }
                else
                {
                    ViewData["Error"] = "There is error";
                    return View();
                }
                return View();
            }
            catch (Exception)
            {

                throw;
            }
*/
        }

        [HttpPost]

        public async Task<IActionResult> Login(string email, string password)
        {
            // Check if the email exists in the Customers table
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == email);

            if (customer != null)
            {
                // Customer login logic
                if (customer.Isactive != true)
                {
                    ViewBag.InfoMessage = "Your email is not confirmed. Please check your inbox for the confirmation email.";
                    return RedirectToAction("EmailConfirmationRequired", "UserAuth");
                }

                bool passwordsMatch = VerifyPassword(password, customer.Password);

                if (passwordsMatch)
                {
                    // Store customer-related session values
                    HttpContext.Session.SetInt32("CustomerId", customer.CustomerId);
                    HttpContext.Session.SetString("CustomerEmail", customer.Email);

                    TempData["SuccessMessage"] = "Login successful!";
                    TempData["ShowSuccessModal"] = true;

                    bool isFirstLogin = HttpContext.Session.GetString("FirstLogin") == "true";

                    if (isFirstLogin)
                    {
                        HttpContext.Session.Remove("FirstLogin");
                        return RedirectToAction("CreateProfileGeneralGet", "UserHome");
                    }
                    else
                    {
                        return RedirectToAction("Index", "UserHome");
                    }
                }
                else
                {
                    // Incorrect password for customer
                    ViewBag.ErrorMessage = "Invalid login attempt. Please check your email and password.";
                    return RedirectToAction("Login");
                }
            }

            // Check if the email exists in the Users table
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user != null)
            {
                /*if (user.IsPaid != true)
                {
                    ViewBag.InfoMessage = "Your email is not confirmed. Please check your inbox for the confirmation email.";
                    return RedirectToAction("EmailConfirmationRequired", "UserAuth");
                }*/

                bool passwordsMatch = VerifyPassword(password, user.Password);

                if (passwordsMatch)
                {
                    // Store user-related session values
                    HttpContext.Session.SetInt32("UserId", user.UserId);
                    HttpContext.Session.SetString("UserEmail", user.Email);

                    TempData["SuccessMessage"] = "Login successful!";
                    TempData["ShowSuccessModal"] = true;

                    bool isFirstLoginUser = HttpContext.Session.GetString("FirstLoginUser") == "true";

                    if (isFirstLoginUser)
                    {
                        HttpContext.Session.Remove("FirstLoginUser");
                        return RedirectToAction("CreateProfileGeneralGetUser", "UserHome");
                    }
                    else
                    {
                        return RedirectToAction("Index", "UserHome");
                    }
                }
                else
                {
                    // Incorrect password for user
                    ViewBag.ErrorMessage = "Invalid login attempt. Please check your email and password.";
                    return RedirectToAction("Login");
                }
            }

            // If the email doesn't exist in either Customers or Users table, show an error
            ViewBag.ErrorMessage = "Invalid login attempt. Please check your email and password.";
            return RedirectToAction("Login");
        }


        private bool VerifyPassword(string enteredPassword, string storedPassword)
        {
           
            bool passwordsMatch = BCrypt.Net.BCrypt.Verify(enteredPassword, storedPassword);

            return passwordsMatch;
        }
        public async Task<IActionResult> SomeAction()
        {
            // Retrieve the customer's email from the session
            string email = HttpContext.Session.GetString("CustomerEmail");

            // Pass the email value to the view
            ViewBag.CustomerEmail = email;

            // ...
            return View();
        }
        [HttpPost]
        public IActionResult Logout()
        {

            if (HttpContext.Session.Keys.Contains("CustomerId"))
            {
                HttpContext.Session.Remove("CustomerId");
                HttpContext.Session.Remove("CustomerEmail");
            }
            else if (HttpContext.Session.Keys.Contains("UserId"))
            {
                HttpContext.Session.Remove("UserId");
                HttpContext.Session.Remove("UserEmail");
            }

            return RedirectToAction("Index", "UserHome");
        }


        

        // GET: UserAuthController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserAuthController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserAuthController/Create
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

        // GET: UserAuthController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserAuthController/Edit/5
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

        // GET: UserAuthController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserAuthController/Delete/5
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
