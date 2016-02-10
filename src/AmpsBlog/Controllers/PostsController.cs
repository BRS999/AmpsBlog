using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using AmpsBlog.Models;
using System;
using Microsoft.AspNet.Identity;
using AmpsBlog.ViewModels.Post;
using Microsoft.AspNet.Authorization;

namespace AmpsBlog.Controllers
{
    public class PostsController : Controller
    {
        private ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;

        public PostsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;    
        }

        // GET: Posts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Posts.Include(p => p.Author).Include(p => p.Blog).Include(p => p.PostStatus);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Post post = await _context.Posts.Include(p => p.PostStatus).Include(p => p.Author).SingleAsync(m => m.PostId == id);
            if (post == null)
            {
                return HttpNotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        [Authorize(Roles = "Author")]
        public IActionResult Create()
        {
            var authors = from a in _context.Users
                          join p in _context.UserRoles
                          on a.Id equals p.UserId
                          join r in _context.Roles
                          on p.RoleId equals r.Id
                          where r.Name == "Author"
                          select a;

            ViewBag.PostStatus = new SelectList(_context.PostStatuses, "Id", "Status");
            ViewBag.AuthorId = new SelectList(authors, "Id", "FullName");
            ViewBag.BlogId = new SelectList(_context.Blogs, "Id", "Name");
            return View();
        }

        // POST: Posts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePostViewModel model)
        {
            //var errors = ModelState.Values.SelectMany(v => v.Errors);

            if (ModelState.IsValid)
            {
                Post post = new Post
                {
                    Title = model.Title,
                    Content = model.Content,
                    Permalink = model.Permalink,
                    Tags = model.Tags,
                    DateCreated = DateTime.UtcNow,
                    Author = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name),
                    Blog = _context.Blogs.Where(x => x.Id == model.Blog).SingleOrDefault(),
                    PostStatus = _context.PostStatuses.Where(x => x.Id == model.PostStatus).SingleOrDefault()
                };
                

                _context.Posts.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.PostStatus = new SelectList(_context.PostStatuses, "Id", "Status");
            ViewBag.BlogId = new SelectList(_context.Blogs, "Id", "Name");
            return View(model);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Post post = await _context.Posts.Include(p => p.PostStatus).Include(p => p.Blog).SingleAsync(m => m.PostId == id);
            if (post == null)
            {
                return HttpNotFound();
            }

            ViewBag.PostStatus = new SelectList(_context.PostStatuses, "Id", "Status", post.PostStatus.Id);
            ViewBag.BlogId = new SelectList(_context.Blogs, "Id", "Name", post.Blog.Id);

            return View(post);
        }

        // POST: Posts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Post post)
        {
            if (ModelState.IsValid)
            {
                var currentuser = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
                //post.Author = currentuser.Id;

                _context.Update(post);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            //ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Author", post.AuthorId);
            //ViewData["BlogId"] = new SelectList(_context.Blogs, "BlogId", "Blog", post.BlogId);
            return View(post);
        }

        // GET: Posts/Delete/5
        [ActionName("Delete")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Post post = await _context.Posts.SingleAsync(m => m.PostId == id);
            if (post == null)
            {
                return HttpNotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Post post = await _context.Posts.SingleAsync(m => m.PostId == id);
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
