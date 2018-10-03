using Ecard.Models;
using Ecard.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.SqlServices
{
    public class SqlMessagesService : IMessagesService
    {
         private readonly DatabaseInstance _databaseInstance;
         private const string TableName = "Fz_Messages";

         public SqlMessagesService(DatabaseInstance databaseInstance)
         {
             _databaseInstance = databaseInstance;
         }
        public int Insert(Models.Fz_Messages item)
        {
            return _databaseInstance.Insert(item, TableName);
        }

        public int Update(Models.Fz_Messages item)
        {
            return _databaseInstance.Update(item, TableName);
        }

        public QueryObject<Models.Fz_Messages> GetSatySend()
        {
            string sql = "select * from Fz_Messages where [State]=@State order by submitTime asc";
            return new QueryObject<Models.Fz_Messages>(_databaseInstance, sql, new { State = MessagesState.staySend });
        }

    }
}
