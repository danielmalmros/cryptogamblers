using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using cryptoGamblers.Models;
using System.IO;

namespace cryptoGamblers.Controllers
{
    [Authorize]
    public class UserController : Controller
    {

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public UserController() { }

        public UserController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager {
            get {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager {
            get {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set {
                _userManager = value;
            }
        }
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> UserProfile()
        {
            string userName = Request.QueryString["username"];

            if(userName != null) { 
                try { 
                var user = await UserManager.FindByNameAsync(userName);

                    UserProfileViewModel model = new UserProfileViewModel()
                    {
                        Username = user.UserName,
                        AvatarPath = user.Avatar,
                        ProfileDescription = user.ProfileDescription,
                        WinStreak = user.WinStreak,
                        WinStreakMax = user.WinStreakMax
                    };

                    return View(model);
                } catch (Exception e)
                {
                    return HttpNotFound();
                }

            }
            //If user can't be found
            return HttpNotFound();
        }
    }
}