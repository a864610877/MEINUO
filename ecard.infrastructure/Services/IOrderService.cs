using Ecard.Infrastructure;
using Ecard.Infrastructure.Models;
using Ecard.Models;
using Ecard.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Services
{
    public interface IOrderService
    {
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        int Insert(Order item);
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        int Update(Order item);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        int Delete(Order item);
        /// <summary>
        /// 根据id，查询实体
        /// </summary>
        /// <param name="commodityId"></param>
        /// <returns></returns>
        Order GetById(int OrderId);
        Order GetOrderNo(string orderNo);


        /// <summary>
        /// 根据条件获取订单列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        DataTables<OrderView> Query(OrderRequest request);
        DataTables<Order> MicroMallQuery(MicroMallOrderRequest request);
        /// <summary>
        /// 根据订单编号查询订单信息及其明细
        /// </summary>
        /// <param name="OrderNo"></param>
        /// <returns></returns>
        UserOrderDetail GetByOrderNo(string OrderNo);
        /// <summary>
        /// 获取明细总数
        /// </summary>
        /// <param name="OrderNo"></param>
        /// <returns></returns>
        int OrderDetailCount(string OrderNo);
        /// <summary>
        /// 根据条件获取订单列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        DataTables<OrderCD> GetQuery(OrderCDRequest request);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OrderNo"></param>
        /// <returns></returns>
        int GetQuantityByOrderNoAndcommodityId(string OrderNo, int commodityId, string specification);

        /// <summary>
        /// 获取要自动收货的订单
        /// </summary>
        /// <returns></returns>
        List<Order> GetOrder();

    }

    public class OrderCDRequest:PageRequest
    {
        public int userId { get; set; }
        public int? orderState { get; set; }
    }

    public class OrderCD
    {
        public int orderId { get; set; }
        public string orderNo { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public int orderState { get; set; }
        /// <summary>
        /// 订单总金额
        /// </summary>
        public decimal amount { get; set; }
        /// <summary>
        /// 商品图片
        /// </summary>
        public string images { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string commodityName { get; set; }
        /// <summary>
        /// 商品规格
        /// </summary>
        public string specification { get; set; }
        /// <summary>
        /// 商品数量
        /// </summary>
        public int quantity { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public decimal price { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime submitTime { get; set; }
    }
}
