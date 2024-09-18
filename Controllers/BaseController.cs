using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EduCrew.Controllers
{
    public class BaseController : Controller
    {
     
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = userManager.FindById(userId);

                // Set the profile image URL in the ViewBag
                ViewBag.ProfileImage = user?.ProfileImage;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}