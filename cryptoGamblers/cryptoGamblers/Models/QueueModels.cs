using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using cryptoGamblers.Models;

namespace cryptoGamblers.Models
{
    public class QueueIn
    {
        public int Id { get; set; }
        public string Opponent1 { get; set; }
        public string Opponent2 { get; set; }
    }
    public class Match
    {
        public int MatchId { get; set; }
        public string Opponent1 { get; set; }
        public string Opponent2 { get; set; }
    }
    public class AfterMatch
    {
        public int AfterMatchId { get; set; }
        public DateTime GameDate { get; set; }
        public string GameWinner { get; set; }
    }



}