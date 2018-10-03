using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Ecard.Models;
using Ecard.Services;
using Microsoft.Practices.Unity;

namespace Ecard.Mvc.Models.Users
{
    public class RecoveryUserPassword : ViewModelBase
    {
        public string Token { get; set; }
        public string Error { get; set; }
        public string DisplayName { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "����������")]
        public string Password { get; set; }
        [NoRender, Dependency]
        public ITemporaryTokenKeyService TemporaryTokenKeyService { get; set; }

        [NoRender, Dependency]
        public IMembershipService MembershipService { get; set; }
        public void Ready()
        {
            var tokenkey = TemporaryTokenKeyService.GetToken(Token, TokenKeyTypes.RecoveryPassword);
            if (tokenkey == null)
            {
                Error = "��������ʧЧ";
                return;
            }

            DisplayName = tokenkey.UserName;
        }
        public bool Save()
        {
            var tokenkey = TemporaryTokenKeyService.GetToken(Token, TokenKeyTypes.RecoveryPassword);
            if (tokenkey == null) return false;
            var user = MembershipService.GetUserByName(tokenkey.UserName);
            if (user == null)
                return false;
            user.SetPassword(Password);
            TemporaryTokenKeyService.Delete(tokenkey);
            MembershipService.UpdateUser(user);
            return true;
        }
    }
}