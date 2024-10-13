using Microsoft.AspNetCore.Mvc;
using Messager.Data;
using System.Text;
using System.Security.Cryptography;

namespace Messager.Controllers
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User model)
        {
            if (ModelState.IsValid) 
            {
                // Password Hashing
                model.Password = HashPassword(model.Password);

                _context.Users.Add(model);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedbytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedbytes).Replace("-", "").ToLower();
            }
        }
    }
}
