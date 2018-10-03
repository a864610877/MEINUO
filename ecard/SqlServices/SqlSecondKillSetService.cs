using Ecard.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.SqlServices
{
    public class SqlSecondKillSetService : ISecondKillSetService
    {
         private readonly DatabaseInstance _databaseInstance;
         private const string TableName = "SecondKillSet";

         public SqlSecondKillSetService(DatabaseInstance databaseInstance)
         {
             _databaseInstance = databaseInstance;
         }
        public Models.SecondKillSet GetFirst()
        {
            string sql = "select top 1 * from SecondKillSet";
            return new QueryObject<Models.SecondKillSet>(_databaseInstance, sql, null).FirstOrDefault();
        }

        public void Update(Models.SecondKillSet item)
        {
            _databaseInstance.Update(item,TableName);
        }
    }
}
