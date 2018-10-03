using Ecard.Models;
using Ecard.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.SqlServices
{
    public class SqlProvinceService : IProvinceService
    {
         private readonly DatabaseInstance _databaseInstance;
         private const string TableName = "Provinces";

         public SqlProvinceService(DatabaseInstance databaseInstance)
         {
             _databaseInstance = databaseInstance;
         }
        public QueryObject<Province> Query()
        {
            string sql = "select * from Provinces";
            return new QueryObject<Province>(_databaseInstance, sql, null);
        }

        public Province GetById(int provinceId)
        {
            return _databaseInstance.GetById<Province>(TableName, provinceId);
        }
    }
}
