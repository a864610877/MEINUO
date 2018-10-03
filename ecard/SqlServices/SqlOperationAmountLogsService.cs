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
    public class SqlOperationAmountLogsService : IOperationAmountLogsService
    {
        private readonly DatabaseInstance _databaseInstance;
        private const string TableName = "fz_OperationAmountLogs";
        public SqlOperationAmountLogsService(DatabaseInstance databaseInstance, IAccountService IAccountService, ISiteService ISiteService)
        {
            _databaseInstance = databaseInstance;
        }
        public int Insert(Models.fz_OperationAmountLogs item)
        {
            return _databaseInstance.Insert(item, TableName);
        }



        public Infrastructure.DataTables<Models.fz_OperationAmountLogs> GetByUserId(OperationAmountLogUserIdRequest request)
        {
            SqlParameter[] param = { 
                                        new SqlParameter("@userId",request.userId),
                                        new SqlParameter("@startTime",request.startTime),
                                        new SqlParameter("@endTime",request.endTime),
                                        new SqlParameter("@pageIndex",request.PageIndex),
                                        new SqlParameter("@pageSize",request.PageSize),
                                       
                                   };
            StoreProcedure sp = new StoreProcedure("P_getOperationAmountLog", param);
            return _databaseInstance.GetTables<fz_OperationAmountLogs>(sp);
        }


        public int Update(fz_OperationAmountLogs item)
        {
            return _databaseInstance.Update(item, TableName);
        }


        public fz_OperationAmountLogs GetByUserId(int userId)
        {
            string sql = "select * from fz_OperationAmountLogs where userId=@userId";
            return new QueryObject<fz_OperationAmountLogs>(_databaseInstance, sql, new { userId = userId }).FirstOrDefault();
        }
    }
}
