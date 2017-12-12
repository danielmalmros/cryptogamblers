using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using cryptoGamblers.Models;
using Microsoft.AspNet.Identity.Owin;

namespace cryptoGamblers.Controllers
{
    
    public class HomeController : AuthorizeController
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