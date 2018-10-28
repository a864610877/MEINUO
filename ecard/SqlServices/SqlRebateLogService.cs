using Ecard.Models;
using Ecard.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.SqlServices
{
    public class SqlRebateLogService : IRebateLogService
    { 
        
         private readonly DatabaseInstance _databaseInstance;
         private const string TableName = "fz_Rebate";

         public SqlRebateLogService(DatabaseInstance databaseInstance)
         {
             _databaseInstance = databaseInstance;
         }
        public List<Models.RebateLog> GetRebateLog(int accountId)
        {
            string sql = @"select * from (
select top 1000 r.rebateId,r.reateAmount,r.[type],u.DisplayName,u.userId,r.submitTime from fz_Rebate r
 join fz_Orders o on r.orderDetailId=o.orderId join Users u on u.UserId=o.userId where r.accountId=@accountId 
 union  all
select top 1000 r1.rebateId,r1.reateAmount,r1.[type],u1.DisplayName,r1.submitTime from fz_Rebate r1
 join PayOrder o1 on r1.orderDetailId=o1.id join Users u1 on u1.UserId=o1.userId where r1.accountId=@accountId) a order by a.submitTime desc";
            //string sql = "select top 1000 r.rebateId,r.reateAmount,r.[type],u.DisplayName,r.submitTime from fz_Rebate r join fz_Orders o on r.orderDetailId=o.orderId join Users u on u.UserId=o.userId where r.accountId=@accountId order by submitTime desc";
            return new QueryObject<RebateLog>(_databaseInstance, sql, new { accountId = accountId }).ToList();
        }
    }
}
