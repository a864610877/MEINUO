using Ecard.Models;
using Ecard.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.SqlServices
{
    public class SqlPayOrderService : IPayOrderService
    {
        private readonly DatabaseInstance _databaseInstance;
        private const string TableName = "PayOrder";
        public SqlPayOrderService(DatabaseInstance databaseInstance)
        {
            _databaseInstance = databaseInstance;
        }
        public int Insert(PayOrder item)
        {
            return _databaseInstance.Insert(item, TableName);
        }
        public int Update(PayOrder item)
        {
            return _databaseInstance.Update(item, TableName);
        }
    }
}
