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
    public class SqlOrderDetailService : IOrderDetailService
    {
          private readonly DatabaseInstance _databaseInstance;
         private const string TableName = "fz_OrderDetails";

         public SqlOrderDetailService(DatabaseInstance databaseInstance)
         {
             _databaseInstance = databaseInstance;
         }
         public int Insert(OrderDetail item)
        {
            return _databaseInstance.Insert(item, TableName);
        }

         public int Update(OrderDetail item)
        {
            return _databaseInstance.Update(item, TableName);
        }

         public int Delete(OrderDetail item)
        {
            return _databaseInstance.Delete(item, TableName);
        }
         public OrderDetail GetById(int id)
         {
             return _databaseInstance.GetById<OrderDetail>(TableName, id);
         }


         public QueryObject<ExpressCompany> QueryAll()
         {
             var sql = @"select * from ExpressCompany";
             return new QueryObject<ExpressCompany>(_databaseInstance, sql, null);
         }

         public DataTables<OrderDetail> GetByOrderId(OrderDetailRequest request)
         {

             SqlParameter[] param = { 
                                     new SqlParameter("@orderId",request.orderNo),
                                     new SqlParameter("@PageIndex",request.PageIndex),
                                     new SqlParameter("@PageSize",request.PageSize),
                                   };
             StoreProcedure sp = new StoreProcedure("P_getOrdrDetailByOrderId", param);
             return _databaseInstance.GetTables<OrderDetail>(sp);
         }



         public OrderDetailView TopOne(string OrderNo)
         {
             string sql = "select top 1 od.orderId,od.orderNo,od.commodityName,od.quantity,c.images from fz_OrderDetails od left join fz_Commoditys c on od.commodityId=c.commodityId  where OrderNo=@OrderNo";
             return new QueryObject<OrderDetailView>(_databaseInstance, sql, new { OrderNo = OrderNo }).FirstOrDefault();
         }


         public int DeleteOrderDetail(int commodityId, string orderNo)
         {
             string sql = "delete from fz_OrderDetails where commodityId=@commodityId and orderNo=@orderNo";
             return _databaseInstance.ExecuteNonQuery(sql, new { commodityId = commodityId, orderNo = orderNo });
         }

         public int UpdateOrderDetailOrderId(int orderId, string orderNo)
         {
             string sql = "Update fz_OrderDetails set orderId =@orderId where orderNo=@orderNo";
             return _databaseInstance.ExecuteNonQuery(sql, new { orderId = orderId, orderNo = orderNo });
         }

         public QueryObject<OrderDetailView> GetByOrderNo(string OrderNo)
         {
             string sql = "select od.specification, od.price,od.commodityId,od.orderId,od.orderNo,od.commodityName,od.quantity,SUBSTRING(c.images,0,charindex(',',c.images)) as images from fz_OrderDetails od left join fz_Commoditys c on od.commodityId=c.commodityId  where OrderNo=@OrderNo";
             return new QueryObject<OrderDetailView>(_databaseInstance, sql, new { OrderNo = OrderNo });
         }


        public OrderDetail GetBycommodityIdAndOrderNo(int commodityId, string orderNo)
        {

            string sql = "select * from fz_OrderDetails where commodityId=@commodityId and orderNo=@orderNo";
            return new QueryObject<OrderDetail>(_databaseInstance, sql, new { commodityId = commodityId, orderNo = orderNo }).FirstOrDefault();
        }


        public QueryObject<OrderDetail> GetAllOrderId(int orderId)
         {
             string sql = "select  * from fz_OrderDetails  where orderId=@orderId";
             return new QueryObject<OrderDetail>(_databaseInstance, sql, new { orderId = orderId });
         }


         public QueryObject<OrderDetail> GetAllByOrderNo(string orderNo)
         {
             string sql = "select  * from fz_OrderDetails  where orderNo=@orderNo";
             return new QueryObject<OrderDetail>(_databaseInstance, sql, new { orderNo = orderNo });
         }

        public int DeleteOrderNo(string orderNo)
        {
            string sql = "delete from fz_OrderDetails  where orderNo=@orderNo";
            return  _databaseInstance.ExecuteNonQuery(sql, new { orderNo = orderNo});
        }
    }
}
