using BlogApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers;

public class UserController : Controller
{
    private readonly ApplicationDbContext _context;

    public UserController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET

    public IActionResult Details(string id)
    {
        var user = _context.Users.Include(p=>p.Posts).FirstOrDefault(u => u.Id == id);

        if (user == null)
        {
            return NotFound(); // or handle the case when user is not found
        }

        return View(user);
    }
}