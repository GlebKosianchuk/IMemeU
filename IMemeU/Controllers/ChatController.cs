using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IMemeU.Data;
using IMemeU.Models;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace IMemeU.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly AppDbContext _context;

        public ChatController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(int receiverId, string content)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var message = new Message
            {
                Content = content,
                SentAt = DateTime.UtcNow.AddHours(3),
                SenderId = userId,
                ReceiverId = receiverId
            };
            
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return RedirectToAction("Chat", new { selectedUserId = receiverId });
        }

        public async Task<IActionResult> Chat(int? selectedUserId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var users = await _context.Users.Where(u => u.Id != userId).ToListAsync();

                if (selectedUserId.HasValue)
                {
                    var messages = await _context.Messages
                        .Where(m => (m.SenderId == userId && m.ReceiverId == selectedUserId) || 
                                    (m.SenderId == selectedUserId && m.ReceiverId == userId))
                        .OrderBy(m => m.SentAt)
                        .Include(m => m.Sender)
                        .Include(m => m.Receiver)
                        .ToListAsync();
                    
                    ViewBag.SelectedUserId = selectedUserId;
                    ViewBag.Messages = messages;
                    return View(users);
                }
            return View(users);
        }
    }
}