using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace cryptoGamblers.Models
{
    public enum MatchState
    {
        PENDINGBET,
        PENDINGBETRESPONSE,
        DECLINED,
        ACCEPTED,
        FINISHED
    }
    public class MatchData
    {
        [Key]
        public int MatchDataId { get; set; }
        public int PrizePool { get; set; }
        public MatchState MatchState { get; set; }
        public int? Opponent1Roll { get; set; }
        public int? Opponent2Roll { get; set; }

        public bool rollReturnedOpponent1 { get; set; }
        public bool rollReturnedOpponent2 { get; set; }

        [ForeignKey("Match")]
        public virtual int? MatchId { get; set;}
        public virtual Match Match { get; set; }

    }
}