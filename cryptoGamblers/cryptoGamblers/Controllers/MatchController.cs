using cryptoGamblers.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cryptoGamblers.Controllers
{
    public class MatchController : AuthorizeController
    {
        // GET: Match
        public ActionResult Index(int Id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var match = db.Match.FirstOrDefault(m => m.MatchId == Id);
            var userName = User.Identity.GetUserName();

            if (match == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if(match.Opponent1 != userName && match.Opponent2 != userName)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            ViewBag.Name = userName;

            return View(match);
        }
    }
}