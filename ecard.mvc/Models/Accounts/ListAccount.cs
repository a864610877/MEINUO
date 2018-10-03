using Ecard.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Mvc.Models.Accounts
{
    public class ListAccount
    {
        private readonly AccountModel _innerObject;
        [NoRender]
        public AccountModel InnerObject
        {
            get { return _innerObject; }
        }

        public ListAccount()
        {
            _innerObject = new AccountModel();
        }
        public ListAccount(AccountModel innerObject)
        {
            _innerObject = innerObject;
        }

        [NoRender]
        /// <summary>
        /// 用户名
        /// </summary>
        public int accountId { get { return InnerObject.accountId; } }
        /// <summary>
        /// 用户名
        /// </summary>
        public string DisplayName { get { return InnerObject.name; } }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get { return InnerObject.Mobile; } }
        /// <summary>
        /// 性别
        /// </summary>
        public string Gender { get { return InnerObject.Gender == 1 ? "男" : "女"; } }
        /// <summary>
        /// 电子邮箱
        /// </summary>
        //public string Email { get { return InnerObject.Email; } }

        /// <summary>
        /// 会员状态
        /// </summary>
       // public string State { get { return InnerObject.State == 1 ? "正常" : "停用"; } }

        public decimal presentExp { get { return InnerObject.presentExp; } }

        public decimal activatePoint { get { return InnerObject.activatePoint; } }
        ///// <summary>
        ///// 详细地址
        ///// </summary>
        //public string Address { get { return InnerObject.Address; } }
        /// <summary>
        /// 推荐人
        /// </summary>
        public string salerName { get { return InnerObject.salerName; } }
        /// <summary>
        /// 余额
        /// </summary>
       // public decimal amount { get { return InnerObject.amount; } }
        /// <summary>
        /// 级别
        /// </summary>
        public string grade { get { return ModelHelper.GetBoundText(InnerObject, x => x.grade); } }

        public string submitTime { get { return InnerObject.submitTime.ToString(); } }
        /// <summary>
        /// 会员推荐码
        /// </summary>
      //  public string orangeKey { get { return InnerObject.orangeKey; } }
        /// <summary>
        /// 会员推荐链接
        /// </summary>
        //public string qrCodeUrl { get { return InnerObject.qrCodeUrl; } }
        /// <summary>
        /// 会员推荐二维码
        /// </summary>
       // public string ticket { get { return InnerObject.ticket; } }


        [NoRender]
        public string boor { get; set; }
    }
}
