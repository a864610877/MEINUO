using Ecard.Infrastructure;
using Ecard.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Ecard.SqlServices
{
    public class SqlCommoditySalesService : ICommoditySalesService
    {
         private readonly DatabaseInstance _databaseInstance;
         private const string TableName = "";

         public SqlCommoditySalesService(DatabaseInstance databaseInstance)
         {
             _databaseInstance = databaseInstance;
         }
        //public int Insert(Models.CommoditySales item)
        //{
        //    throw new NotImplementedException();
        //}

        //public int CanCel(int commoditySalesId)
        //{
        //    throw new NotImplementedException();
        //}

         public Infrastructure.DataTables<CommoditySalesStatistics> Query(Requests.CommoditySalesStatisticsRequest request)
         {
             SqlParameter[] param = { 
                                      new SqlParameter("@name",""),
                                      new SqlParameter("@pageIndex",request.PageIndex),
                                      new SqlParameter("@pageSize",request.PageSize),
                                   };
             StoreProcedure sp = new StoreProcedure("P_getCommoditySales", param);
             return _databaseInstance.GetTables<CommoditySalesStatistics>(sp);
         }

    }
}
