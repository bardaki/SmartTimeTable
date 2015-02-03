using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FinalProject.Models
{
    public class ChangePasswordModel
    {
        //[Required]
        [DataType(DataType.Password)]
        [Display(Name = "סיסמא נוכחית")]
        public string OldPassword { get; set; }

        //[Required]
        [StringLength(100, ErrorMessage = "ה{0} חייבת להיות לפחות {2} תווים", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "סיסמא חדשה")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "סיסמא חדשה שנית")]
        [Compare("סיסמא חדשה", ErrorMessage = "שתי הסיסמאות לא תואמות")]
        public string ConfirmPassword { get; set; }
    }

    public class LogOnModel
    {
        //[Required]
        [Display(Name = "שם משתמש")]
        public string UserName { get; set; }

        //[Required]
        [DataType(DataType.Password)]
        [Display(Name = "סיסמא")]
        public string Password { get; set; }

        [Display(Name = "?זכור אותי")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        //[Required]
        [Display(Name = "שם משתמש")]
        public string UserName { get; set; }

        //[Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        //[Required]
        [StringLength(100, ErrorMessage = "ה{0} חייבת להיות לפחות {2} תווים", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "סיסמא")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "סיסמא שנית")]
        [Compare("Password", ErrorMessage = "שתי הסיסמאות לא תואמות")]
        public string ConfirmPassword { get; set; }
    }
}