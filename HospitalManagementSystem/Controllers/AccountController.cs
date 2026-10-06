using BCrypt.Net;
using HospitalManagementSystem.Data;
using HospitalManagementSystem.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HospitalManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Displays the Login Page
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Processes the Login Attempt
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Username and password are required.";
                return View();
            }

            var user = _context.Users.FirstOrDefault(u => u.Username == username);

            // Verify user exists and password matches the hash
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                // Create the security claims (identity card) for the user
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserID),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                // Sign the user in and issue the cookie
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                TempData["LoginSuccess"] = $"Welcome back, {user.Username}!";
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Invalid username or password. Please try again.";
            return View();
        }

        // GET: Logs the user out
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["LogoutMessage"] = "You have been logged out successfully.";
            return RedirectToAction("Login");
        }

        // GET: Displays the Registration Page
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: Processes the Registration Attempt
        [HttpPost]
        public async Task<IActionResult> Register(string username, string password, string confirmPassword, string role)
        {
            // 1. Validate inputs
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Username and password are required.";
                return View();
            }

            // 2. Block Admin role from public self-registration
            if (role == "Admin")
            {
                ViewBag.Error = "Administrator accounts cannot be created from this page. Please contact your system administrator.";
                return View();
            }

            // 3. Password confirmation check
            if (password != confirmPassword)
            {
                ViewBag.Error = "Passwords do not match. Please try again.";
                return View();
            }

            // 4. Minimum password length
            if (password.Length < 6)
            {
                ViewBag.Error = "Password must be at least 6 characters long.";
                return View();
            }

            // 5. Check if the username already exists to prevent duplicates
            if (_context.Users.Any(u => u.Username == username))
            {
                ViewBag.Error = "Username is already taken. Please choose another.";
                return View();
            }

            // 6. Hash the password using BCrypt for security
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            // 7. Create the base User account
            var newUser = new User
            {
                Username = username,
                PasswordHash = hashedPassword,
                Role = role,
                CreatedAt = DateTime.Now
            };

            _context.Users.Add(newUser);

            // 8. Generate the specific role profile to satisfy our One-to-One database relationship
            if (role == "Patient")
            {
                var newPatient = new Patient
                {
                    UserID = newUser.UserID,
                    FirstName = "Pending",
                    LastName = "Details",
                    RegisteredAt = DateTime.Now
                };
                _context.Patients.Add(newPatient);
            }
            else if (role == "Doctor")
            {
                var newDoctor = new Doctor
                {
                    UserID = newUser.UserID,
                    FirstName = "Pending",
                    LastName = "Details"
                };
                _context.Doctors.Add(newDoctor);
            }

            // 9. Save all changes to the SQL Server database
            await _context.SaveChangesAsync();

            // 10. Show success alert on Login page
            TempData["RegisterSuccess"] = $"Account created successfully! Welcome, {username}. Please sign in.";
            return RedirectToAction("Login");
        }

        // GET: Access Denied Page
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}