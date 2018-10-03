using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Moonlit.Data;

namespace Ecard.Services
{
    public class QueryObject<T> : IEnumerable<T>
    {
        private readonly DatabaseInstance _databaseInstance;
        private readonly string _sql;
        private readonly object _parameterObject;

        public QueryObject(DatabaseInstance databaseInstance, string sql, object parameterObject)
        {
            _databaseInstance = databaseInstance;
            _sql = sql;
            _parameterObject = parameterObject;
        }

        public List<T> ToList()
        {
            return _databaseInstance.Query<T>(_sql, _parameterObject).ToList();
        } 

        public long Count()
        {
            return _databaseInstance.Count<T>(_sql, _parameterObject);
        }

        public IEnumerable<T> ToList(int skip, int take, string orderBy)
        {
            return _databaseInstance.Query<T>(_sql, _parameterObject, skip, take, orderBy).ToList();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _databaseInstance.Query<T>(_sql, _parameterObject).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}