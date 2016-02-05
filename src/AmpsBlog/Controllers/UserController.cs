using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using AmpsBlog.Models;
using AmpsBlog.ViewModels.Admin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AmpsBlog.Controllers
{
    public class UserController : Controller
    {
        private ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;

        public UserController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: User
        public IActionResult Index()
        {
            var query = from u in _context.Users
                        join ur in _context.UserRoles
                        on u.Id equals ur.UserId
                        join r in _context.Roles
                        on ur.RoleId equals r.Id
                        select new UserViewModel
                        {
                            UserId = u.Id,
                            FirstName = u.FirstName,
                            LastName = u.LastName,
                            Email = u.Email,
                            Photo = u.Photo,
                            Role = r.Name
                        };


            return View(query.ToList());
        }

        // GET: User/Details/5
        public IActionResult Details(string id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var user = from u in _context.Users
                       join ur in _context.UserRoles
                       on u.Id equals ur.UserId
                       join r in _context.Roles
                       on ur.RoleId equals r.Id
                       where u.Id == id
                       select new UserViewModel
                       {
                           UserId = u.Id,
                           FirstName = u.FirstName,
                           LastName = u.LastName,
                           Email = u.Email,
                           Photo = u.Photo,
                           Role = r.Name
                       };


            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user.First());
        }

        //// GET: User/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST: User/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Create(UserViewModel userViewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Users.Add(userViewModel);
        //        _context.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(userViewModel);
        //}

        // GET: User/Edit/5
        public IActionResult Edit(string id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var user = from u in _context.Users
                       join ur in _context.UserRoles
                       on u.Id equals ur.UserId
                       join r in _context.Roles
                       on ur.RoleId equals r.Id
                       where u.Id == id
                       select new UserViewModel
                       {
                           UserId = u.Id,
                           FirstName = u.FirstName,
                           LastName = u.LastName,
                           Email = u.Email,
                           Photo = u.Photo,
                           Role = r.Name
                       };

            
            var obj = from r in _context.Roles
                       select new SelectListItem
                       {
                           Text = r.Name,
                           Value = r.Id,
                           Selected = (r.Name == user.First().Role) ? true : false
                       };


            ViewBag.StateType = obj.ToList();

            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user.First());
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = (from u in _context.Users
                           where u.Id == userViewModel.UserId
                           select u).First();

                //_userManager.UpdateAsync(user.First());
                user.FirstName = userViewModel.FirstName;
                user.LastName = userViewModel.LastName;

                var roles = _userManager.GetRolesAsync(user);

                _context.Update(user);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userViewModel);
        }

        // GET: User/Delete/5
        [ActionName("Delete")]
        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var user = _context.Users.Single(m => m.Id == id);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            var user = _context.Users.Single(m => m.Id == id);
            _context.Users.Remove(user);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
