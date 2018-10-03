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
    public class SqlCommodityCategorysService : ICommodityCategorysService
    {
         private readonly DatabaseInstance _databaseInstance;
         private const string TableName = "fz_CommodityCategorys";

         public SqlCommodityCategorysService(DatabaseInstance databaseInstance)
         {
             _databaseInstance = databaseInstance;
         }
        public int Insert(Models.fz_CommodityCategorys item)
        {
            return _databaseInstance.Insert(item, TableName);
        }

        public int Update(Models.fz_CommodityCategorys item)
        {
            return _databaseInstance.Update(item, TableName);
        }

        public Models.fz_CommodityCategorys GetById(int id)
        {
            return _databaseInstance.GetById<fz_CommodityCategorys>(TableName, id);
            
        }

        public int Delete(Models.fz_CommodityCategorys item)
        {
            return _databaseInstance.Delete(item, TableName);
        }


        public Infrastructure.DataTables<fz_CommodityCategorys> Query(Requests.CommodityCategorysRequest request)
        {
            SqlParameter[] param = { 
                                     new SqlParameter("@name",request.name),
                                     new SqlParameter("@pageIndex",request.PageIndex),
                                     new SqlParameter("@pageSize",request.PageSize),
                                   };
            StoreProcedure sp = new StoreProcedure("Pro_getCommodityCategory", param);
            return _databaseInstance.GetTables<fz_CommodityCategorys>(sp);
        }


        public List<fz_CommodityCategorys> GetAll()
        {
            string sql = "select * from fz_CommodityCategorys";
            return new QueryObject<Models.fz_CommodityCategorys>(_databaseInstance, sql, null).ToList();
        }
    }
}
