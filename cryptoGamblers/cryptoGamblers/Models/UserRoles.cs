using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cryptoGamblers.Models
{
    public class UserRoles : IdentityRole
    {
        public int Id { get; set; }
        public string Userrole { get; set; }
    }
}