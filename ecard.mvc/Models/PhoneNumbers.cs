using Moonlit;
using System.ComponentModel.DataAnnotations;

namespace Ecard.Mvc.Models
{
    /// <summary>
    /// 电话号码，可以保存两个号码
    /// </summary>
    public class PhoneNumbers 
    {
        private string _phoneNumber1;
        [RegularExpression(@"((\d{11})|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$)", ErrorMessage = "输入的必须是手机号码和电话号码")]
        public string Value1
        {
            get { return _phoneNumber1.TrimSafty(); }
            set { _phoneNumber1 = value; }
        }

        private string _phoneNumber2;
        [RegularExpression(@"((\d{11})|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$)", ErrorMessage = "输入的必须是手机号码和电话号码")]
        public string Value2
        {
            get { return _phoneNumber2.TrimSafty(); }
            set { _phoneNumber2 = value; }
        }
    }
}