using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Forum.Data;
using Forum.Models;
using System.Security.Claims;

namespace ForumApp.Controllers
{
    public class DiscussionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DiscussionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Discussions
        public async Task<IActionResult> Index()
        {
            ViewBag.Title = "Discussions";
            ViewBag.IsAuth = IsAuth();
            return View(await _context.Discussions.Include(d => d.User).ToListAsync());
        }

        // GET: Discussions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discussion = await _context.Discussions.Include(d => d.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (discussion == null)
            {
                return NotFound();
            }

            ViewBag.IsAuth = IsAuth();
            ViewBag.Comments = await _context.Comments.Include(c => c.User)
                .Where(c => c.DiscussionId == id).ToListAsync();
            ViewBag.UserId = GetUserID();
            return View(discussion);
        }

        // GET: Discussions/Create
        public IActionResult Create()
        {
            ViewBag.UserId = GetUserID();
            return View();
        }

        // POST: Discussions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Topic,Description,Date,UserId")] Discussion discussion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(discussion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.UserId = GetUserID();
            return View(discussion);
        }

        public async Task<IActionResult> MyDiscussions()
        {
            ViewBag.Title = "My Discussions";
            ViewBag.IsAuth = IsAuth();
            return View("~/Views/Discussions/Index.cshtml", await _context.Discussions.Include(d => d.User)
                .Where(d => d.UserId == GetUserID()).ToListAsync());
        }
        private bool DiscussionExists(int id)
        {
            return _context.Discussions.Any(e => e.Id == id);
        }
        private bool IsAuth() => HttpContext.User.Identity.IsAuthenticated;
        private string GetUserID() => HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
