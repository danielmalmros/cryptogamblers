using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using cryptoGamblers.Models;
using cryptoGamblers.App_Start;
using Microsoft.AspNet.Identity.Owin;

namespace cryptoGamblers.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
       

        public ActionResult Index()
        {
            IEnumerable<ApplicationUser> allUsers = UserManager.Users.OrderByDescending(u => u.WinStreakMax).Take(3).ToList();
            return View(allUsers);
        }

        private ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }
    }
}