using Moonlit;
using Moonlit.Validations;
using System.ComponentModel.DataAnnotations;

namespace Ecard.Mvc.Models
{
    /// <summary>
    /// �ֻ��࣬���԰����������룬Ĭ�ϰ󶨵�һ�����롣
    /// </summary>
    public class Mobiles  
    {
        /// <summary>
        /// �ֻ������Ƿ���Ч
        /// </summary>
        public bool IsMobileAvailable { get; set; }
        private string _mobile1;
        private string _code;
        /// <summary>
        /// �ֻ�����֤�룿
        /// </summary>
        public string Code
        {
            get { return _code.TrimSafty(); }
            set { _code = value; }
        } 
        /// <summary>
        /// ��һ���ֻ�����
        /// </summary>
        
        [RegularExpression(@"((\d{11})|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$)", ErrorMessage = "����ı������ֻ�����͵绰����")]
        public string Value1
        {
            get { return _mobile1.TrimSafty(); }
            set { _mobile1 = value; }
        }

        private string _mobile2;
        private bool _hasBinding;
         /// <summary>
         /// �ڶ����ֻ�����
         /// </summary>
        [RegularExpression(@"((\d{11})|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$)", ErrorMessage = "����ı������ֻ�����͵绰����")]
        public string Value2
        {
            get { return _mobile2.TrimSafty(); }
            set { _mobile2 = value; }
        }
        /// <summary>
        /// �Ƿ��
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