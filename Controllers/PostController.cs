using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogApp.Data;
using BlogApp.Models;
using Microsoft.AspNetCore.Authorization;
using BlogApp.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BlogApp.Views
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _host;
        // private IHostingEnvironment Environment;
        private readonly UserManager<ApplicationUser> _userManager;
        public PostController(ApplicationDbContext context, IWebHostEnvironment host, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _host = host;
            _context = context;
        }

        // GET: Post
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Posts.Include(p => p.Category).Include(p => p.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Post/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Category)
                .Include(p => p.User).Include(p => p.Comments)
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Post/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId");
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id");
            return View();
        }

        // POST: Post/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostViewModel post)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId
            string uniqueFileName = UploadedFile(post);
            Post newPost = new()
            {
                Title = post.Title,
                Content = post.Content,
                PostImage = uniqueFileName,
                UserId = userId,
                CategoryId = post.CategoryId,
                PublishedAt = DateTime.Now,

            };
            if (post != null)
            {
                _context.Add(newPost);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", post.CategoryId);
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", post.UserId);
            return View(post);
        }

        // GET: Post/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", post.CategoryId);
            return View(post);
        }

        // POST: Post/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PostId,Title,Content,PublishedAt,UserId,CategoryId")] Post post)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId

            // to get current user info
            var user = await _userManager.FindByIdAsync(userId);
            if (post.UserId != user.Id)
            {
                return Forbid();
            }
            // if (currentUser.Identity.Name == post.User.Id)
            if (id != post.PostId)
            {
                return NotFound();
            }

            if (post != null)
            {
                try
                {
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.PostId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", post.CategoryId);
            // ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", post.UserId);
            return View(post);
        }

        // GET: Post/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Category)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Post/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Posts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Posts'  is null.");
            }
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return (_context.Posts?.Any(e => e.PostId == id)).GetValueOrDefault();
        }

        private string UploadedFile(PostViewModel model)
        {
            string uniqueFileName = null;

            if (model.PostImage != null)
            {
                string uploadsFolder = Path.Combine(_host.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.PostImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.PostImage.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }
}
