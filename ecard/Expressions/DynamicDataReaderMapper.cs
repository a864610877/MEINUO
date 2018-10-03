using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Oxite.Expressions
{
    public class DynamicDataReaderMapper
    {
        private readonly Type _type;
        private readonly IMetadateAnalyzer _metadateAnalyzer;
        public Action<IDataRecord, object> FromDataReader;
        List<Action<IDataRecord, object>> _setFields = new List<Action<IDataRecord, object>>();

        public DynamicDataReaderMapper(Type type)
            : this(type, new DefaultMetadateAnalyzer())
        {
        }

        public DynamicDataReaderMapper(Type type, IMetadateAnalyzer metadateAnalyzer)
        {
            _type = type;
            _metadateAnalyzer = metadateAnalyzer;

            BuildFromDataReader();
        }

        private void BuildFromDataReader()
        {
            IList<ColumnMetadate> columns = _metadateAnalyzer.GetColumns(_type);

            foreach (var column in columns)
            {
                ParameterExpression instance = Expression.Parameter(typeof(object), "instance");
                ParameterExpression arg = Expression.Parameter(typeof(object), "arg");
                var instanceCast = Expression.Convert(instance, _type);
                var property = _type.GetProperty(column.PropertyName);

                // convert.toXXXX
                var convertMethod = typeof(Convert).GetMethod("To" + property.PropertyType.Name,
                                                              BindingFlags.Static | BindingFlags.Public, null,
                                                              new Type[] { typeof(object) }, null);
                var callConvertMethod = Expression.Call(null, convertMethod, arg);

                //var convertArg = Expression.Convert(arg, property.DeclaringType);
                var bind = Expression.Call(instanceCast, property.GetSetMethod(), callConvertMethod);
                var columnName = column.ColumnName;

                var setValue = LambdaExpression.Lambda<Action<object, object>>(bind, instance, arg).
                    Compile();

                _setFields.Add((r, x) =>
                                   {
                                       object readerObject = r[columnName];
                                       if (!Convert.IsDBNull(readerObject))
                                       {
                                           setValue(x, readerObject);
                                       }
                                   });
            }
            FromDataReader = (r, x) =>
                                 {
                                     foreach (var action in _setFields)
                                     {
                                         action(r, x);
                                     }
                                 };
        }
    }
    public static class DynamicDataReaderMapperCache
    {
        static Dictionary<Type, DynamicDataReaderMapper> _map = new Dictionary<Type, DynamicDataReaderMapper>(100);
        public static void Convert<T>(IDataReader reader, T t) where T : class
        {
            if (!_map.ContainsKey(typeof(T)))
            {
                lock (_map)
                {
                    if (!_map.ContainsKey(typeof(T)))
                    {
                        _map.Add(typeof(T), new DynamicDataReaderMapper(typeof(T)));
                    }
                }
            }
            _map[typeof(T)].FromDataReader(reader, t);
        }
    }
}