using Ecard.Services;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Mvc.Models.Orders
{
    public class OrderEdit
    {

        [Dependency]
        [NoRender]
        public IOrderService orderService { get; set; }

        public int orderId { get; set; }

        /// <summary>
        /// 订单状态，1等待付款 2已付款 3已发货 4完成, 5申请退款 6退款
        /// </summary>
        public int orderState { get; set; }
        /// <summary>
        /// 支付状态 1未支付 2已付款
        /// </summary>
        public int payState { get; set; }
        public decimal point { get; set; }
        /// <summary>
        /// 配送方式
        /// </summary>
        public int distributionType { get; set; }
        /// <summary>
        /// 支付方式 1微信支付 2货到付款
        /// </summary>
        public int payType { get; set; }
        /// <summary>
        /// 省id
        /// </summary>
        public int provinceId { get; set; }

        /// <summary>
        /// 快递公司
        /// </summary>
        public string ExpressCompany { get; set; }
        /// <summary>
        /// 城市id
        /// </summary>
        public int cityId { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        public string detailedAddress { get; set; }
        /// <summary>
        /// 配送状态
        /// </summary>
        public int distributionstate { get; set; }
        /// <summary>
        /// 收件人
        /// </summary>
        public string recipients { get; set; }
        /// <summary>
        /// 邮政编码
        /// </summary>
        public string zipCode { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string moblie { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string phone { get; set; }


        public ResultMsg Save()
        {
            var order = orderService.GetById(this.orderId);
            ResultMsg result = new ResultMsg();
            if (order != null)
            {

                order.distributionstate = distributionstate;
                order.distributionType = distributionType;
                order.detailedAddress = detailedAddress;
                order.payState = payState;
                order.payType = payType;
                order.phone = phone;
                order.zipCode = zipCode;
                order.recipients = recipients;
                order.moblie = moblie;
                order.ExpressCompany = ExpressCompany;
                order.cityId = cityId;
                order.provinceId = provinceId;
                order.orderState = orderState;
                order.point = point;
                orderService.Update(order);
                result.Code = 1;
                result.CodeText = "修改成功!";
                return result;
            }
            else
            {
                result.Code = 2;
                result.CodeText = "修改失败!";
                return result;
            }

        }
    }
}
