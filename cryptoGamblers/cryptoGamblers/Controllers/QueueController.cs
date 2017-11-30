using cryptoGamblers.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cryptoGamblers.Controllers
{
    public class QueueController : Controller
    {
        

        public QueueController()
        {

        }
        // GET: Queue
        public ActionResult QueueMe() {
            var userName = this.User.Identity.Name;

            var service = new QueueService();
            service.findOpponent(userName);
            
            return View();
        }
    }
}