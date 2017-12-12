﻿using cryptoGamblers.Models;
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
            viewModel.Match = db.Matchdat.FirstOrDefault(m => m.MatchId == Id);
            viewModel.Opponent1 = db.Users.FirstOrDefault(u => u.UserName == viewModel.Match.Opponent1);
            viewModel.Opponent2 = db.Users.FirstOrDefault(u => u.UserName == viewModel.Match.Opponent2);
            viewModel.IsOpponent1 = User.Identity.GetUserName() == viewModel.Opponent1.UserName;

            var userName = User.Identity.GetUserName();

            if (viewModel.Match == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if(viewModel.Match.Opponent1 != userName && viewModel.Match.Opponent2 != userName)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            ViewBag.Name = userName;

            return View(viewModel);
        }

        public JsonResult ProposeBet(int amount, int matchId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            
            //init match info 
            var match = db.Matchdat.FirstOrDefault(m => m.MatchId == matchId);
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

            if (data.BetState == betState.PENDINGBET) { 
            //setprizepool to amount
            data.PrizePool = amount;
                data.BetState = betState.PENDINGBETRESPONSE;
            db.MatchData.AddOrUpdate(data);
                db.Matchdat.AddOrUpdate(match);
            db.SaveChanges();
            }

            //Wait for response to bet
            bool stateChanged = false;
            while (!stateChanged) {
                db = new ApplicationDbContext();
                data = db.MatchData.FirstOrDefault(md => md.MatchId == match.MatchId);
                //Checks what state the matchdata has if the state if different than pending a response should be returned
                switch (data.BetState)
                {
                    case betState.ACCEPTED:

                        stateChanged = true;
                        
                        db.Matchdat.AddOrUpdate(match);
                        db.MatchData.AddOrUpdate(data);
                        db.SaveChanges();

                        return Json(new {
                            Status = "ACC",
                            Amount = data.PrizePool
                        }, JsonRequestBehavior.AllowGet);

                    case betState.DECLINED:
                        stateChanged = true;

                        data.BetState = betState.PENDINGBET;
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
            var match = db.Matchdat.FirstOrDefault(m => m.MatchId == matchId);
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
            var match = db.Matchdat.FirstOrDefault(m => m.MatchId == matchId);
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
                data.BetState = betState.ACCEPTED;
            }
            else {
                data.BetState = betState.DECLINED;
                    data.PrizePool = 0;
            }

            db.MatchData.AddOrUpdate(data);
            db.SaveChanges();

            } catch(Exception e)
            {
                new HttpStatusCodeResult(500);
            }
            return new HttpStatusCodeResult(200);
        }

        public JsonResult RecieveBet(int matchId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            
            //get current user name
            var userName = User.Identity.GetUserName();

            //init match info 
            var match = db.Matchdat.FirstOrDefault(m => m.MatchId == matchId);
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
            var match = db.Matchdat.FirstOrDefault(m => m.MatchId == matchId);
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
            if (data.BetState == betState.ACCEPTED) { 
            Random rng = new Random();
            var roll = rng.Next(1,6);

                var hasOpponentRolled = false;

                if (isOpponent1) {
                    data.Opponent1Roll = roll;
                    db.MatchData.AddOrUpdate(data);
                    db.SaveChanges();

                    while (!hasOpponentRolled)
                    {
                        db = new ApplicationDbContext();
                        data = db.MatchData.FirstOrDefault( d => d.MatchDataId == data.MatchDataId);

                        if (data.Opponent2Roll > 0)
                        {
                            return Json(new {
                                Opponent1Roll = data.Opponent1Roll,
                                Opponent2Roll = data.Opponent2Roll,
                                Winner = data.Opponent1Roll > data.Opponent2Roll ? "Opponent1" : "Opponent2"
                            }, JsonRequestBehavior.AllowGet);
                        }
                        System.Threading.Thread.Sleep(1000);
                    }

                } else {

                    data.Opponent2Roll = roll;
                    data.Opponent1Roll = roll;
                    db.MatchData.AddOrUpdate(data);
                    db.SaveChanges();

                    while (!hasOpponentRolled)
                    {
                        db = new ApplicationDbContext();
                        data = db.MatchData.FirstOrDefault(d => d.MatchDataId == data.MatchDataId);

                        if (data.Opponent1Roll > 0)
                        {
                            hasOpponentRolled = true;
                            return Json(new
                            {
                                Opponent1Roll = data.Opponent1Roll,
                                Opponent2Roll = data.Opponent2Roll,
                                Winner = data.Opponent1Roll > data.Opponent2Roll ? "Opponent1" : "Opponent2"
                            },JsonRequestBehavior.AllowGet);
                        }
                        System.Threading.Thread.Sleep(1000);
                    }

                }

            }
            else{
                return null;
            }
            return Json(new { },JsonRequestBehavior.AllowGet);
        }
    }

}