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

        public ActionResult Index(string sortOrder, string searchString)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            ViewBag.MaxStreakSortParm = string.IsNullOrEmpty(sortOrder) ? "maxstreak" : "";
            ViewBag.UserNameSortParm = sortOrder == "username" ? "username_desc" : "username";

            var users = from u in db.Users.OrderByDescending(u => u.WinStreakMax).Take(100) select u;
            //var firstPlace = from u in db.Users.OrderByDescending(u => u.WinStreakMax).Take(1) select u;
            //ViewBag.firstPlace = firstPlace.ToString();

            if (!string.IsNullOrEmpty(searchString))
            {
                users = users.Where(u => u.UserName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "maxstreak":
                    users = users.OrderBy(s => s.WinStreakMax);
                    break;
                case "username":
                    users = users.OrderBy(s => s.UserName);
                    break;
                case "username_desc":
                    users = users.OrderByDescending(s => s.UserName);
                    break;
                default:
                    users = users.OrderByDescending(s => s.WinStreakMax);
                    break;
            }
            return View(users.ToList());
            //IEnumerable<ApplicationUser> allUsers = UserManager.Users.OrderByDescending(u => u.WinStreakMax).Take(100).ToList();
            //return View(allUsers);
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