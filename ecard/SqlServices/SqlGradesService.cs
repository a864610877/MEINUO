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
    public class SqlGradesService : IGradesService
    {
        private readonly DatabaseInstance _databaseInstance;
        private const string TableName = "fz_Grades";

        public SqlGradesService(DatabaseInstance databaseInstance)
         {
             _databaseInstance = databaseInstance;
         }
        public int Insert(Models.Grades item)
        {
            return _databaseInstance.Insert(item, TableName);
        }

        public int Update(Models.Grades item)
        {
            return _databaseInstance.Update(item, TableName);
        }

        public int Delete(Models.Grades item)
        {
            return _databaseInstance.Delete(item, TableName);
        }

        public Models.Grades GetById(int id)
        {
            return _databaseInstance.GetById<Models.Grades>(TableName, id);
        }

        public QueryObject<Models.Grades> QueryAll()
        {
            string sql = "select * from fz_Grades";
            return new QueryObject<Models.Grades>(_databaseInstance, sql, null);
        }

        public Infrastructure.DataTables<Models.Grades> Query(Requests.GradesRequest request)
        {
            SqlParameter[] param = { 
                                     new SqlParameter("@name",request.name),
                                     new SqlParameter("@pageIndex",request.PageIndex),
                                     new SqlParameter("@pageSize",request.PageSize),
                                   };
            StoreProcedure sp = new StoreProcedure("P_getGrades", param);
            return _databaseInstance.GetTables<Grades>(sp);
        }
    }
}
