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
            

            //var service = new QueueService();
            //service.findOpponent(userName);

            var queue = db.queueIn.FirstOrDefault(d => d.Opponent1 == userName);

            if (queue == null)
            {
                System.Diagnostics.Debug.WriteLine(userName.ToString());
                queue = db.queueIn.Create();
                queue.Opponent1 = userName;
                
            } else
            {
                queue = db.queueIn.FirstOrDefault(d => d.Opponent2 == userName);




                //db.queueIn.Attach(queue);
                //db.Entry(queue).CurrentValues.SetValues(userName);

                if (queue == null)
                {
                    //queue = db.queueIn.Find(userId);
                    queue = db.queueIn.Create();
                    queue.Opponent2 = userName;
                }
                //db.queueIn.AddOrUpdate(queue);



            }
            db.queueIn.AddOrUpdate(queue);
            db.SaveChanges();
            return View();
            
        }
    }
}