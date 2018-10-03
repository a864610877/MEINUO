using System.ComponentModel.DataAnnotations;
using Ecard.Mvc.ViewModels;

namespace Ecard.Mvc.Models.Users
{
    public class LogOnModel : EcardModel
    {
        [Required(ErrorMessage = "请输入用户名")]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        public string LogonToken { get; set; }
    }

}
