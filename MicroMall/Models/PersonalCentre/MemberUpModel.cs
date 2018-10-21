using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroMall.Models.PersonalCentre
{
    public class MemberUpModel
    {
        public MemberUpModel() { }
        public MemberUpModel(string mobile="", decimal member=118,decimal shopowner=238,decimal shopkeeper=358)
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
}