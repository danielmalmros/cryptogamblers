using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using cryptoGamblers.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cryptoGamblers.Models
{
    public class QueueIn
    {
        public int Id { get; set; }
        public string Opponent1 { get; set; }
        public string Opponent2 { get; set; }
    }
    public enum MatchState
    {
        PENDINGBETPROPOSAL,
        PENDINGBETACCEPTANCE,
        FINISHED
    }

    public class Match
    {
        [Key]
        public int MatchId { get; set; }
        public string Opponent1 { get; set; }
        public string Opponent2 { get; set; }
        public DateTime Date { get; set; }
        public MatchState MatchState { get; set; }
        //[ForeignKey("MatchDataId")]
    }
    public class AfterMatch
    {
        public int AfterMatchId { get; set; }
        public DateTime GameDate { get; set; }
        public string GameWinner { get; set; }
    }



}