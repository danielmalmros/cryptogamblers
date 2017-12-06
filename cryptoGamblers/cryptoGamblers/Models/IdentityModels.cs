using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace cryptoGamblers.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public double Balance { get; set; }
		public string Avatar { get; set; }
		public string ProfileDescription { get; set; }
        public int WinStreak { get; set; }
        public int WinStreakMax { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        static ApplicationDbContext()
        {
            Database.SetInitializer<ApplicationDbContext>(new IdentityDbInit());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<QueueIn> queueIn { get; set; }
        public DbSet<Match> Match { get; set; }
        public DbSet<AfterMatch> AfterMatch { get; set; }

    }
    public class IdentityDbInit : NullDatabaseInitializer<ApplicationDbContext>
    {

    }
}