using Ecard.Models;
using Ecard.Services;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroMall.Models.Orders
{
    public class PayView:layouts.LayoutModel
    {
        public string orderNo { get; set; }

        [Dependency]
        public IOrderService OrderService { get; set; }

        public Order Order { get; set; }

        public void Ready()
        {
            Load();
            var order = OrderService.GetOrderNo(orderNo);
            if(orderNo!=null)
            {
                Order = order;
            }
        }
    }
}