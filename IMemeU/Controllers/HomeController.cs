using IMemeU.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using IMemeU.Data;
using Microsoft.AspNetCore.Authorization;

namespace IMemeU.Controllers;

public class HomeController(AppDbContext context) : Controller
{
    public IActionResult Index()
    {
        return View();
    }
        
    [Authorize]
    public IActionResult Dashboard()
    {
        return View();
    }
    [Authorize]
    public IActionResult Chat()
    {
        var messages = context.Messages.OrderByDescending(m => m.Timestamp).ToList();
        ViewBag.Messages = messages;
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}