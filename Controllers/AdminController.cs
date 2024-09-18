using EduCrew.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EduCrew.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ApplicationDbContext _context;
        private ApplicationUserManager _userManager;

        public AdminController(ApplicationUserManager userManager)
        {
            UserManager = userManager;

        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public AdminController()
        {
            _context = new ApplicationDbContext();
        }


        public ActionResult AdminPanel()
        {
            var model = _context.Posts
                .Include("User")
                .Include("Category")
                .Select(item => new PostDTO
                {
                    PostId = item.PostId,
                    Title = item.Title,
                    Description = item.Content,
                    CreatedDate = item.DatePosted,
                    UserId = item.User.Id,
                    UserName = item.User.UserName,
                    Category = item.Category.Name
                }).ToList();

            return View(model);
        }

        public ActionResult ManageRoles()
        {
            var currentUserId = User.Identity.GetUserId();  


            var users = _context.Users
                .Where(u => u.Id != currentUserId)
                .ToList();

            var roles = _context.Roles.ToList();  

           
            var userRoles = users.Select(user => new UserRolesViewModel
            {
                User = user,
                Roles = UserManager.GetRoles(user.Id) // Get roles for the user
            }).ToList();

            ViewBag.Roles = roles; // Pass all roles to ViewBag for the dropdown
            return View(userRoles); // Return the userRoles list
        }


        [HttpPost]
        public ActionResult AssignRole(string userId, string roleName)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
            userManager.AddToRole(userId, roleName);
            return RedirectToAction("ManageRoles");
        }

        [HttpPost]
        public ActionResult RemoveRole(string userId, string roleName)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
            if (userManager.IsInRole(userId, roleName))
            {
                userManager.RemoveFromRole(userId, roleName);
            }
            return RedirectToAction("ManageRoles");
        }
    }
}
