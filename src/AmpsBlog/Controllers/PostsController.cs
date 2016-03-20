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

        // GET: Posts/List
        [Authorize(Roles = "Author")]
        public async Task<IActionResult> List()
        {
            var applicationDbContext = _context.Posts.Include(p => p.Author).Include(p => p.Blog).Include(p => p.PostStatus);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Posts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Posts.Include(p => p.Author).Include(p => p.Blog).Include(p => p.PostStatus);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Posts/id/5
        [Route("Posts/id/{id?}")]
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
            var date = post.DateCreated.ToLocalTime();
            post.DateCreated = date;
            return View(post);
        }

        [Route("Posts/{perma}")]
        public async Task<IActionResult> Details(string perma)
        {
            if (perma == null)
            {
                return HttpNotFound();
            }

            Post post = await _context.Posts.Include(p => p.PostStatus).Include(p => p.Author).SingleAsync(m => m.Permalink == perma);
            if (post == null)
            {
                return HttpNotFound();
            }
            var date = post.DateCreated.ToLocalTime();
            post.DateCreated = date;
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
        [Authorize(Roles = "Author")]
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

            EditPostViewModel editPost = new EditPostViewModel
            {
                Title = post.Title,
                Content = post.Content,
                Permalink = post.Permalink,
                DateCreated = post.DateCreated,
                Tags = post.Tags,
                Blog = post.Blog.Id,
                PostId = post.PostId,
                Author = post.Author.Id,
                PostStatus = post.PostStatus.Id
            };

            ViewBag.PostStatus = new SelectList(_context.PostStatuses, "Id", "Status", post.PostStatus.Id);
            ViewBag.BlogId = new SelectList(_context.Blogs, "Id", "Name", post.Blog.Id);

            return View(editPost);
        }

        // POST: Posts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditPostViewModel editPost)
        {
            if (ModelState.IsValid)
            {
                var currentuser = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
                Post post = new Post {
                    PostId = editPost.PostId,
                    Title = editPost.Title,
                    Content = editPost.Content,
                    Permalink = editPost.Permalink,
                    DateCreated = editPost.DateCreated,
                    Tags = editPost.Tags,
                    Author = currentuser,
                    Blog = _context.Blogs.First(x=>x.Id == editPost.Blog),
                    PostStatus = _context.PostStatuses.First(x=>x.Id == editPost.PostStatus)
                };

                _context.Update(post);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.PostStatus = new SelectList(_context.PostStatuses, "Id", "Status", editPost.PostStatus);
            ViewBag.BlogId = new SelectList(_context.Blogs, "Id", "Name", editPost.Blog);
            return View(editPost);
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
