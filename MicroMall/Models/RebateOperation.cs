using Ecard.Services;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroMall.Models
{
    public class RebateOperation
    {
        [Dependency]
        public IOrderService OrderService { get; set; }
        [Dependency]
        public IOrderDetailService OrderDetailService { get; set; }

        [Dependency]
        public IShoppingCartService ShoppingCartService { get; set; }

        [Dependency]
        public IAccountService AccountService { get; set; }
        [Dependency]
        public IRebateService RebateService { get; set; }
        [Dependency]
        public IOperationPointLogService OperationPointLogService { get; set; }

      
        //public void Rebate(string orderNo)
        //{ 
        //     var order=OrderService
        //}

    }
}