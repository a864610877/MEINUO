using Ecard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Mvc.Models.Withdraws
{
    public class ListWithdraw
    {
        private readonly WithdrawModel _innerObject;
        [NoRender]
        public WithdrawModel InnerObject
        {
            get { return _innerObject; }
        }

        public ListWithdraw()
        {
            _innerObject = new WithdrawModel();
        }
        public ListWithdraw(WithdrawModel innerObject)
        {
            _innerObject = innerObject;
        }
        [NoRender]

        public int withdrawId { get { return InnerObject.withdrawId; } }
        [NoRender]
        /// 提现用户id
        /// </summary>
        public int userId { get { return InnerObject.userId; } }

        public string orderNo { get { return InnerObject.orderNo; } }
        /// <summary>
        /// 用户名
        /// </summary>
        public string DisplayName { get { return InnerObject.DisplayName; } }
        /// <summary>
        /// 性别
        /// </summary>
        public string Gender { get { return InnerObject.Gender == 1 ? "男" : "女"; } }
        /// <summary>
        /// 电子邮箱
        /// </summary>
        //public string Email { get { return InnerObject.Email; } }
        /// <summary>
        /// 电子邮箱
        /// </summary>
        //public string Mobile { get { return InnerObject.Mobile; } }
        /// <summary>
        /// 提现积分
        /// </summary>
        //public decimal point { get { return InnerObject.point; } }
        /// <summary>
        /// 换算后金额
        /// </summary>
        public decimal amount { get { return InnerObject.amount; } }
        /// <summary>
        /// 审核员
        /// </summary>
        public string Operator { get { return InnerObject.Operator; } }
        /// <summary>
        /// 状态
        /// </summary>
        public string state
        {
            get
            {
                if (InnerObject.state==1)
                {
                    return "待审核";

                }
                else if (InnerObject.state==2)
                {
                    return "成功";

                }
                else
                {
                    return "失败";

                }
            }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get { return InnerObject.remark; } }
        /// <summary>
        /// 提交时间
        /// </summary>
        public string submitTime { get { return InnerObject.submitTime.ToString("yyyy-MM-dd"); } }


        [NoRender]
        public string boor { get; set; }
    }
}
