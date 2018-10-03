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
    public class SqlOperationPointLogService : IOperationPointLogService
    {
         private readonly DatabaseInstance _databaseInstance;
         private const string TableName = "fz_OperationPointLogs";

         public SqlOperationPointLogService(DatabaseInstance databaseInstance)
         {
             _databaseInstance = databaseInstance;
         }
        public int Insert(Models.OperationPointLog item)
        {
            return _databaseInstance.Insert(item, TableName);
        }

        public int Update(Models.OperationPointLog item)
        {
            return _databaseInstance.Update(item, TableName);
        }

        public int Delete(Models.OperationPointLog item)
        {
            return _databaseInstance.Delete(item, TableName);
        }

        public Models.OperationPointLog GetById(int id)
        {
            return _databaseInstance.GetById<Models.OperationPointLog>(TableName, id);
        }

        public DataTables<OperationPointLogModel> Query(OperationPointLogRequest request)
        {

            SqlParameter[] param = { 
                                    new SqlParameter("@userId",request.userId),
                                     new SqlParameter("@startTime",request.startTime),
                                     new SqlParameter("@endTime",request.endTime),
                                     new SqlParameter("@pageIndex",request.PageIndex),
                                     new SqlParameter("@pageSize",request.PageSize),
                                   };
            StoreProcedure sp = new StoreProcedure("P_getOperationPointLogs", param);
            return _databaseInstance.GetTables<OperationPointLogModel>(sp);
        }
    }
}
