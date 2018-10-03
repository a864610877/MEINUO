using Ecard.Models;
using Ecard.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.SqlServices
{
    public class SqlImageAdsService : IImageAdsService
    {
        private readonly DatabaseInstance _databaseInstance;
        private const string TableName = "Ads";

        public SqlImageAdsService(DatabaseInstance databaseInstance)
        {
            _databaseInstance = databaseInstance;
        }

        public QueryObject<Models.Ad> Query()
        {

             string sql = "select * from ads where [State]=@State order by rank asc";
             return new QueryObject<Models.Ad>(_databaseInstance, sql, new { State = AdStates.putaway});
         
        }
    }
}
