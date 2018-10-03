using Ecard.Models;
using Moonlit.Data;
using Ecard.Infrastructure;
using System.Data.SqlClient;

namespace Ecard.Services
{
    public class SqlLogService : ILogService
    {
        private readonly DatabaseInstance _databaseInstance;
        private const string TableName = "Logs";

        public SqlLogService(DatabaseInstance databaseInstance)
        {
            _databaseInstance = databaseInstance;
        }

        public QueryObject<Log> Query(LogRequest request)
        {
            return new QueryObject<Log>(_databaseInstance, "Log.query", request);
        }
        public DataTables<Log> NewQuery(LogRequest request)
        {
            SqlParameter[] param = { 
                                   new SqlParameter("@UserName",request.UserName),
                                   new SqlParameter("@ContentWith",request.ContentWith),
                                   new SqlParameter("@pageIndex",request.PageIndex),
                                   new SqlParameter("@pageSize",request.PageSize),
                                   new SqlParameter("@LogType",request.LogType) 
                                   };
            StoreProcedure sp = new StoreProcedure("P_getLogs", param);
            return _databaseInstance.GetTables<Log>(sp);
        }
        public void Create(Log item)
        {
            item.LogId = _databaseInstance.Insert(item, TableName);
        }

        public Log GetById(int id)
        {
            return _databaseInstance.GetById<Log>(TableName, id);
        }

        public void Update(Log item)
        {
            _databaseInstance.Update(item, TableName);
        }

        public void Delete(Log item)
        {
            _databaseInstance.Delete(item, TableName);
        }
    }
}