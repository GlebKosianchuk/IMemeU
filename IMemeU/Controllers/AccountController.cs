using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using IMemeU.Data;
using System.Text;
using System.Security.Cryptography;
using IMemeU.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace IMemeU.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Register()
        {
            return View();
        }
        
        private bool IsUserNameUnique(string userName)
        {
            return !_context.Users.Any(u => u.UserName == userName);
        }
        
        private async Task SignInAsync(string userName, int userId)
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Register(User model)
        {
            if (ModelState.IsValid)
            {
                if (!IsUserNameUnique(model.UserName))
                {
                    ModelState.AddModelError("UserName", "Данное имя пользователя уже существует");
                    return View(model);
                }
                model.Password = HashPassword(model.Password);

                _context.Users.Add(model);
                await _context.SaveChangesAsync();
                
                await SignInAsync(model.UserName, model.Id);
                
                return RedirectToAction("Dashboard", "Home");
            }
            return View(model);
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == model.Username);

                if (user != null && VerifyPassword(model.Password, user.Password))
                {
                    await SignInAsync(user.UserName, user.Id);
                    
                    return RedirectToAction("Dashboard", "Home");
                } 
                ModelState.AddModelError("", "Неверный логин или пароль");
            }

            return View(model);
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
        
        

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            return HashPassword(password) == hashedPassword;
        }
    }
}