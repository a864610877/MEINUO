using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Ecard.Infrastructure;
using Ecard.Models;
using Ecard.Services;
using Microsoft.Practices.Unity;
using Moonlit;
using Moonlit.Text;
using Oxite.Mvc.Extensions;

namespace Ecard.Mvc.Models.Users
{
    public class EditProfile:ViewModelBase
    {
        public void Ready()
        {
            User currentUser = SecurityHelper.GetCurrentUser().CurrentUser;
            DisplayName = currentUser.DisplayName;
            Mobile = currentUser.Mobile;
            Email = currentUser.Email;
            BirthDate = currentUser.BirthDate;

        }

        [Dependency, NoRender]
        public IMembershipService MembershipService { get; set; }

        private string _displayName;

        public string DisplayName
        {
            get { return _displayName.TrimSafty(); }
            set { _displayName = value; }
        }

        private string _mobile;

        public string Mobile
        {
            get { return _mobile.TrimSafty(); }
            set { _mobile = value; }
        }

        private string _email;

        public string Email
        {
            get { return _email.TrimSafty(); }
            set { _email = value; }
        }


        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; } 
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        public IMessageProvider Save()
        {
            User currentUser = SecurityHelper.GetCurrentUser().CurrentUser;
            var user = MembershipService.GetUserById(currentUser.UserId);
            InnerSave(user);
            InnerSave(currentUser);
            if (!string.IsNullOrEmpty(Password))
                user.SetPassword(Password);

            MembershipService.UpdateUser(user);
            Logger.LogWithSerialNo(LogTypes.EditProfile, SerialNoHelper.Create(), currentUser.UserId, user.Name);
            AddMessage("success");
            return this;
        }

        private void InnerSave(User user)
        {
            user.DisplayName = DisplayName;
            user.Mobile = Mobile;
            user.BirthDate = BirthDate;
            user.Email = Email;
        }
    }
}
