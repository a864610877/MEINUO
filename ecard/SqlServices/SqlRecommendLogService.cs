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
    public class SqlRecommendLogService : IRecommendLogService
    {
         private readonly DatabaseInstance _databaseInstance;
         private const string TableName = "fz_RecommendLogs";

         public SqlRecommendLogService(DatabaseInstance databaseInstance)
         {
             _databaseInstance = databaseInstance;
         }
        public int Insert(RecommendLog item)
        {
            return _databaseInstance.Insert(item,TableName);
        }

        public int Update(RecommendLog item)
        {
            return _databaseInstance.Update(item, TableName);
        }

        public int Delete(RecommendLog item)
        {
            return _databaseInstance.Delete(item, TableName);
        }

        public Models.RecommendLog GetById(int id)
        {
            return _databaseInstance.GetById<Models.RecommendLog>(TableName, id);
        }

        public DataTables<RecommendLogModel> Query(MemberRecommendLogRequest request)
        {

            SqlParameter[] param = { 
                                     new SqlParameter("@salerName",request.salerName),
                                     new SqlParameter("@userName",request.userName),
                                     new SqlParameter("@startTime",request.startTime),
                                     new SqlParameter("@endTime",request.endTime),
                                     new SqlParameter("@pageIndex",request.PageIndex),
                                     new SqlParameter("@pageSize",request.PageSize),
                                   };
            StoreProcedure sp = new StoreProcedure("P_getRecommendLogs", param);
            return _databaseInstance.GetTables<RecommendLogModel>(sp);
        }


        public DataTables<RecommendLog> MemberQuery(MemberRecommendLogRequest request)
        {
            SqlParameter[] param = { 
                                     new SqlParameter("@salerId",request.salerId),
                                     new SqlParameter("@startTime",request.startTime),
                                     new SqlParameter("@endTime",request.endTime),
                                     new SqlParameter("@pageIndex",request.PageIndex),
                                     new SqlParameter("@pageSize",request.PageSize),
                                   };
            StoreProcedure sp = new StoreProcedure("P_getMemberRecommendLog", param);
            return _databaseInstance.GetTables<RecommendLog>(sp);
        }


        public int RecommendCount(string UserName)
        {
            string sql = "select count(*) from fz_RecommendLogs where salerName=@salerName";
            var item=_databaseInstance.ExecuteScalar(sql, new { salerName = UserName }).ToString();
            int count = 0;
            int.TryParse(item, out count);
            return count;
        }
    }
}
