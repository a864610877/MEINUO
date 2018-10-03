using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Ecard.Mvc.Controllers;
using Ecard.Services;

namespace Ecard.Web.Models
{

    #region Models

    public class ChangePasswordModel
    {
        [Required(ErrorMessage= "请输入原密码")]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }
              
        [Required(ErrorMessage = "请输入密码")]
        [ValidatePasswordLength]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "请输入确认密码")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }


    public class RegisterModel
    {
        [Required(ErrorMessage = "请输入用户名")]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "请输入电子邮件")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "请输入密码")]
        [ValidatePasswordLength]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "请输入确认密码")]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; }
    }
    #endregion


}
