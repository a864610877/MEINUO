using Ecard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroMall.Models.JuMeiMallIndex
{
    public class OrderDetailsModel
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        public decimal Price { get; set; }

        public int quantity { get; set; }

        public string Image { get; set; }

        public string specification { get; set; }

        public int Id { get; set; }

        public OrderDetailsModel()
        {
            

        }
    }

    public class ListOrderDetail
    {
        public ListOrderDetail()
        {
            OrderDetailsList = new List<OrderDetailsModel>();
            ListUserAddress = new List<UserAddress>();
        }

        public List<UserAddress> ListUserAddress { get; set; }
        public List<OrderDetailsModel> OrderDetailsList { get; set; }

        /// <summary>
        /// 合计总金额
        /// </summary>
        public decimal TotalAmt { get; set; }

        /// <summary>
        /// 运费
        /// </summary>
        public decimal Freight { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int payState { get; set; }

        /// <summary>
        /// 收件人
        /// </summary>
        public string recipients { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string moblie { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        public string detailedAddress { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
       // public string orderState { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public int orderState { get; set; }

        /// <summary>
        /// 快递公司
        /// </summary>
        public string ExpressCompany { get; set; }

        /// <summary>
        /// 快递单号
        /// </summary>
        public string ExpressNumber { get; set; }
        /// <summary>
        /// 用户收货地址
        /// </summary>
        public int UserAddressId { get; set; }

        public string orderNo { get; set; }

    }

   
}