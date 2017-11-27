using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace cryptoGamblers.Models
{
    public class MatchGenerator
    {
        public int Id { get; set; }
        public int UserId { get; set; }


        //public List<user> Users = new List<user>();



        //public void AddUserToQueue(user user)
        //{ 
        //    Users.Add(user);
        //}

        //public void GenerateMatch()
        //{
        //    var GeneratedMatch = Users.GetRange(0, 1);
        //}
    }
}