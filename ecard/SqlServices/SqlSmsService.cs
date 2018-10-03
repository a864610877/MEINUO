using Ecard.Models;

namespace Ecard.Services
{
    public class SqlSmsService : ISmsService
    {
        private readonly DatabaseInstance _databaseInstance;
        private const string TableName = "Sms";

        public SqlSmsService(DatabaseInstance databaseInstance)
        {
            _databaseInstance = databaseInstance;
        }

        public void Create(Sms item)
        {
            item.SmsId = _databaseInstance.Insert(item, TableName);
        }

        public Sms GetById(int id)
        {
            return _databaseInstance.GetById<Sms>(TableName, id);
        }

        public void Update(Sms item)
        {
            _databaseInstance.Update(item, TableName);
        }

        public void Delete(Sms item)
        {
            _databaseInstance.Delete(item, TableName);
        }
        public QueryObject<Sms> Query(int[] states)
        {
            string sql = @"select * from Sms where [State] in(@states)";
            return new QueryObject<Sms>(_databaseInstance, sql, new { states = states });
        }
    }
}