using cryptoGamblers.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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
            var viewModel = new MatchViewModel();
            //init match info 

            viewModel.Match = db.Match.OrderByDescending(m => m.Date).FirstOrDefault(m => m.MatchId == Id);
            var userName = User.Identity.GetUserName();

            if (viewModel.Match == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (viewModel.Match.Opponent1 != userName && viewModel.Match.Opponent2 != userName)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            viewModel.Opponent1 = db.Users.FirstOrDefault(u => u.UserName == viewModel.Match.Opponent1);
            viewModel.Opponent2 = db.Users.FirstOrDefault(u => u.UserName == viewModel.Match.Opponent2);
            viewModel.IsOpponent1 = User.Identity.GetUserName() == viewModel.Opponent1.UserName;
     


            ViewBag.Name = userName;

            return View(viewModel);
        }

        public JsonResult ProposeBet(int amount, int matchId)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            //init match info 
            var match = db.Match.FirstOrDefault(m => m.MatchId == matchId);
            var data = db.MatchData.FirstOrDefault(d => d.MatchId == matchId);

            var userName = User.Identity.GetUserName();

            if (match == null && amount > 0)
            {
                return null;
            }
            else
            {
                if (match.Opponent1 != userName)
                {
                    return null;
                }
            }

            if (data.MatchState == MatchState.PENDINGBET) {
                //setprizepool to amount
                data.PrizePool = amount;
                data.MatchState = MatchState.PENDINGBETRESPONSE;
                db.MatchData.AddOrUpdate(data);
                db.Match.AddOrUpdate(match);
                db.SaveChanges();
            }

            //Wait for response to bet
            bool stateChanged = false;
            while (!stateChanged) {
                db = new ApplicationDbContext();
                data = db.MatchData.FirstOrDefault(md => md.MatchId == match.MatchId);
                //Checks what state the matchdata has if the state if different than pending a response should be returned
                switch (data.MatchState)
                {
                    case MatchState.ACCEPTED:

                        stateChanged = true;

                        db.Match.AddOrUpdate(match);
                        db.MatchData.AddOrUpdate(data);
                        db.SaveChanges();

                        return Json(new {
                            Status = "ACC",
                            Amount = data.PrizePool
                        }, JsonRequestBehavior.AllowGet);

                    case MatchState.DECLINED:
                        stateChanged = true;

                        data.MatchState = MatchState.PENDINGBET;
                        data.PrizePool = 0;
                        db.MatchData.AddOrUpdate(data);
                        db.SaveChanges();

                        return Json(new
                        {
                            Status = "DEC"
                        }, JsonRequestBehavior.AllowGet);

                    default: //Pending
                        //Sleep 1 second
                        System.Threading.Thread.Sleep(1000);
                        break;
                }
            }
            return new JsonResult();
        }

        public JsonResult checkMatchStatus(int matchId)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            //init match info 
            var match = db.Match.FirstOrDefault(m => m.MatchId == matchId);
            var data = db.MatchData.FirstOrDefault(d => d.MatchId == matchId);

            var userName = User.Identity.GetUserName();

            if (match == null)
            {
                return null;
            }
            else
            {
                if (match.Opponent1 != userName && match.Opponent2 != userName)
                {
                    return null;
                }
            }
            return Json(new {
                Match = match,
                Data = data
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RespondBet(bool accepted, int matchId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            try {
                //init match info 
                var match = db.Match.FirstOrDefault(m => m.MatchId == matchId);
                var data = db.MatchData.FirstOrDefault(d => d.MatchId == matchId);

                var userName = User.Identity.GetUserName();

                if (match == null)
                {
                    return null;
                }
                else
                {
                    if (match.Opponent2 != userName)
                    {
                        return null;
                    }
                }
                if (accepted) {
                    data.MatchState = MatchState.ACCEPTED;
                }
                else {
                    data.MatchState = MatchState.DECLINED;
                    data.PrizePool = 0;
                }

                db.MatchData.AddOrUpdate(data);
                db.SaveChanges();

            } catch (Exception e)
            {
                new HttpStatusCodeResult(500);
            }
            return new HttpStatusCodeResult(200);
        }
        public HttpStatusCodeResult ResetRoll(int matchId)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            //get current user name
            var userName = User.Identity.GetUserName();

            //init match info 
            var match = db.Match.FirstOrDefault(m => m.MatchId == matchId);
            var data = db.MatchData.FirstOrDefault(d => d.MatchId == matchId);
            if (match == null)
            {
                return new HttpStatusCodeResult(403);
            }
            else
            {
                if (match.Opponent2 != userName && match.Opponent1 != userName)
                {
                    return new HttpStatusCodeResult(403);
                }
            }

            //reset rolls and state
            data.Opponent1Roll = 0;
            data.Opponent2Roll = 0;
            data.MatchState = MatchState.ACCEPTED;
            db.MatchData.AddOrUpdate(data);
            db.SaveChanges();

            return new HttpStatusCodeResult(200);
        }


        public HttpStatusCodeResult TransferMatch(int matchId)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            //get current user name
            var userName = User.Identity.GetUserName();

            //init match info 
            var match = db.Match.FirstOrDefault(m => m.MatchId == matchId);
            var data = db.MatchData.FirstOrDefault(d => d.MatchId == matchId);
            if (match == null)
            {
                return new HttpStatusCodeResult(200);
            }
            else
            {
                if (match.Opponent2 != userName && match.Opponent1 != userName && data.MatchState == MatchState.FINISHED)
                {
                    return new HttpStatusCodeResult(403);
                }
            }
            try
            {
                //reset rolls and state
                db.AfterMatch.AddOrUpdate(new AfterMatch
                {
                    GameDate = match.Date,
                });
                db.Match.Remove(match);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                //log e.message
                return new HttpStatusCodeResult(500);
            }

            return new HttpStatusCodeResult(200);
        }

        public JsonResult RecieveBet(int matchId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            
            //get current user name
            var userName = User.Identity.GetUserName();

            //init match info 
            var match = db.Match.FirstOrDefault(m => m.MatchId == matchId);
            var data = db.MatchData.FirstOrDefault(d => d.MatchId == matchId);

            if (match == null)
            {
                return null;
            }
            else
            {
                if (match.Opponent2 != userName)
                {
                    return null;
                }
            }

            //wait for bet
            bool betRecieved = false;
            while (!betRecieved)
            {
                db = new ApplicationDbContext();
                data = db.MatchData.FirstOrDefault(md => md.MatchId == match.MatchId);
                if (data.PrizePool > 0)
                {
                    betRecieved = true;
                    return Json(new{
                        Amount = data.PrizePool.ToString()
                    },JsonRequestBehavior.AllowGet);
                }
                System.Threading.Thread.Sleep(1000);
            }
            return new JsonResult();
        }

        public JsonResult Roll(int matchId) {
            ApplicationDbContext db = new ApplicationDbContext();

            //init match info 
            var match = db.Match.FirstOrDefault(m => m.MatchId == matchId);
            var data = db.MatchData.FirstOrDefault(d => d.MatchId == matchId);
          

            var userName = User.Identity.GetUserName();
            var isOpponent1 = match.Opponent1 == userName;

            if (match == null)
            {
                return null;
            }
            else
            {
                if (match.Opponent1 != userName && match.Opponent2 != userName)
                {
                    return null;
                }
            }

            if (data.MatchState == MatchState.ACCEPTED) { 
            Random rng = new Random();
          //  var roll = rng.Next(1,6);

                var hasOpponentRolled = false;

                if (isOpponent1) {

                    db.MatchData.AddOrUpdate(data);
					data.Opponent1Roll = 1;
                   // data.Opponent1Roll = rng.Next(1, 6);
					db.SaveChanges();

                    while (!hasOpponentRolled)
                    {
                        db = new ApplicationDbContext();
                        data = db.MatchData.FirstOrDefault( d => d.MatchDataId == data.MatchDataId);

                        if (data.Opponent2Roll > 0)
                        {
                            hasOpponentRolled = true;
                            data.MatchState = MatchState.FINISHED;
                            db.MatchData.AddOrUpdate(data);
                            db.SaveChanges();

                            if (data.Opponent1Roll == data.Opponent2Roll)
                            {
                                return Json(new
                                {
                                    Opponent1Roll = data.Opponent1Roll,
                                    Opponent2Roll = data.Opponent2Roll,
                                    Winner = "None"
                                }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                transferCurrency(match.MatchId);
                            }
                        }
                        System.Threading.Thread.Sleep(1000);
                    }

                } else {

					//data.Opponent2Roll = rng.Next(1, 6);
					data.Opponent2Roll = 1;
					db.MatchData.AddOrUpdate(data);
                    db.SaveChanges();

                    while (!hasOpponentRolled)
                    {
                        db = new ApplicationDbContext();
                        data = db.MatchData.FirstOrDefault(d => d.MatchDataId == data.MatchDataId);

                        if (data.Opponent1Roll > 0)
                        {
                            hasOpponentRolled = true;

                            if (data.Opponent1Roll == data.Opponent2Roll)
                            {
                                return Json(new
                                {
                                    Opponent1Roll = data.Opponent1Roll,
                                    Opponent2Roll = data.Opponent2Roll,
                                    Winner = "None"
                                }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        System.Threading.Thread.Sleep(1000);
                    }

                }

            }
            else{
                return null;
            }

            return Json(new
            {
                Opponent1Roll = data.Opponent1Roll,
                Opponent2Roll = data.Opponent2Roll,
                Winner = data.Opponent1Roll > data.Opponent2Roll ? match.Opponent1 : match.Opponent2
            }, JsonRequestBehavior.AllowGet);
        }

        private void transferCurrency(int matchId)
        {

            //transfer money from one account to another
            ApplicationDbContext db = new ApplicationDbContext();

            //init match info 
            var match = db.Match.FirstOrDefault(m => m.MatchId == matchId);
            var data = db.MatchData.FirstOrDefault(d => d.MatchId == matchId);


            var userName = User.Identity.GetUserName();
            var isOpponent1 = match.Opponent1 == userName;

            if (match == null)
            {
                return;

            }
            else
            {
                if (match.Opponent1 != userName && match.Opponent2 != userName)
                {
                    return;
                }
            }

            if (data.MatchState == MatchState.FINISHED)
            {
               var winner = data.Opponent1Roll > data.Opponent2Roll ?  db.Users.FirstOrDefault(u => u.UserName == match.Opponent1) : db.Users.FirstOrDefault(u => u.UserName == match.Opponent2);
               var looser = data.Opponent1Roll < data.Opponent2Roll ? db.Users.FirstOrDefault(u => u.UserName == match.Opponent1) : db.Users.FirstOrDefault(u => u.UserName == match.Opponent2);
                
                winner.Balance = winner.Balance + data.PrizePool;
                looser.Balance = looser.Balance - data.PrizePool;
                //update winstreak
                winner.WinStreak = winner.WinStreak + 1;
                winner.WinStreakMax = winner.WinStreak >= winner.WinStreakMax ? winner.WinStreak : winner.WinStreakMax;

                looser.WinStreak = 0;

                db.Users.AddOrUpdate(winner);
                db.Users.AddOrUpdate(looser);
                db.SaveChanges();
            }
        }
    }

}