using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using IMemeU.Data;
using System.Text;
using System.Security.Cryptography;
using IMemeU.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace IMemeU.Controllers
{
    public class AccountController(AppDbContext context) : Controller
    {
        public IActionResult Register()
        {
            return View();
        }
        private bool IsUserNameUnique(string userName)
        {
            return !context.Users.Any(u => u.UserName == userName);
        }
        
        private async Task SignInAsync(string userName, int userId)
        {
            var claims = new Claim[]
            {
                new (ClaimTypes.Name, userName),
                new (ClaimTypes.NameIdentifier, userId.ToString())
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

                context.Users.Add(model);
                await context.SaveChangesAsync();
                
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
            if (!ModelState.IsValid) return View(model);
            
            var user = await context.Users.FirstOrDefaultAsync(u => u.UserName == model.Username);

            if (user != null && VerifyPassword(model.Password, user.Password))
            {
                await SignInAsync(user.UserName, user.Id);
                    
                return RedirectToAction("Dashboard", "Home");
            }
            ModelState.AddModelError("", "Неверный логин или пароль");

            return View(model);
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        private static string HashPassword(string password)
        {
            var hashedBytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }

        private static bool VerifyPassword(string password, string hashedPassword)
        {
            return HashPassword(password) == hashedPassword;
        }
    }
}