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

namespace Forum.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CommentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Comments/Create
        public IActionResult Create(int DiscussionId)
        {
            ViewBag.UserId = GetUserID();
            ViewBag.DiscussionId = DiscussionId;
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,Date,UserId,DiscussionId")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                //_context.Add(comment);
                var discussion = _context.Discussions.Find(comment.DiscussionId);
                discussion.Comments.Add(comment);
                _context.Update(discussion);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Discussions", new { id = comment.DiscussionId });
            }
            ViewBag.UserId = GetUserID();
            ViewBag.DiscussionId = Request.Form["DiscussionId"];
            return View(comment);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var comment = await _context.Comments.FindAsync(id);
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Discussions", new { id = comment.DiscussionId });
        }

        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }
        private bool IsAuth() => HttpContext.User.Identity.IsAuthenticated;
        private string GetUserID() => HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
