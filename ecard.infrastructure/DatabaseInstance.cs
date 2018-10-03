using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text.RegularExpressions;
using Ecard.Models;
using Moonlit;
using Moonlit.Data;
using Moonlit.Reflection;
using System.Reflection;
using System.Data.SqlClient;
using Ecard.Infrastructure;
using System.Configuration; 
namespace Ecard
{
    public class DatabaseInstance : IDisposable
    {
        private static ISqlStore _sqlStore;
        private readonly DbConnection _connection;
        private readonly Database _database;
        private DbTransaction _transaction;

        public DatabaseInstance(Database database)
        {
            _database = database;

            _connection = database.OpenConnection();
        }

        public static ISqlStore SqlStore
        {
            get
            {
                if (_sqlStore == null)
                {
                    _sqlStore = new NonSqlStore();
                }
                return _sqlStore;
            }
            set { _sqlStore = value; }
        }

        public bool InTransaction
        {
            get { return Transaction != null; }
        }

        public DbConnection Connection
        {
            get { return _connection; }
        }

        public DbTransaction Transaction
        {
            get { return _transaction; }
            set { _transaction = value; }
        }

        #region IDisposable Members

        public void Dispose()
        {
            using (Connection)
            {
                using (Transaction)
                {
                }
            }
        }

        #endregion

        public int Insert(Type itemType, object item, string tableName)
        {
            InsertCommandBuilder inserter = InsertCommandBuilder.Create(itemType);
            DbCommand command = inserter.Build(item, tableName, _database, Connection, Transaction);
            command.ExecuteNonQuery();
            return (int)inserter.GetKey(command);
        }

        public int Insert(object item, string tableName)
        {
            if (item == null) throw new ArgumentNullException("item");
            if (tableName == null) throw new ArgumentNullException("tableName");
            return Insert(item.GetType(), item, tableName);
        }

        public DataTable Table(string sqlName, object parameterObject, int skip, int take, string orderBy)
        {
            string sql = SqlStore.GetSql(sqlName);

            orderBy = orderBy.ToLower();
            bool asc = true;
            if (orderBy.EndsWith("desc"))
            {
                orderBy = orderBy.Substring(0, orderBy.Length - 4);
                asc = false;
            }
            orderBy = orderBy.Replace(" ", "");
            while (orderBy.IndexOf(" ") >= 0)
            {
                orderBy = orderBy.Trim();
            }
            //            select *
            //from (
            //    select row_number()over(order by tempColumn)tempRowNumber,*
            //    from (select top 1510 tempColumn=0,* from u1club_bom_orders
            //    order by orderid desc
            //    )t
            //)tt
            sql =
                "select Convert(nvarchar(50), tt.rownum) rownumName, tt.* from (select row_number()over(order by {0}) rownum, * from(" +
                sql + ") t) tt where rownum between {1} and {2}";
            sql = string.Format(sql, orderBy + (asc ? "" : " desc"), skip + 1, skip + take);
            return Table(sql, parameterObject);
        }

        public DataTable Table(string sqlName, object parameterObject)
        {
            string sql = SqlStore.GetSql(sqlName);

            var dt = new DataTable();
            IEnumerable<DbParameter> parameters = BuildParameters(parameterObject, ref sql);
            DbDataAdapter adapter = _database.CreateDataAdapter(sql, Connection);
            DbCommand command = adapter.SelectCommand;
            if (Transaction != null)
                command.Transaction = Transaction;
            foreach (DbParameter dbParameter in parameters)
            {
                command.Parameters.Add(dbParameter);
            }
            adapter.Fill(dt);
            return dt;
        }

        public object ExecuteScalar(string sqlName, object parameterObject)
        {
            string sql = SqlStore.GetSql(sqlName);
            IEnumerable<DbParameter> parameters = BuildParameters(parameterObject, ref sql);
            DbCommand command = _database.CreateCommand(sql, Connection);

            if (Transaction != null)
                command.Transaction = Transaction;
            foreach (DbParameter dbParameter in parameters)
            {
                command.Parameters.Add(dbParameter);
            }
            return command.ExecuteScalar();
        }

        public long Count(string sql, object parameterObject)
        {
            sql = SqlStore.GetSql(sql);
            sql = string.Format("select count(*) from ({0}) t_count", sql);
            IEnumerable<DbParameter> parameters = BuildParameters(parameterObject, ref sql);
            DbCommand command = _database.CreateCommand(sql, Connection);

            command.Transaction = Transaction;
            foreach (DbParameter dbParameter in parameters)
            {
                command.Parameters.Add(dbParameter);
            }
            return Convert.ToInt64(command.ExecuteScalar() ?? 0);
        }


        public long Count<T>(string sql, object parameterObject)
        {
            sql = SqlStore.GetSql(sql);
            sql = string.Format("select count(*) from ({0}) t_count", sql);
            IEnumerable<DbParameter> parameters = BuildParameters(parameterObject, ref sql);
            DbCommand command = _database.CreateCommand(sql, Connection);

            command.Transaction = Transaction;
            foreach (DbParameter dbParameter in parameters)
            {
                command.Parameters.Add(dbParameter);
            }

            List<DiscriminatorAttribute> attrs = typeof(T).GetAttributes<DiscriminatorAttribute>(false);
            if (attrs.Count == 0)
                command.Parameters.Add(_database.CreateParameter("Discriminator", typeof(T).Name));
            else
                command.Parameters.Add(_database.CreateParameter("Discriminator", null));

            return Convert.ToInt64(command.ExecuteScalar() ?? 0);
        }

        public IEnumerable<T> Query<T>(string sqlName, object parameterObject, int skip, int take, string orderBy)
        {
            string sql = SqlStore.GetSql(sqlName);

            orderBy = orderBy.ToLower();
            bool asc = true;
            if (orderBy.EndsWith("desc"))
            {
                orderBy = orderBy.Substring(0, orderBy.Length - 4);
                asc = false;
            }
            orderBy = orderBy.Replace(" ", "");
            while (orderBy.IndexOf(" ") >= 0)
            {
                orderBy = orderBy.Trim();
            }
            //            select *
            //from (
            //    select row_number()over(order by tempColumn)tempRowNumber,*
            //    from (select top 1510 tempColumn=0,* from u1club_bom_orders
            //    order by orderid desc
            //    )t
            //)tt
            sql =
                "select Convert(nvarchar(50), tt.rownum) rownumName, tt.* from (select row_number()over(order by {0}) rownum, * from(" +
                sql + ") t) tt where rownum between {1} and {2}";
            sql = string.Format(sql, orderBy + (asc ? "" : " desc"), skip + 1, skip + take);
            return Query<T>(sql, parameterObject);
        }

        public IEnumerable<T> Query<T>(string sqlName, object parameterObject)
        {
            string sql = SqlStore.GetSql(sqlName);
            IEnumerable<DbParameter> parameters = BuildParameters(parameterObject, ref sql);
            DbCommand command = _database.CreateCommand(sql, Connection);

            command.Transaction = Transaction;

            foreach (DbParameter dbParameter in parameters)
            {
                command.Parameters.Add(dbParameter);
            }
            List<DiscriminatorAttribute> attrs = typeof(T).GetAttributes<DiscriminatorAttribute>(false);
            if (attrs.Count == 0)
                command.Parameters.Add(_database.CreateParameter("Discriminator", typeof(T).Name));
            else
                command.Parameters.Add(_database.CreateParameter("Discriminator", null));
            using (DbDataReader reader = command.ExecuteReader())
            {
                bool hasDiscriminator = HasDiscriminator(reader);
                while (reader.Read())
                {
                    Type type = typeof(T);
                    if (hasDiscriminator)
                        foreach (DiscriminatorAttribute attr in attrs)
                        {
                            if (string.Equals(attr.Discriminator, reader["Discriminator"].ToString(),
                                              StringComparison.OrdinalIgnoreCase))
                                type = attr.DiscriminatorType;
                        }
                    yield return (T)ReaderTranslator.Map(reader, Activator.CreateInstance(type));
                }
            }
        }

        private static bool HasDiscriminator(DbDataReader reader)
        {
            bool hasDiscriminator = false;
            DataTable tb = reader.GetSchemaTable();
            foreach (DataRow row in tb.Rows)
            {
                if (string.Equals("Discriminator", row[0].ToString(), StringComparison.OrdinalIgnoreCase))
                    hasDiscriminator = true;
            }
            return hasDiscriminator;
        }

        public int ExecuteNonQuery(string sqlName, object parameterObject, CommandType commandType = CommandType.Text)
        {
            string sql = SqlStore.GetSql(sqlName);

            IEnumerable<DbParameter> parameters = BuildParameters(parameterObject, ref sql);
            DbCommand command = _database.CreateCommand(sql, Connection);

            command.Transaction = Transaction;
            command.CommandType = commandType;
            foreach (DbParameter dbParameter in parameters)
            {
                command.Parameters.Add(dbParameter);
            }
            return command.ExecuteNonQuery();
        }

        private IEnumerable<DbParameter> BuildParameters(object parameterObject, ref string sql)
        {
            var parameters = new List<DbParameter>();
            if (parameterObject == null)
                return parameters;

            var dict = parameterObject as IDictionary;
            if (dict != null)
            {
                foreach (object key in dict.Keys)
                {
                    object value = dict[key];
                    string parameterName = key.ToString();
                    BuildParameter(parameterName, value, parameters, ref sql);
                }
                return parameters;
            }
            Type type = parameterObject.GetType();
            foreach (System.ComponentModel.PropertyDescriptor property in TypeDescriptor.GetProperties(type))
            {
                BuildParameter(property.Name, property.GetValue(parameterObject), parameters, ref sql);
            }
            return parameters;
        }

        private void BuildParameter(string parameterName, object value, List<DbParameter> parameters, ref string sql)
        {
            var items = value as IEnumerable;
            if (items != null && items.GetType() != typeof(string) && items.GetType() != typeof(byte[]))
            {
                value = null;
                var names = new List<string>();
                int index = 1;
                foreach (object obj in items)
                {
                    string tmpName = parameterName + "_" + index;
                    names.Add(tmpName);
                    parameters.Add(_database.CreateParameter(tmpName, obj));
                    index++;
                    value = true;
                }
                if (value != null)
                {
                    var regex = new Regex(@"\(\s*@" + parameterName + @"\s*\)", RegexOptions.IgnoreCase);
                    sql = regex.Replace(sql, @"(" + string.Join(",", names.Select(x => "@" + x).ToArray()) + ")");
                }
                else // value = null, no items in here
                {
                    var regex = new Regex(@"\(\s*@" + parameterName + @"\s*\)", RegexOptions.IgnoreCase);
                    sql = regex.Replace(sql, @"(null)");
                    value = true;
                }
            }
            parameters.Add(_database.CreateParameter(parameterName, value));
        }

        public T GetById<T>(string tableName, object id)
        {
            var inserter = new GetByIdCommandBuilder<T>(_database, Connection, tableName, Transaction);
            DbCommand command = inserter.Build(id);

            List<DiscriminatorAttribute> attrs = typeof(T).GetAttributes<DiscriminatorAttribute>(false);
            using (DbDataReader reader = command.ExecuteReader())
            {
                bool hasDiscriminator = HasDiscriminator(reader);
                while (reader.Read())
                {
                    Type type = typeof(T);
                    if (hasDiscriminator)
                        foreach (DiscriminatorAttribute attr in attrs)
                        {
                            if (string.Equals(attr.Discriminator, reader["Discriminator"].ToString(),
                                              StringComparison.OrdinalIgnoreCase))
                                type = attr.DiscriminatorType;
                        }
                    return (T)ReaderTranslator.Map(reader, Activator.CreateInstance(type));
                }
            }
            return default(T);
        }

        public int Update(object item, string tableName, params string[] updateFields)
        {
            var command = UpdateCommandBuilder.Create(item.GetType()).Build(item, _database, Connection, tableName, Transaction, updateFields);
            var count = command.ExecuteNonQuery();
            //if (item is IRecordVersion && count == 0)
            //    throw new ConflictException();
            return count;
        }

        public int Delete<T>(T item, string tableName)
        {
            var inserter = new DeleteCommandBuilder<T>(_database, Connection, tableName, Transaction);
            DbCommand command = inserter.Build(item);
            return command.ExecuteNonQuery();
        }

        public DbTransaction BeginTransaction()
        {
            Transaction = Connection.BeginTransaction();
            return Transaction;
        }

        public void Commit()
        {
            Transaction.Commit();
            Transaction = null;
        }

        /// <summary>
        /// 获取泛型集合
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="conn"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public  DataTables<T> GetTables<T>(StoreProcedure sp)
        {
            string str = ConfigurationManager.ConnectionStrings["ecard"].ConnectionString;
            SqlConnection conn = new SqlConnection(str);
            try
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = sp.ProcedureName;
                    if (sp.param != null)
                        cmd.Parameters.AddRange(sp.param);
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {

                        DataSet ds = new DataSet();
                        sda.Fill(ds);

                        if (ds == null || ds.Tables.Count <= 0)
                            return null;

                        DataTable dt = ds.Tables[0];
                        DataTables<T> tables = new DataTables<T>();
                        tables.ModelList = new List<T>();

                        if (dt.Rows.Count == 1 && dt.Columns.Count == 1 && dt.Columns["Total"] != null)
                        {
                            tables.TotalCount = Convert.ToInt32(dt.Rows[0]["Total"]);
                        }

                        tables.ModelList = (List<T>)DataSetToList<T>(ds, 1);
                        return tables;
                    }
                }
            }
            catch (Exception ex)
            {
               // Log.Error(ex.Message, ex);
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// DataSetToList
        /// </summary>
        /// <typeparam name="T">转换类型</typeparam>
        /// <param name="dataSet">数据源</param>
        /// <param name="tableIndex">需要转换表的索引</param>
        /// <returns></returns>
        public  IList<T> DataSetToList<T>(DataSet dataSet, int tableIndex)
        {
            //确认参数有效
            if (dataSet == null || dataSet.Tables.Count <= 0 || tableIndex < 0)
                return null;

            DataTable dt = dataSet.Tables[tableIndex];
            IList<T> list = new List<T>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //创建泛型对象
                T _t = Activator.CreateInstance<T>();
                //获取对象所有属性
                PropertyInfo[] propertyInfo = _t.GetType().GetProperties();
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    foreach (PropertyInfo info in propertyInfo)
                    {
                        //属性名称和列名相同时赋值
                        if (dt.Columns[j].ColumnName.ToUpper().Equals(info.Name.ToUpper()))
                        {
                            if (dt.Rows[i][j] != DBNull.Value)
                            {
                                string typeName = info.PropertyType.Name;
                                //switch (typeName)
                                //{
                                //    case "Int32":
                                //        info.SetValue(_t, Convert.ToInt32(dt.Rows[i][j]), null);
                                //        break;
                                //    case "DateTime":
                                //        info.SetValue(_t, Convert.ToDateTime(dt.Rows[i][j]), null);
                                //        break;
                                //    case "String":
                                //        info.SetValue(_t, dt.Rows[i][j], null);
                                //        break;
                                //    case "Nullable`1":
                                //        var value = dt.Rows[i][j];
                                //        try
                                //        {
                                //            if (value == null)
                                //            {
                                //                info.SetValue(_t, null, null);
                                //            }
                                //            else
                                //            {
                                //                info.SetValue(_t, Convert.ToInt32(dt.Rows[i][j]), null);
                                //            }
                                //        }
                                //        catch
                                //        {

                                //            if (value == null)
                                //            {
                                //                info.SetValue(_t, null, null);
                                //            }
                                //            else
                                //            {
                                //                info.SetValue(_t, Convert.ToDateTime(dt.Rows[i][j]), null);
                                //            }
                                //        }
                                //        break;
                                //    case "Boolean":
                                //        info.SetValue(_t, Convert.ToBoolean(dt.Rows[i][j]), null);
                                //        break;
                                //    case "Decimal":
                                //        info.SetValue(_t, Convert.ToDecimal(dt.Rows[i][j]), null);
                                //        break;
                                //}
                                try
                                {
                                    info.SetValue(_t, dt.Rows[i][j], null);
                                }
                                catch
                                {
                                    info.SetValue(_t, Convert.ToBoolean(dt.Rows[i][j]), null);
                                }
                            }
                            else
                            {
                                info.SetValue(_t, null, null);
                            }
                            break;
                        }
                    }
                }
                list.Add(_t);
            }
            return list;
        }
    }
}
