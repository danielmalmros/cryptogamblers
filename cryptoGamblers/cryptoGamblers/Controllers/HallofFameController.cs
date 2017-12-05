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
            ViewBag.Message = "Hall of Fame page";

            return View();
        }
    }
}