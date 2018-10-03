using Ecard.Services;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroMall.Models.Orders
{
    public class DeleteOrderDetail
    {
        [Dependency]
        public IOrderDetailService OrderDetailService { get; set; }
         [Dependency]
        public IOrderService OrderService { get; set; }
        [Dependency]
         public ILog4netService Log4netService { get; set; }
        public int commodityId { get; set; }

        public string orderNo { get; set; }

        public ResultMessage Save()
        {
            try
            {
                OrderDetailService.DeleteOrderDetail(commodityId, orderNo);
                if (OrderService.OrderDetailCount(orderNo) <= 0)
                {
                    var item = OrderService.GetByOrderNo(orderNo);
                    if (item != null && item.item != null)
                        OrderService.Delete(item.item);
                }
                return new ResultMessage() { Code = 0 };
            }
            catch(Exception ex)
            {
                Log4netService.Insert(ex);
                return new ResultMessage() { Code = -1, Msg = "系统异常！" };
            }
        }
    }
}