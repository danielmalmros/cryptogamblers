using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace cryptoGamblers.Models
{

    public class UserProfileViewModel
    {
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Display(Name = "Profile picture")]
        public string AvatarPath { get; set; }

        [Display(Name = "Profile description")]
        public string ProfileDescription { get; set; }
    }
}
