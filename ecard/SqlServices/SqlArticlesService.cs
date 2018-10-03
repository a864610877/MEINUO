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
    public class SqlArticlesService : IArticlesService
    {
        private readonly DatabaseInstance _databaseInstance;
        private const string TableName = "fz_Articles";

        public SqlArticlesService(DatabaseInstance databaseInstance)
         {
             _databaseInstance = databaseInstance;
         }
        public int Insert(Models.Articles item)
        {
            return _databaseInstance.Insert(item, TableName);
        }

        public int Update(Models.Articles item)
        {
            return _databaseInstance.Update(item, TableName);
        }

        public int Delete(Models.Articles item)
        {
            return _databaseInstance.Delete(item, TableName);
        }

        public Models.Articles GetById(int id)
        {
            return _databaseInstance.GetById<Models.Articles>(TableName, id);
        }

        public QueryObject<Models.Articles> QueryAll()
        {
            string sql = "select * from fz_Articles order by submitTime desc";
            return new QueryObject<Models.Articles>(_databaseInstance, sql, null);
        }

        public Infrastructure.DataTables<Models.Articles> Query(Requests.ArticlesRequest request)
        {
            SqlParameter[] param = { 
                                     new SqlParameter("@title",request.title),
                                     new SqlParameter("@pageIndex",request.PageIndex),
                                     new SqlParameter("@pageSize",request.PageSize),
                                   };
            StoreProcedure sp = new StoreProcedure("P_getArticles", param);
            return _databaseInstance.GetTables<Articles>(sp);
        }
    }
}
