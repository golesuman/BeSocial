﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BlogApp.Models;
using BlogApp.Data;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;
    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _context = context;

        _logger = logger;
    }

    public IActionResult Index()
    {
        var applicationDbContext = _context.Posts.Include(p => p.Category).Include(p => p.User);
        return View(applicationDbContext.ToList());
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}