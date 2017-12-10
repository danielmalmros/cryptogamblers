using cryptoGamblers.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cryptoGamblers.Controllers
{
    public class HallofFameController : Controller
    {
        // GET: HallofFame

        public ActionResult Index()
        {
            IEnumerable<ApplicationUser> allUsers = UserManager.Users.OrderByDescending(u => u.WinStreakMax).Take(100).ToList();
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