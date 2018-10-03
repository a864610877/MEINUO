using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oxite.Model;

namespace Ecard.Models
{
    
    public class PrePayStates
    {
        /// <summary>
        /// 全部
        /// </summary>
        public const int All = Globals.All;
        /// <summary>
        /// 进行状态
        /// </summary>
        public const int Processing = 1;
        /// <summary>
        /// 完成时
        /// </summary>
        public const int Complted = 2;
    }
    public class UserStates : States
    {
      
    }
    public class DealLogStates
    { 
        /// <summary>
        /// 正常的
        /// </summary>
        public const int Normal = 1; 

        /// <summary>
        /// 全部
        /// </summary>
        public const int All = 100000;
        /// <summary>
        /// 消费冲正
        /// </summary>
        public const int Normal_ = 3;

        /// <summary>
        /// 取消
        /// </summary>
        public const int Cancel = 4;
    }
    public class RoleStates : States
    {

    }
    public class ShopStates : States
    {
    }
    public class PosEndPointStates : States
    {
    }
    public class ShopRoles
    {
        public const int Owner = 1;
        public const int Employee = 2;
    }
}
