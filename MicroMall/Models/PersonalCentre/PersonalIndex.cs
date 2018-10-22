using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroMall.Models.PersonalCentre
{
    public class PersonalIndex : ResultMessage
    {
        /// <summary>
        /// 头像
        /// </summary>
        public string photo { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 级别
        /// </summary>
        public string grade { get; set; }
        /// <summary>
        /// 销售额
        /// </summary>
        public decimal saleAmount { get; set; }
        /// <summary>
        /// 总返利金额
        /// </summary>
        public decimal rebateAmount { get; set; }

        public int ID { get; set; }

        /// <summary>
        /// 总积分
        /// </summary>
        public decimal presentExp { get; set; }
        /// <summary>
        /// 可用于提）
        /// </summary>
        public decimal activatePoint { get; set; }
        /// <summary>
        /// 提现积分汇总
        /// </summary>
        public decimal withdrawPoint { get; set; }
    }

    public class PersonalIndexExpress: PersonalIndex
    {
        public string orangeKey { get; set; }
        public string ProvinceName { get; set; }
        public string detailaddress { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Genders { get; set; }
        /// <summary>
        /// 赠送积分
        /// </summary>
        public decimal presentExp { get; set; }
        /// <summary>
        /// 未激活积分
        /// </summary>
        public decimal notActivatePoint { get; set; }
        /// <summary>
        /// 激活积分
        /// </summary>
        public decimal activatePoint { get; set; }
        /// <summary>
        /// 消费积分汇总
        /// </summary>
        public decimal payPoint { get; set; }
        /// <summary>
        /// 提现积分汇总
        /// </summary>
        public decimal withdrawPoint { get; set; }
    }
}