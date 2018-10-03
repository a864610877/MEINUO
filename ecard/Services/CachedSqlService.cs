using System.Collections.Generic;
using System.Linq;
using Ecard.Models;

namespace Ecard.Services
{
    public abstract class CachedSqlService<T>
        where T :   IKeySetter
    {
        private readonly DatabaseInstance _databaseInstance;
        protected abstract string TableName { get; }
        static List<T> _cache ;
        private static object _locker = new object();
        protected CachedSqlService(DatabaseInstance databaseInstance)
        {
            _databaseInstance = databaseInstance;
        }

        public virtual IEnumerable<T> Query()
        {
            var items = _cache;
            if (items == null)
                lock (_locker)
                {
                    items = _cache;
                    if (items == null)
                    {
                        items = new QueryObject<T>(_databaseInstance, "select * from " + TableName, null).ToList();
                        _cache = items;
                    }
                }
            return items.AsEnumerable();
        }

        public void Create(T item)
        {
            item.Id = _databaseInstance.Insert(item, TableName);
            _cache = null;
        }

        public T GetById(int id)
        {
            return Query().FirstOrDefault(x => x.Id == id);
        }

        public void Update(T item)
        {
            _databaseInstance.Update(item, TableName);
            _cache = null;
        }

        public void Delete(T item)
        {
            _databaseInstance.Delete(item, TableName);
            _cache = null;
        }
    }
}