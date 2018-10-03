using Ecard.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.SqlServices
{
    public class SqlOrangeKeyAndopenIDService : IOrangeKeyAndopenIDService
    {
         private readonly DatabaseInstance _databaseInstance;
         private const string TableName = "OrangeKeyAndopenIDs";

         public SqlOrangeKeyAndopenIDService(DatabaseInstance databaseInstance)
         {
             _databaseInstance = databaseInstance;
         }
        public int Insert(Models.OrangeKeyAndopenID item)
        {
            return _databaseInstance.Insert(item,TableName);
        }

        public int Update(Models.OrangeKeyAndopenID item)
        {
            return _databaseInstance.Update(item, TableName);
        }

        public int Delete(Models.OrangeKeyAndopenID item)
        {
            return _databaseInstance.Delete(item, TableName);
        }

        public Models.OrangeKeyAndopenID GetByopenID(string openID)
        {
            string sql = "select * from OrangeKeyAndopenIDs where openID=@openID";
            return new QueryObject<Models.OrangeKeyAndopenID>(_databaseInstance, sql, new { openID = openID }).FirstOrDefault();
        }

        public Models.OrangeKeyAndopenID GetByorangeKey(string orangeKey)
        {
            string sql = "select * from OrangeKeyAndopenIDs where orangeKey=@orangeKey";
            return new QueryObject<Models.OrangeKeyAndopenID>(_databaseInstance, sql, new { orangeKey = orangeKey }).FirstOrDefault();
        }

        public Models.OrangeKeyAndopenID GetById(int id)
        {
            return _databaseInstance.GetById<Models.OrangeKeyAndopenID>(TableName, id);
        }


        public Models.OrangeKeyAndopenID GetBymessageId(string messageId)
        {
            string sql = "select * from OrangeKeyAndopenIDs where messageId=@messageId";
            return new QueryObject<Models.OrangeKeyAndopenID>(_databaseInstance, sql, new { messageId = messageId }).FirstOrDefault();
        }
    }
}
