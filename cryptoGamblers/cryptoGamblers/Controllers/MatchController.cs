using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cryptoGamblers.Controllers
{
    public class MatchController : Controller
    {
        // GET: Match
        public ActionResult Index(int matchId)
        {
            return View();
        }
    }
}