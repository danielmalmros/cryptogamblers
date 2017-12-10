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
   
    public class QueueController : AuthorizeController
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Queue
        public JsonResult QueueMe() {

            var userName = User.Identity.GetUserName();
            var userId = User.Identity.GetUserId();
            var response = new Models.ActionResult<Match>();

            var queue = db.queueIn.FirstOrDefault(u => u.Opponent2 == null);

            if (queue == null)
            {
                queue = db.queueIn.Create();
                queue.Opponent1 = userName;
                db.queueIn.AddOrUpdate(queue);
                db.SaveChanges();
            }
            else
            {

                if (queue.Opponent1 != userName)
                {
                    queue.Opponent2 = userName;
                    db.queueIn.AddOrUpdate(queue);
                    db.SaveChanges();
                    bool MatchTransfered = false;
                    //get newly created match
                    while (!MatchTransfered)
                    {
                        //re-init application context so data is updated
                        db = new ApplicationDbContext();
                        var matchCreated = db.Match.FirstOrDefault(u => u.Opponent2 == userName && u.Opponent1 == queue.Opponent1);

                        if (matchCreated != null)
                        {
                            MatchTransfered = true;
                            
                             response = new Models.ActionResult<Match>
                            {
                                Object = matchCreated,
                                Status = new HttpStatusCodeResult(200)
                            };
                            return Json(response, JsonRequestBehavior.AllowGet);

                        }
                        else
                        {
                            System.Threading.Thread.Sleep(1000);
                        }
                    }
                }
            }

            //Waiting for opponent
            bool OpponentFound = false;
                while (!OpponentFound)
                {
                //re-init application context so data is updated
                db = new ApplicationDbContext();
                queue = db.queueIn.FirstOrDefault(u => u.Opponent1 == userName);
                    if (!string.IsNullOrEmpty(queue.Opponent2))
                    {
                        OpponentFound = true;
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(1000);
                    }
            }

            bool queueContains = db.queueIn.Any(t => t.Opponent1 != null && t.Opponent2 != null && t.Opponent1 == userName);
            
            //transfer from queue to actual match
            Match newMatch = null;
            if (queueContains)
            {
                QueueIn queueData = db.queueIn.Where(x => x.Opponent1 == userName || x.Opponent2 == userName).Select(x => x).FirstOrDefault();
                newMatch = new Match { Opponent1 = queueData.Opponent1, Opponent2 = queueData.Opponent2, Date = DateTime.Now };
                db.Match.AddOrUpdate(newMatch);
				db.SaveChanges();
                newMatch = db.Match.FirstOrDefault(x => x.Opponent1 == newMatch.Opponent1 && x.Opponent2 == newMatch.Opponent2);

                db.queueIn.Remove(queue);
                db.SaveChanges();
            }

            response = new Models.ActionResult<Match>
            {
                Object = newMatch,
                Status = new HttpStatusCodeResult(200)
            };

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        // GET: Match
        public ActionResult Match()
        {
            return View();
        }

        // POST: Match
        [HttpPost]
        public ActionResult Match(FormCollection fc)
        {
            Random rnd = new Random();
            double i = rnd.NextDouble() * 6;
            ViewBag.Face = i;

            return View();
        }
    }
}