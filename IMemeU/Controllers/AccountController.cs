using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using IMemeU.Data;
using System.Text;
using System.Security.Cryptography;
using IMemeU.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace IMemeU.Controllers;

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
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            if (!IsUserNameUnique(model.UserName))
            {
                ModelState.AddModelError("UserName", "Данное имя пользователя уже существует");
                return View(model);
            }

            var dbUser = new User
            {
                UserName = model.UserName,
                Password = HashPassword(model.Password),
            };

            context.Users.Add(dbUser);
            await context.SaveChangesAsync();

            await SignInAsync(dbUser.UserName, dbUser.Id);

            return RedirectToAction("Chat", "Home");
        }
        return View(model);
    }

    public class MessageController : Controller
    {
        private readonly AppDbContext _context;

        public MessageController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> PostMessage([FromBody] string Text)
        {
            if (string.IsNullOrEmpty(Text))
            {
                return BadRequest("Message text is required.");
            }
            var msg = new MessageViewModel()
            {
                UserName = User.Identity.Name,
                Text = Text,
                Timestamp = DateTime.UtcNow
            };

            _context.Messages.Add(msg);
            await _context.SaveChangesAsync();

            return Ok();
        }
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
                    
            return RedirectToAction("Chat", "Home");
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