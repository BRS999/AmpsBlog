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
using Microsoft.AspNet.Authorization;

namespace AmpsBlog.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        public RoleManager<IdentityRole> _roleManager { get; set; }

        public UserController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: User
        public IActionResult Index()
        {
            var query = GetUserViewModel();

            return View(query.ToList());
        }

        // GET: User/Details/5
        public IActionResult Details(string id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var user = GetUserViewModel();


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

            var user = GetUserViewModel().Where(x=>x.UserId == id);

            if (user == null)
            {
                return HttpNotFound();
            }

            //ViewBag for Roles Dropdown list
            var obj = from r in _context.Roles
                       select new SelectListItem
                       {
                           Text = r.Name,
                           Value = r.Id,
                           Selected = (r.Name == user.First().Role) ? true : false
                       };
            ViewBag.StateType = obj.ToList();
            
            return View(user.First());
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.FindByIdAsync(userViewModel.UserId);

                user.FirstName = userViewModel.FirstName;
                user.LastName = userViewModel.LastName;
                var existingRole = await _userManager.GetRolesAsync(user);

                if (existingRole[0] != userViewModel.Role)
                {
                    var newRole = await _roleManager.FindByIdAsync(userViewModel.RoleId);

                    await _userManager.RemoveFromRoleAsync(user, existingRole[0]);
                    await _userManager.AddToRoleAsync(user, newRole.Name);
                }

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

        public IQueryable<UserViewModel> GetUserViewModel()
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
                            Role = r.Name,
                            RoleId = r.Id
                        };

            return query;
        }
    }
}
