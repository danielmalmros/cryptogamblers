using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace cryptoGamblers.Models
{
    public class AdminIndexViewModel
    {
        //public List<ApplicationUser> AllUsers { get; set; }
        //public List<AppRole> AllRoles { get; set; }
        public IEnumerable<AppRole> AllRoles { get; set; }
        public IEnumerable<ApplicationUser> AllUsers { get; set; }
    }

    public class RoleEditModel
    {
        public AppRole Role { get; set; }
        public IEnumerable<ApplicationUser> Members { get; set; }
        public IEnumerable<ApplicationUser> NonMembers { get; set; }
    }

    public class RoleModificationModel
    {
        [Required]
        public string RoleName { get; set; }
        public string[] IdsToAdd { get; set; }
        public string[] IdsToDelete { get; set; }
    }
}