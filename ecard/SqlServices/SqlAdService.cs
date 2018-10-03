using Ecard.Infrastructure;
using Ecard.Models;
using Ecard.Requests;
using Ecard.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Ecard.SqlServices
{
    public class SqlAdService : IAdService
    {
         private readonly DatabaseInstance _databaseInstance;
         private const string TableName = "Ads";

         public SqlAdService(DatabaseInstance databaseInstance)
         {
             _databaseInstance = databaseInstance;
         }

         public int Insert(Models.Ad item)
         {
             return _databaseInstance.Insert(item,TableName);
         }

         public int Update(Models.Ad item)
         {
             return _databaseInstance.Update(item, TableName);
         }

         public int Delete(Models.Ad item)
         {
             return _databaseInstance.Delete(item, TableName);
         }

         public Models.Ad GetById(int id)
         {
             return _databaseInstance.GetById<Models.Ad>(TableName,id);
         }


         public QueryObject<Models.Ad> MicroMallQuery()
         {
             string sql = "select * from ads where [State]=@State order by rank asc";
             return new QueryObject<Models.Ad>(_databaseInstance, sql, new { State = AdStates.putaway});
         }


         public DataTables<Ad> Query(AdRequest request)
         {
             if (request.state == 0)
             {
                 request.state = null;
             }
             SqlParameter[] param = { 
                                     new SqlParameter("@title",request.title),
                                     new SqlParameter("@state",request.state),
                                     new SqlParameter("@startSubmitTime",request.startSubmitTime),
                                     new SqlParameter("@endSubmitTime",request.endSubmitTime),
                                     new SqlParameter("@pageIndex",request.PageIndex),
                                     new SqlParameter("@pageSize",request.PageSize),
                                   };
             StoreProcedure sp = new StoreProcedure("P_getAds", param);
             return _databaseInstance.GetTables<Ad>(sp);
         }
    }
}
