using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace cryptoGamblers.Models
{
    public class User : IdentityUser
    {
        public int UserId { get; set; }
        public string Username { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6)]
        public string Password { get; set; }

        //[Required]
        //[Compare("Password")]
        //public string ConfirmPassword { get; set; }


        public string Email { get; set; }
        public double Balance { get; set; }
        public string AvatarURI { get; set; }
        public int WinStreak { get; set; }


        public User()
        {

        }

        public User(string username, string email)
        {
            email = Email;
            username = Username;


        }


    }
}