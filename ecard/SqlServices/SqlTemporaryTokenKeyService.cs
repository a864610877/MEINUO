using System.Data;

using System.Linq;
using Ecard.Models;
using Moonlit.Data;

namespace Ecard.Services
{
    public class SqlTemporaryTokenKeyService : ITemporaryTokenKeyService
    {
        private readonly DatabaseInstance _databaseInstance;
        private const string TableName = "TemporaryTokenKeys";

        public SqlTemporaryTokenKeyService(DatabaseInstance databaseInstance)
        {
            _databaseInstance = databaseInstance;
        }

        public QueryObject<TemporaryTokenKey> QueryByUser(int tokenKeyType, string userName)
        {
            return new QueryObject<TemporaryTokenKey>(_databaseInstance, "TemporaryTokenKey.queryByUser", new { TokenKeyType = tokenKeyType, userName = userName });
        }

        public void Create(TemporaryTokenKey item)
        {
            item.TemporaryTokenKeyId = _databaseInstance.Insert(item, TableName);
        }

        public TemporaryTokenKey GetById(int id)
        {
            return _databaseInstance.GetById<TemporaryTokenKey>(TableName, id);
        }

        public TemporaryTokenKey GetToken(string token, int tokenType)
        {
            return new QueryObject<TemporaryTokenKey>(_databaseInstance, "TemporaryTokenKey.GetToken", new { Token = token, TokenKeyType = tokenType }).FirstOrDefault();
        }
        public void Update(TemporaryTokenKey item)
        {
            _databaseInstance.Update(item, TableName);
        }

        public void Delete(TemporaryTokenKey item)
        {
            _databaseInstance.Delete(item, TableName);
        }

    }
}