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
        public Models.ActionResult<Match> QueueMe() {
            var userName = User.Identity.GetUserName();
            var userId = User.Identity.GetUserId();

            var queue = db.queueIn.FirstOrDefault(u => u.Opponent2 == null);
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

            bool queueContains = db.queueIn.Any(t => t.Opponent1 != null && t.Opponent2 != null);
            Match newMatch = null;

            if (queueContains)
            {
                QueueIn queueData = db.queueIn.Where(x => x.Opponent1 == userName || x.Opponent2 == userName).Select(x => x).FirstOrDefault();
                 newMatch = new Match { Opponent1 = queueData.Opponent1, Opponent2 = queueData.Opponent2 };
                db.Match.AddOrUpdate(newMatch);

                newMatch = db.Match.FirstOrDefault(x => x.Opponent1 == newMatch.Opponent1 && x.Opponent2 == newMatch.Opponent2);


                db.queueIn.Remove(queue);

                db.SaveChanges();
            }

            return  new Models.ActionResult<Match>{
                Object = newMatch,
                Status = new HttpStatusCodeResult(200)
            };
        }
    }
}