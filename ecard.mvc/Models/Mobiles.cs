using Moonlit;
using Moonlit.Validations;
using System.ComponentModel.DataAnnotations;

namespace Ecard.Mvc.Models
{
    /// <summary>
    /// 手机类，可以包含两个号码，默认绑定第一个号码。
    /// </summary>
    public class Mobiles  
    {
        /// <summary>
        /// 手机号码是否有效
        /// </summary>
        public bool IsMobileAvailable { get; set; }
        private string _mobile1;
        private string _code;
        /// <summary>
        /// 手机的验证码？
        /// </summary>
        public string Code
        {
            get { return _code.TrimSafty(); }
            set { _code = value; }
        } 
        /// <summary>
        /// 第一个手机号码
        /// </summary>
        
        [RegularExpression(@"((\d{11})|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$)", ErrorMessage = "输入的必须是手机号码和电话号码")]
        public string Value1
        {
            get { return _mobile1.TrimSafty(); }
            set { _mobile1 = value; }
        }

        private string _mobile2;
        private bool _hasBinding;
         /// <summary>
         /// 第二个手机号码
         /// </summary>
        [RegularExpression(@"((\d{11})|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$)", ErrorMessage = "输入的必须是手机号码和电话号码")]
        public string Value2
        {
            get { return _mobile2.TrimSafty(); }
            set { _mobile2 = value; }
        }
        /// <summary>
        /// 是否绑定
        /// </summary>
        public bool HasBinding
        {
            get {
                return _hasBinding;
            }
            set {
                _hasBinding = value;
            }
        }
        public int UserId { get; set; }
    }
}