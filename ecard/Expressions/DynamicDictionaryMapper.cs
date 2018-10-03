using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;

namespace Oxite.Expressions
{
    public class DynamicDictionaryMapper
    {
        private readonly Type _type;
        private readonly IMetadateAnalyzer _metadateAnalyzer;
        public Func<object, IDictionary<string, object>> ToDictionary;
        List<Action<IDictionary<string, object>, object>> _setFields = new List<Action<IDictionary<string, object>, object>>();

        public DynamicDictionaryMapper(Type type)
            : this(type, new DefaultMetadateAnalyzer())
        {
        }

        public DynamicDictionaryMapper(Type type, IMetadateAnalyzer metadateAnalyzer)
        {
            _type = type;
            _metadateAnalyzer = metadateAnalyzer;

            BuildToDictionary();
        }

        private void BuildToDictionary()
        {
            IList<ColumnMetadate> columns = _metadateAnalyzer.GetColumns(_type);
            foreach (var column in columns)
            {
                ParameterExpression dict = Expression.Parameter(typeof(IDictionary<string, object>), "dict");
                ParameterExpression instance = Expression.Parameter(typeof(object), "instance");

                var propertyInfo = _type.GetProperty(column.PropertyName);
                var instanceCast = Expression.Convert(instance, _type);

                // (({TargetType})instance).{Property}
                Expression propertyAccess = Expression.Property(instanceCast, propertyInfo);

                // (object)((({TargetType})instance).{Property})
                UnaryExpression castPropertyValue = Expression.Convert(propertyAccess, typeof(object));

                var setItemMethod = typeof(IDictionary<string, object>).GetMethod("Add", new Type[] { typeof(string), typeof(object) });
                var setItem = Expression.Call(dict, setItemMethod, Expression.Constant(column.PropertyName), castPropertyValue);
                // Lambda expression
                Expression<Action<IDictionary<string, object>, object>> lambda =
                    Expression.Lambda<Action<IDictionary<string, object>, object>>(setItem, dict, instance);


                _setFields.Add(lambda.Compile());
            }
            ToDictionary = (x) =>
                               {
                                   IDictionary<string, object> dics = new IngoreCaseDictionary();
                                   foreach (var action in _setFields)
                                   {
                                       action(dics, x);
                                   }
                                   return dics;
                               };
        }
        public class IngoreCaseDictionary : IDictionary<string, object>
        {
            IDictionary<string, object> _hash = new Dictionary<string, object>();
            public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
            {
                return _hash.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public void Add(KeyValuePair<string, object> item)
            {
                _hash.Add(item.Key.ToLower(), item.Value);
            }

            public void Clear()
            {
                _hash.Clear();
            }

            public bool Contains(KeyValuePair<string, object> item)
            {
                return _hash.ContainsKey(item.Key.ToLower());
            }

            public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
            {
                _hash.CopyTo(array, arrayIndex);
            }

            public bool Remove(KeyValuePair<string, object> item)
            {
                return _hash.Remove(item.Key.ToLower());
            }

            public int Count
            {
                get { return _hash.Count; }
            }

            public bool IsReadOnly
            {
                get { return _hash.IsReadOnly; }
            }

            public bool ContainsKey(string key)
            {
                return _hash.ContainsKey(key.ToLower());
            }

            public void Add(string key, object value)
            {
                _hash.Add(key.ToLower(), value);
            }

            public bool Remove(string key)
            {
                return _hash.Remove(key.ToLower());
            }

            public bool TryGetValue(string key, out object value)
            {
                return _hash.TryGetValue(key.ToLower(), out value);
            }

            public object this[string key]
            {
                get
                {
                    key = key.ToLower();
                    return _hash[key];
                }
                set
                {
                    key = key.ToLower();
                    _hash[key] = value;
                }
            }

            public ICollection<string> Keys
            {
                get { return _hash.Keys; }
            }

            public ICollection<object> Values
            {
                get { return _hash.Values; }
            }
        }
    }
    public static class DynamicDictionaryMapperCache
    {
        static Dictionary<Type, DynamicDictionaryMapper> _map = new Dictionary<Type, DynamicDictionaryMapper>(100);
        public static IDictionary<string, object> GetDicionary(object item)
        {
            var type = item.GetType();
            if (!_map.ContainsKey(type))
            {
                lock (_map)
                {
                    if (!_map.ContainsKey(type))
                    {
                        _map.Add(type, new DynamicDictionaryMapper(type));
                    }
                }
            }
            return _map[type].ToDictionary(item);
        }
    }
}