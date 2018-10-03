using Ecard.Models;
using System;

namespace Ecard.Services
{
    public class SqlSiteService : ISiteService
    {
        private readonly DatabaseInstance _databaseInstance;
        private const string TableName = "Sites";

        public SqlSiteService(DatabaseInstance databaseInstance)
        {
            _databaseInstance = databaseInstance;
        }

        public QueryObject<Site> Query(SiteRequest request)
        {
            try
            {

                return new QueryObject<Site>(_databaseInstance, "select * from sites", request);
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
         

        public void Update(Site item)
        {
            _databaseInstance.Update(item, TableName);
        }

        public void Delete(Site item)
        {
            _databaseInstance.Delete(item, TableName);
        }
    }
}