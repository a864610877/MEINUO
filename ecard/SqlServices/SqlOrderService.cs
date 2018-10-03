using Ecard.Infrastructure;
using Ecard.Models;
using Ecard.Requests;
using Ecard.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Ecard.SqlServices
{
    public class SqlOrderService : IOrderService
    {
         private readonly DatabaseInstance _databaseInstance;
         private const string TableName = "fz_Orders";

         public SqlOrderService(DatabaseInstance databaseInstance)
         {
             _databaseInstance = databaseInstance;
         }
         public int Insert(Order item)
        {
            return _databaseInstance.Insert(item,TableName);
        }

         public int Update(Order item)
        {
            return _databaseInstance.Update(item,TableName);
        }

         public int Delete(Order item)
        {
            return _databaseInstance.Delete(item, TableName);
        }

         public Order GetById(int orderId)
        {
            return _databaseInstance.GetById<Order>(TableName, orderId);
        }
         public Order GetOrderNo(string orderNo)
         {
             string sql = "select * from fz_Orders where orderNo=@orderNo";
             return new QueryObject<Order>(_databaseInstance, sql, new { orderNo = orderNo }).FirstOrDefault();
         }


         public DataTables<Ecard.Infrastructure.Models.OrderView> Query(OrderRequest request)
        {
            if (request.orderState == 0)
            {
                request.orderState = null;
            }
            if (request.payState == 0)
            {
                request.payState = null;
            }
            if (request.distributionstate == 0)
            {
                request.distributionstate = null;
            }
            SqlParameter[] param = { 
                                     new SqlParameter("@orderNo",request.orderNo),
                                     new SqlParameter("@name",request.name),
                                     new SqlParameter("@orderState",request.orderState),
                                     new SqlParameter("@payState",request.payState),
                                     new SqlParameter("@distributionstate",request.distributionstate),
                                     new SqlParameter("@startSubmitTime",request.startSubmitTime),
                                     new SqlParameter("@endSubmitTime",request.endSubmitTime),
                                     new SqlParameter("@pageIndex",request.PageIndex),
                                     new SqlParameter("@pageSize",request.PageSize),
                                   };
            StoreProcedure sp = new StoreProcedure("P_getOrder", param);
            return _databaseInstance.GetTables<Ecard.Infrastructure.Models.OrderView>(sp);
        }


         public DataTables<Order> MicroMallQuery(MicroMallOrderRequest request)
         {
             SqlParameter[] param = { 
                                     new SqlParameter("@UserId",request.UserId),
                                     new SqlParameter("@orderState",request.orderState),
                                     new SqlParameter("@OrderType",request.OrderType),
                                     new SqlParameter("@pageIndex",request.PageIndex),
                                     new SqlParameter("@pageSize",request.PageSize),
                                   };
             StoreProcedure sp = new StoreProcedure("P_getMicroMallOrder", param);
             return _databaseInstance.GetTables<Order>(sp);
         }


         public UserOrderDetail GetByOrderNo(string OrderNo)
         {
             var model = new UserOrderDetail();
             string sql = "select * from fz_Orders where orderNo=@orderNo";
             var item = new QueryObject<Order>(_databaseInstance, sql, new { orderNo = OrderNo }).FirstOrDefault();
             if(item!=null)
             {
                 string sql1 = "select * from fz_OrderDetails where orderNo=@orderNo";
                 var list = new QueryObject<OrderDetail>(_databaseInstance, sql1, new { orderNo = OrderNo }).ToList();
                 model.item = item;
                 model.OrderDetails = list;
             }
             return model;
         }


         public int OrderDetailCount(string OrderNo)
         {
             string sql = "select count(*) from fz_OrderDetails where orderNo=@orderNo";
             object value = _databaseInstance.ExecuteScalar(sql, new { orderNo = OrderNo }).ToString();
             int count = 0;
             if (value == null)
                 return count;
             int.TryParse(value.ToString(), out count);
             return count;
         }


         public DataTables<OrderCD> GetQuery(OrderCDRequest request)
         {
             throw new NotImplementedException();
         }

         public int GetQuantityByOrderNoAndcommodityId(string OrderNo, int commodityId, string specification)
         {
             int quantity = 0;
             string sql = "select * from fz_OrderDetails where orderNo=@orderNo AND commodityId=@commodityId and specification = @specification";
             var item = new QueryObject<OrderDetail>(_databaseInstance, sql, new { orderNo = OrderNo, commodityId = commodityId, specification = specification }).FirstOrDefault();
             if (item !=null)
             {
                 quantity = item.quantity;
             }
             return quantity;
         }


         public List<Order> GetOrder()
         {
             string sql = "select * from fz_Orders where orderState=@orderState";
             var list = new QueryObject<Order>(_databaseInstance, sql, new { orderState = OrderStates.shipped });
             if (list != null)
                 return list.ToList();
             return null;
         }
    }
}
