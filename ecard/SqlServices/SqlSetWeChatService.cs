using Ecard.Models;
using Ecard.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.SqlServices
{
    public class SqlSetWeChatService : ISetWeChatService
    {
         private readonly DatabaseInstance _databaseInstance;
         private const string TableName = "SetWeChats";

         public SqlSetWeChatService(DatabaseInstance databaseInstance)
         {
             _databaseInstance = databaseInstance;
         }
        public SetWeChat GetById(int id)
        {
            return _databaseInstance.GetById<SetWeChat>(TableName, id);
        }

        public int Update(SetWeChat item)
        {
            return _databaseInstance.Update(item, TableName);
        }


        public SetWeChat GeByFirst()
        {
            try
            {
                string sql = "select top 1 * from SetWeChats";
                return new QueryObject<SetWeChat>(_databaseInstance, sql, null).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                //_databaseInstance.Dispose();
            }
        }
    }
}
