using cryptoGamblers.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace cryptoGamblers.Controllers
{
	[Authorize]
	public class AdminController : Controller
	{
		// GET: Admin
		public ActionResult Index()
		{
			IEnumerable<ApplicationUser> allUsers = UserManager.Users.ToList();
			IEnumerable<AppRole> allRoles = RoleManager.Roles.ToList();

			AdminIndexViewModel model = new AdminIndexViewModel
			{
				AllRoles = allRoles,
				AllUsers = allUsers
			};

			return View(model);
		}

		public ActionResult CreateUser()
		{
			return View();
		}


		//
		// POST: /Admin/CreateUser
		[HttpPost]
		public async Task<ActionResult> CreateUser(RegisterViewModel model)
		{
			if (ModelState.IsValid)
			{
				//Avatar - check for uploaded files
				if (Request.Files.Count > 0)
				{
					HttpPostedFileBase file = Request.Files[0];
					if (file.ContentLength > 0)
					{
						string fileNameRandomExt = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
						string uploadResult = Path.Combine(Server.MapPath("~/Content/uploads"), fileNameRandomExt);
						model.Avatar = fileNameRandomExt;
						file.SaveAs(uploadResult);
					}
				}
				else
				{
					if (Request.Files.Count == 0)
					{
						model.Avatar = "placeholder_profile_picture.jpg";
					}
				}


				var user = new ApplicationUser { UserName = model.Username, Email = model.Email, Balance = 100, Avatar = model.Avatar };
				var result = await UserManager.CreateAsync(user, model.Password);
				if (result.Succeeded)
				{
					return RedirectToAction("Index");
				}
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}


		//
		// POST: DELETE
		[HttpPost]
		public async Task<ActionResult> Delete(string id)
		{
			ApplicationUser user = await UserManager.FindByIdAsync(id);
			if (user != null)
			{
				IdentityResult result = await UserManager.DeleteAsync(user);
				if (result.Succeeded)
				{
					return RedirectToAction("Index");
				}
				else
				{
					return View("Error", result.Errors);
				}
			}
			else
			{
				return View("Error", new string[] { "User Not Found" });
			}
		}

		//
		// GET: Edit
		public async Task<ActionResult> Edit(string id)
		{
			ApplicationUser user = await UserManager.FindByIdAsync(id);
			if (user != null)
			{
				return View(user);
			}
			else
			{
				return RedirectToAction("Index");
			}
		}

        //
        // POST: Edit
        [HttpPost]
        public async Task<ActionResult> Edit(string id, string email, string password, double balance, string profiledescription, string avatar)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                user.Email = email;
                IdentityResult validEmail = await UserManager.UserValidator.ValidateAsync(user);
                if (!validEmail.Succeeded)
                {
                    return View("Error", validEmail.Errors);
                }

                IdentityResult validPass = null;
                if (password != string.Empty)
                {
                    validPass = await UserManager.PasswordValidator.ValidateAsync(password);
                    if (validPass.Succeeded)
                    {
                        user.PasswordHash = UserManager.PasswordHasher.HashPassword(password);
                    }
                    else
                    {
                        return View("Error", validPass.Errors);
                    }
                }

                user.Balance = balance;
                IdentityResult validBalance = await UserManager.UserValidator.ValidateAsync(user);
                if (!validBalance.Succeeded)
                {
                    return View("Error", validEmail.Errors);
                }

                user.ProfileDescription = profiledescription;
                IdentityResult validDescription = await UserManager.UserValidator.ValidateAsync(user);
                if (!validDescription.Succeeded)
                {
                    return View("Error", validEmail.Errors);
                }

                user.Avatar = avatar;
                IdentityResult validAvatar = await UserManager.UserValidator.ValidateAsync(user);
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase file = Request.Files[0];
                    //Check for image size
                    if (file.ContentLength > 0 && file.ContentLength < 4000000)
                    {
                        if (file.ContentType.Contains("image/jpeg") || file.ContentType.Contains("image/png") || file.ContentType.Contains("image/gif"))
                        {
                            //Create random name & save with path
                            string fileNameRandomExt = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                            string uploadResult = Path.Combine(Server.MapPath("~/Content/uploads"), fileNameRandomExt);
                            file.SaveAs(uploadResult);
                            avatar = fileNameRandomExt;
                            if (!validAvatar.Succeeded)
                            {
                                return View("Error", validAvatar.Errors);
                            }
                        }
                    }
                }
                


                if ((validEmail.Succeeded && validPass == null && validBalance.Succeeded && validDescription.Succeeded && validAvatar.Succeeded) || (validEmail.Succeeded && password != string.Empty && validPass.Succeeded && validBalance.Succeeded && validDescription.Succeeded && validAvatar.Succeeded))
                {
                    IdentityResult result = await UserManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View("Error", result.Errors);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }
            return View(user);
        }




		//
		// CreateRole
		public ActionResult CreateRole()
		{
			return View();
		}

		[HttpPost]
		public async Task<ActionResult> CreateRole([Required]string name)
		{
			if (ModelState.IsValid)
			{
				IdentityResult result = await RoleManager.CreateAsync(new AppRole(name));
				if (result.Succeeded)
				{
					return RedirectToAction("Index");
				}
				else
				{
					return View("Error", result.Errors);
				}
			}
			return View(name);
		}


		//
		// POST: DeleteRole
		[HttpPost]
		public async Task<ActionResult> DeleteRole(string id)
		{
			AppRole role = await RoleManager.FindByIdAsync(id);
			if (role != null)
			{
				IdentityResult result = await RoleManager.DeleteAsync(role);
				if (result.Succeeded)
				{
					return RedirectToAction("Index");
				}
				else
				{
					return View("Error", result.Errors);
				}
			}
			else
			{
				return View("Error", new string[] { "Role Not Found" });
			}
		}


		//
		// EditRole
		public async Task<ActionResult> EditRole(string id)
		{
			AppRole role = await RoleManager.FindByIdAsync(id);
			string[] memberIds = role.Users.Select(x => x.UserId).ToArray();
			IEnumerable<ApplicationUser> members = UserManager.Users.Where(x => memberIds.Any(y => y == x.Id));
			IEnumerable<ApplicationUser> nonMembers = UserManager.Users.Except(members);
			return View(new RoleEditModel
			{
				Role = role,
				Members = members,
				NonMembers = nonMembers
			});
		}


		//
		// POST: EditRole
		[HttpPost]
		public async Task<ActionResult> EditRole(RoleModificationModel model)
		{
			IdentityResult result;
			if (ModelState.IsValid)
			{
				foreach (string userId in model.IdsToAdd ?? new string[] { })
				{
					result = await UserManager.AddToRoleAsync(userId, model.RoleName);
					if (!result.Succeeded)
					{
						return View("Error", result.Errors);
					}
				}
				foreach (string userId in model.IdsToDelete ?? new string[] { })
				{
					result = await UserManager.RemoveFromRoleAsync(userId, model.RoleName);
					if (!result.Succeeded)
					{
						return View("Error", result.Errors);
					}
				}
				return RedirectToAction("Index");
			}
			return View("Error", new string[] { "Role Not Found" });
		}




		private ApplicationUserManager UserManager {
			get {
				return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
			}
		}


		private ApplicationRoleManager RoleManager {
			get {
				return HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();
			}
		}


	}
}