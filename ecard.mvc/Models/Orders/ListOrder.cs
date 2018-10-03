using Ecard.Infrastructure.Models;
using Ecard.Models;
using Ecard.Services;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Mvc.Models.Orders
{
    public class ListOrder
    {
        private readonly OrderView _innerObject;
        [NoRender]
        public OrderView InnerObject
        {
            get { return _innerObject; }
        }

        public ListOrder()
        {
            _innerObject = new OrderView();
        }
        public ListOrder(OrderView innerObject)
        {
            _innerObject = innerObject;
        }
        [NoRender]
        public int orderId { get { return InnerObject.orderId; } }

        /// <summary>
        /// 订单号
        /// </summary>
        public string orderNo { get { return InnerObject.orderNo; } }
        public string userName { get { return InnerObject.DisplayName; } }

        public decimal amount { get { return InnerObject.amount; } }

        public string orderState
        {
            get
            {
                if (InnerObject.orderState == 1)
                {
                    return "等待付款";
                }
                else if (InnerObject.orderState == 2)
                {
                    return "已付款";
                }
                else if (InnerObject.orderState == 3)
                {
                    return "已发货";
                }
                else if (InnerObject.orderState == 4)
                {
                    return "完成";
                }
                else if (InnerObject.orderState == 5)
                {
                    return "申请退款";
                }
                else
                {
                    return "退款";
                }
            }
        }

        public string payState
        {
            get
            {
                if (InnerObject.payState == 1)
                {
                    return "未支付";
                }
                else
                {
                    return "已付款";
                }
            }
        }

        public string payType
        {
            get
            {
                if (InnerObject.payType == 1)
                {
                    return "微信支付";
                }
                else
                {
                    return "货到付款";
                }
            }
        }

        public string distributionType 
        {
            get
            {
                if (InnerObject.distributionType == 1)
                {
                    return "快递";
                }
                else if (InnerObject.distributionType == 2)
                {
                    return "EMS";
                }
                else if (InnerObject.distributionType == 3)
                {
                    return "平邮";
                }
                else
                {
                    return "买家承担运费";
                }
            }
        }
        public string distributionstate
        {
            get
            {
                if (InnerObject.distributionstate == 1)
                {
                    return "未发货";
                }
                else if (InnerObject.distributionstate == 2)
                {
                    return "已发货";
                }
                else
                {
                    return "已收货";
                }
            }
        }
        public string submitTime { get { return InnerObject.submitTime.ToString("yyyy-MM-dd"); } }



        [NoRender]
        public string boor { get; set; }
    }
}
