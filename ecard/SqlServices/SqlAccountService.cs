using Ecard.Infrastructure;
using Ecard.Models;
//using Ecard.Mvc.Models.Accounts;
using Ecard.Requests;
using Ecard.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Ecard.SqlServices
{
   public class SqlAccountService:IAccountService
    {
         private readonly DatabaseInstance _databaseInstance;
         private const string TableName = "fz_Accounts";

         public SqlAccountService(DatabaseInstance databaseInstance)
         {
             _databaseInstance = databaseInstance;
         }
        public int Insert(Account item)
        {
            return _databaseInstance.Insert(item,TableName);
        }

        public int Update(Account item)
        {
            return _databaseInstance.Update(item,TableName);
        }

        public int Delete(Account item)
        {
            return _databaseInstance.Delete(item, TableName);
        }

        public Account GetById(int accountId)
        {
            return _databaseInstance.GetById<Account>(TableName, accountId);
        }

        public Account GetByUserId(int userId)
        {
            string sql = "select * from fz_Accounts where userId=@userId";
            return new QueryObject<Account>(_databaseInstance, sql, new { userId = userId }).FirstOrDefault();
        }


        public Account GetByopenID(string openID)
        {
            string sql = "select * from fz_Accounts where openID=@openID";
            return new QueryObject<Account>(_databaseInstance, sql, new { openID = openID }).FirstOrDefault();
        }


        public Account GetByorangeKey(string orangeKey)
        {
            string sql = "select * from fz_Accounts where orangeKey=@orangeKey";
            return new QueryObject<Account>(_databaseInstance, sql, new { orangeKey = orangeKey }).FirstOrDefault();
        }

        public   DataTables<AccountModel> Query(AccountRequest request)
        {

            SqlParameter[] param = { 
                                     new SqlParameter("@DisplayName",request.DisplayName),
                                     new SqlParameter("@Mobile",request.Mobile),
                                     new SqlParameter("@startTime",request.startTime),
                                     new SqlParameter("@endTime",request.endTime),
                                     new SqlParameter("@pageIndex",request.PageIndex),
                                     new SqlParameter("@pageSize",request.PageSize),
                                   };
            StoreProcedure sp = new StoreProcedure("P_getAccounts", param);
            return _databaseInstance.GetTables<AccountModel>(sp);
        }

        public decimal SaleAmount(int id, int type)
        {
            decimal amount = 0;
            SqlParameter[] param = { 
                                     new SqlParameter("@Id",id),
                                     new SqlParameter("@type",type),
                                   };
            StoreProcedure sp = new StoreProcedure("P_getSalesAmount", param);
            var data = _databaseInstance.GetTables<AmountModel>(sp);
            if (data != null && data.ModelList != null)
            {
               var item= data.ModelList.FirstOrDefault();
               if (item != null)
                   amount = item.amount;
            }
            return amount;
        }


        public DataTables<AccountSale> GetSaleTeam(AccountSaleRequest request)
        {
            SqlParameter[] param = { 
                                     new SqlParameter("@Id",request.Id),
                                     new SqlParameter("@type",request.type),
                                     new SqlParameter("@pageIndex",request.PageIndex),
                                     new SqlParameter("@pageSize",request.PageSize),
                                   };
            StoreProcedure sp = new StoreProcedure("P_getSalesTeam", param);
            return _databaseInstance.GetTables<AccountSale>(sp);
        }


        public Account GetByMobile(string Mobile)
        {
            string sql = "select * from fz_Accounts where Mobile=@Mobile";
            return new QueryObject<Account>(_databaseInstance, sql, new {Mobile=Mobile }).FirstOrDefault();
        }

        public int GetSalerCount(int accountId)
        {
            int total = 0;
            string sql = "select COUNT(1) as total from fz_Accounts where salerId=@salerId and grade in (0,2,3)";
            var count = _databaseInstance.ExecuteScalar(sql, new { salerId = accountId }).ToString();
            if (!string.IsNullOrWhiteSpace(count))
            {
                int.TryParse(count, out total);
            }
            return total;
        }
    }

   public class AmountModel
   {
       public decimal amount { get; set; }
   }
}
