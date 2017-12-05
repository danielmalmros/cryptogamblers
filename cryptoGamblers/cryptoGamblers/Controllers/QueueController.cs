using cryptoGamblers.Models;
using cryptoGamblers.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cryptoGamblers.Controllers
{
    [Authorize]
    public class QueueController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Queue
        public ActionResult QueueMe() {
            var userName = User.Identity.GetUserName();
            var userId = User.Identity.GetUserId();

            var queue = db.queueIn.FirstOrDefault(u => u.Opponent2 == null);
            bool queueContains = db.queueIn.Any(t => t.Opponent1 != null && t.Opponent2 != null);

            if (queue == null)
            {
                queue = db.queueIn.Create();
                queue.Opponent1 = userName;
                db.queueIn.AddOrUpdate(queue);
            } else
            {
                if (queue.Opponent1 != userName)
                {
                    queue.Opponent2 = userName;
                    db.queueIn.AddOrUpdate(queue);
                }
            }
            db.SaveChanges();

            if (queueContains)
            {
                System.Diagnostics.Debug.WriteLine("WE HAVE A MATCH FUCKERS!");
            }

            return View();
        }
    }
}