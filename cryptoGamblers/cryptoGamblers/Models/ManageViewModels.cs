using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace cryptoGamblers.Models
{
    public class IndexViewModel
    {
        
        //public bool HasPassword { get; set; }
        //public IList<UserLoginInfo> Logins { get; set; }
        //public string PhoneNumber { get; set; }
        //public bool TwoFactor { get; set; }
        //public bool BrowserRemembered { get; set; }
    }

    //public class ManageLoginsViewModel
    //{
    //    public IList<UserLoginInfo> CurrentLogins { get; set; }
    //    public IList<AuthenticationDescription> OtherLogins { get; set; }
    //}

    //public class FactorViewModel
    //{
    //    public string Purpose { get; set; }
    //}

    //public class SetPasswordViewModel
    //{
    //    [Required]
    //    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
    //    [DataType(DataType.Password)]
    //    [Display(Name = "New password")]
    //    public string NewPassword { get; set; }

    //    [DataType(DataType.Password)]
    //    [Display(Name = "Confirm new password")]
    //    [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
    //    public string ConfirmPassword { get; set; }
    //}

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangeBalanceViewModel
    {
        [Required]
        [Display(Name = "Add Balance")]
        public double AddBalance { get; set; }

        [Required(ErrorMessage = "Please add correct Wallet ID")]
        [RegularExpression(@"^[13][a-km-zA-HJ-NP-Z1-9]{25,34}$", ErrorMessage = "Please insert a correct Wallet ID")]
        [Display(Name = "Add Wallet ID")]
        public string AddWalletID { get; set; }
    }

    public class ChangeAvatarViewModel
    {
        [Required]
        [Display(Name = "Change Avatar")]
        public string AvatarPath { get; set; }
    }

	public class ChangeDescriptionViewModel
	{
		[Display(Name = "Change Profile Text")]
		[System.Web.Mvc.AllowHtml]
		public string ProfileDescription { get; set; }
	}

    //public class AddPhoneNumberViewModel
    //{
    //    [Required]
    //    [Phone]
    //    [Display(Name = "Phone Number")]
    //    public string Number { get; set; }
    //}

    //public class VerifyPhoneNumberViewModel
    //{
    //    [Required]
    //    [Display(Name = "Code")]
    //    public string Code { get; set; }

    //    [Required]
    //    [Phone]
    //    [Display(Name = "Phone Number")]
    //    public string PhoneNumber { get; set; }
    //}

    //public class ConfigureTwoFactorViewModel
    //{
    //    public string SelectedProvider { get; set; }
    //    public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    //}
}