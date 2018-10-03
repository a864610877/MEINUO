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
    public class SqlShoppingCartService : IShoppingCartService
    {
         private readonly DatabaseInstance _databaseInstance;
         private const string TableName = "fz_ShoppingCarts";

         public SqlShoppingCartService(DatabaseInstance databaseInstance)
         {
             _databaseInstance = databaseInstance;
         }
        public int Insert(ShoppingCart item)
        {
            return _databaseInstance.Insert(item,TableName);
        }

        public int Update(ShoppingCart item)
        {
            return _databaseInstance.Update(item, TableName);
        }

        public int Delete(ShoppingCart item)
        {
            return _databaseInstance.Delete(item, TableName);
        }

        public ShoppingCart GetById(int shoppingCartId)
        {
            return _databaseInstance.GetById<ShoppingCart>(TableName, shoppingCartId);
        }

        public DataTables<UserShoppingCarts> GetByUserId(ShoppingCartRequest request)
        {
            SqlParameter[] param = { 
                                     new SqlParameter("@UserId",request.UserId),
                                     new SqlParameter("@pageIndex",request.PageIndex),
                                     new SqlParameter("@pageSize",request.PageSize),
                                   };
            StoreProcedure sp = new StoreProcedure("P_getFz_ShoppingCartsByUserId", param);
            return _databaseInstance.GetTables<UserShoppingCarts>(sp);
        }


        public ShoppingCart GetByUserIdAndCommodityId(int userId, int commodityId)
        {
            string sql = "select * from fz_ShoppingCarts where userId=@userId and commodityId=@commodityId";
            return new QueryObject<ShoppingCart>(_databaseInstance, sql, new { userId = userId, commodityId = commodityId }).FirstOrDefault();
        }


        public DataTables<ShoppingCart> GetByAccountId(int accountId)
        {
            var model = new DataTables<ShoppingCart>();
            string sql = "select * from fz_ShoppingCarts where userId=@accountId ";
            model.ModelList = new QueryObject<ShoppingCart>(_databaseInstance, sql, new { accountId = accountId }).ToList();
            return model;
        }
    }
}
