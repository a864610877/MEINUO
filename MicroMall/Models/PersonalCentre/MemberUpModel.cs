using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroMall.Models.PersonalCentre
{
    public class MemberUpModel
    {
        public MemberUpModel() { }
        public MemberUpModel(string mobile="", decimal member= MemberUpPirce.member, decimal shopowner= MemberUpPirce.shopowner, decimal shopkeeper= MemberUpPirce.shopkeeper)
        {
            Mobile = mobile;
            Member = member;
            Shopowner = shopowner;
            Shopkeeper = shopkeeper;
        }
        public string Mobile { get; set; }
        public decimal Member { get; set; }
        /// <summary>
        /// 店长
        /// </summary>
        public decimal Shopowner { get; set; }
        /// <summary>
        /// 店主
        /// </summary>
        public decimal Shopkeeper { get; set; }
    }

    public class MemberUpPirce
    {
        /// <summary>
        /// vip会员
        /// </summary>
        public const decimal member = 118;
        /// <summary>
        /// 店长
        /// </summary>
        public const decimal shopowner = 238;
        /// <summary>
        /// 店主
        /// </summary>
        public const decimal shopkeeper = 358;
    }
}