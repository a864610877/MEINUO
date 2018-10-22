using Ecard.Infrastructure;
using Ecard.Models;
using Ecard.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Ecard.SqlServices
{
    public class SqlWithdrawService : IWithdrawService
    {
        private readonly DatabaseInstance _databaseInstance;
        private const string TableName = "fz_Withdraws";

        public SqlWithdrawService(DatabaseInstance databaseInstance)
        {
            _databaseInstance = databaseInstance;
        }

        public int Insert(Models.Withdraw item)
        {
            return _databaseInstance.Insert(item, TableName);
        }

        public int Update(Models.Withdraw item)
        {
            return _databaseInstance.Update(item, TableName);
        }

        public int Delete(Models.Withdraw item)
        {
            return _databaseInstance.Delete(item, TableName);
        }

        public Models.Withdraw GetById(int id)
        {
            return _databaseInstance.GetById<Models.Withdraw>(TableName, id);
        }


        public DataTables<WithdrawModel> Query(WithdrawRequest request)
        {

            if (request.state == 0)
            {
                request.state = null;
            }
            SqlParameter[] param = { 
                                     new SqlParameter("@Operator",request.Operator),
                                     new SqlParameter("@UserId",request.UserId),
                                     new SqlParameter("@state",request.state),
                                     new SqlParameter("@startTime",request.startTime),
                                     new SqlParameter("@endTime",request.endTime),
                                     new SqlParameter("@pageIndex",request.PageIndex),
                                     new SqlParameter("@pageSize",request.PageSize),
                                   };
            StoreProcedure sp = new StoreProcedure("P_getfWithdrawss", param);
            return _databaseInstance.GetTables<WithdrawModel>(sp);
        }

        public DataTables<Withdraw> Query(UserWithdrawRequest request, string openId)
        {

            SqlParameter[] param = { new SqlParameter("@openId",openId),
                                     new SqlParameter("@pageIndex",request.PageIndex),
                                     new SqlParameter("@pageSize",request.PageSize),
                                   };
            StoreProcedure sp = new StoreProcedure("P_getUserWithdrawDetail", param);
            return _databaseInstance.GetTables<Withdraw>(sp);
        }

        public decimal GetUserIdPoint(int userId)
        {
            string sql = string.Format("select sum(point) as point from fz_Withdraws where userId=@userId and state in ({0},{1})", WithdrawStates.notaudit, WithdrawStates.success);
            var point = _databaseInstance.ExecuteScalar(sql, new { userId = userId });
            if (point != null)
                return decimal.Parse(point.ToString());
            return 0;
        }
    }
}
