using Ecard.Models;
using Ecard.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.SqlServices
{
    public class SqlCityService : ICityService
    {
         private readonly DatabaseInstance _databaseInstance;
         private const string TableName = "Citys";

         public SqlCityService(DatabaseInstance databaseInstance)
         {
             _databaseInstance = databaseInstance;
         }

         public QueryObject<City> Query(int provinceId)
         {
             try
             {
                 string sql = "select * from Citys where ProvinceId=@ProvinceId";
                 return new QueryObject<City>(_databaseInstance, sql, new { ProvinceId = provinceId });
             }
             catch(Exception ex)
             {
                 return null;
             }
             finally
             {
                 //_databaseInstance.Dispose();
             }
         }

         public City GetById(int cityId)
         {
             return _databaseInstance.GetById<City>(TableName, cityId);
         }
    }
}
