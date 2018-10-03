using Ecard.Infrastructure;
using Ecard.Models;
using Ecard.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Services
{
    public interface IOrderDetailService
    {

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        int Insert(OrderDetail item);
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        int Update(OrderDetail item);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        int Delete(OrderDetail item);
        /// <summary>
        /// 根据id，查询实体
        /// </summary>
        /// <param name="commodityId"></param>
        /// <returns></returns>
        DataTables<OrderDetail> GetByOrderId(OrderDetailRequest request);


        QueryObject<OrderDetail> GetAllByOrderNo(string OrderNo);
        OrderDetail GetById(int id);
        /// <summary>
        /// 查询所有快递公司
        /// </summary>
        /// <returns></returns>
        QueryObject<ExpressCompany> QueryAll();

        /// <summary>
        /// 根据条件获取订单列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        //DataTables<OrderDetail> Query(OrderRequest request);
        /// <summary>
        /// 获取该订单的一个商品
        /// </summary>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        OrderDetailView TopOne(string OrderNo);
        /// <summary>
        /// 根据订单号获取订单详情
        /// </summary>
        /// <param name="OrderNo"></param>
        /// <returns></returns>
        QueryObject<OrderDetailView> GetByOrderNo(string OrderNo);

        int DeleteOrderDetail(int commodityId, string orderNo);

        int UpdateOrderDetailOrderId(int orderId, string orderNo);
        OrderDetail GetBycommodityIdAndOrderNo(int commodityId, string orderNo);

        QueryObject<OrderDetail> GetAllOrderId(int OrderId);
    }
}
