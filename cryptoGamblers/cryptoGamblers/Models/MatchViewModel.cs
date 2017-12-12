using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cryptoGamblers.Models
{
    public class MatchViewModel
    {
        public Match Match { get; set; }
        public ApplicationUser Opponent1 { get; set; }
        public ApplicationUser Opponent2 { get; set; }
        public bool IsOpponent1 { get; set; }
    }
}