using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using log4net;
using System.Reflection;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Ecard
{
    public abstract class DBHelper
    {
        //获取数据库连接字符串，其属于静态变量且只读，项目中所有文档可以直接使用，但不能修改
        /// <summary>
        /// 
        /// </summary>
        public static readonly string ConnectionStringLocalTransaction = ConfigurationManager.ConnectionStrings["ecard"].ConnectionString;
        private static ILog _log = log4net.LogManager.GetLogger(typeof(DBHelper));
        //public static SqlConnection con; //定义数据库连接对象

        #region Connection属性
        public static SqlConnection connection
        {
            get
            {
                string connectionString = ConnectionStringLocalTransaction;
                //if (con == null)
                //{
                    SqlConnection con = new SqlConnection(connectionString);
                    con.Open();
                //}
                //else if (con.State == ConnectionState.Broken)
                //{
                //    con.Close();
                //    con.Open();
                //}
                //else if (con.State == ConnectionState.Closed)
                //{
                //    con.Open();
                //}
                return con;

            }

        }
        #endregion

        #region 储存过程执行支持事务
        /// <summary>
        ///  存储过程集合执行
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="st"></param>
        /// <param name="sp"></param>
        public static void ExecuteNonQuery(SqlConnection conn,SqlTransaction st,StoreProcedure[] sp)
        {
            try
            {
                //SqlTransaction st = connection.BeginTransaction();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.Transaction = st;
                cmd.CommandType = CommandType.StoredProcedure;
                foreach (var item in sp)
                {
                    cmd.CommandText = item.ProcedureName;
                    cmd.Parameters.AddRange(item.param);
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }
                //st.Commit();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
            }
            finally
            {
                conn.Close();
            }
        }
        /// <summary>
        /// 执行单个存储过程(返回ID)
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="st"></param>
        /// <param name="sp"></param>
        /// <returns>返回最新ID</returns>
        public static int ExecuteNonQuery(SqlConnection conn, SqlTransaction st, StoreProcedure sp)
        {
            try
            {
                //SqlTransaction st = connection.BeginTransaction();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.Transaction = st;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = sp.ProcedureName;
                cmd.Parameters.AddRange(sp.param);
                var dr = cmd.ExecuteReader();
                 cmd.Parameters.Clear();
                //st.Commit();
                 int Id = -1;
                 while (dr.Read())
                 {
                     Id = Convert.ToInt32(dr[0]);
                 }
                 dr.Close();
                 return Id;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                return -1;
            }
            finally
            {
                if(st==null)
                   conn.Close();
            }
        }

        public static int ExecuteNonQuery2(SqlConnection conn, SqlTransaction st, StoreProcedure sp)
        {
            try
            {
                //SqlTransaction st = connection.BeginTransaction();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.Transaction = st;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = sp.ProcedureName;
                cmd.Parameters.AddRange(sp.param);
                var dr = cmd.ExecuteReader();
                cmd.Parameters.Clear();
                //st.Commit();
                int Id = -1;
                while (dr.Read())
                {
                    Id = Convert.ToInt32(dr[0]);
                }
                dr.Close();
                return Id;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                return -1;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// 执行单个存储过程
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="st"></param>
        /// <param name="sp"></param>
        /// <returns>收影响行数</returns>
        public static int ExecuteNonQuery1(SqlConnection conn, SqlTransaction st, StoreProcedure sp)
        {
            try
            {
                //SqlTransaction st = connection.BeginTransaction();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.Transaction = st;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = sp.ProcedureName;
                cmd.Parameters.AddRange(sp.param);
                var dr = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                //st.Commit();
                return dr;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                return -1;
            }
            finally
            {
                if(st==null)
                 conn.Close();
            }
        }
        #endregion

        #region 泛型

        /// <summary>
        /// 获取泛型集合
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="conn"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public  static IList<T> GetList<T>(SqlConnection conn, StoreProcedure sp)
        {
            try
            {
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
                        return DataSetToList<T>(ds, 0);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// 执行sql语句获取泛型集合
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="conn"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public static IList<T> GetSqlList<T>(SqlConnection conn, StoreProcedure sp)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sp.ProcedureName;
                    if (sp.param != null)
                        cmd.Parameters.AddRange(sp.param);
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {

                        DataSet ds = new DataSet();
                        sda.Fill(ds);
                        return DataSetToList<T>(ds, 0);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                return null;
            }
            finally
            {
                conn.Close();
            }
        }



        /// <summary>
        /// 获取泛型集合
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="conn"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public static DataTables<T> GetTables<T>(SqlConnection conn, StoreProcedure sp)
        {
            try
            {
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

                          if (ds == null || ds.Tables.Count <= 0 )
                             return null;

                          DataTable dt = ds.Tables[0];
                          DataTables<T> tables = new DataTables<T>();
                          tables.ModelList = new List<T>();

                          if (dt.Rows.Count == 1 && dt.Columns.Count == 1&&dt.Columns["Total"]!=null)
                          {
                              tables.TotalCount = Convert.ToInt32(dt.Rows[0]["Total"]);
                          }

                        tables.ModelList=(List<T>)DataSetToList<T>(ds, 1);
                         return tables;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
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
        public static IList<T> DataSetToList<T>(DataSet dataSet, int tableIndex)
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
                                switch (typeName)
                                { 
                                    case "Int32":
                                        info.SetValue(_t, Convert.ToInt32(dt.Rows[i][j]), null);
                                        break;
                                    case "DateTime":
                                         info.SetValue(_t, Convert.ToDateTime(dt.Rows[i][j]), null);
                                        break;
                                    case "String":
                                        info.SetValue(_t, dt.Rows[i][j], null);
                                        break ;
                                    case "Nullable`1":
                                        var value = dt.Rows[i][j];
                                        try
                                        {
                                            if (value == null)
                                            {
                                                info.SetValue(_t, null, null);
                                            }
                                            else
                                            {
                                                info.SetValue(_t, Convert.ToInt32(dt.Rows[i][j]), null);
                                            }
                                        }
                                        catch
                                        {

                                            if (value == null)
                                            {
                                                info.SetValue(_t, null, null);
                                            }
                                            else
                                            {
                                                info.SetValue(_t, Convert.ToDateTime(dt.Rows[i][j]), null);
                                            }
                                        }
                                        break;
                                    case "Boolean":
                                        info.SetValue(_t, Convert.ToBoolean(dt.Rows[i][j]), null);
                                        break;
                                    case "Decimal":
                                        info.SetValue(_t, Convert.ToDecimal(dt.Rows[i][j]), null);
                                        break;
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
        /// <summary>
        /// 获取泛型实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public static T GetModel<T>(SqlConnection conn, StoreProcedure sp)
        {
            try
            {
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
                        return DataSetModel<T>(ds, 0);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message,ex);
                return default(T);
            }
            finally
            {
               conn.Close();
            }
        }

        /// <summary>
        /// 获取泛型实体需事务支持
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public static T GetModel<T>(SqlConnection conn, SqlTransaction st,StoreProcedure sp)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = sp.ProcedureName;
                    cmd.Transaction = st;
                    if (sp.param != null)
                        cmd.Parameters.AddRange(sp.param);
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {

                        DataSet ds = new DataSet();
                        sda.Fill(ds);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            return DataSetModel<T>(ds, 0);
                        }
                        else
                        {
                            return default(T);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                return default(T);
            }
            finally
            {
                  conn.Close();
            }
        }

        /// <summary>
        /// 获取泛型实体需事务支持
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public static T SqlGetModel<T>(SqlConnection conn, SqlTransaction st, StoreProcedure sp)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sp.ProcedureName;
                    cmd.Transaction = st;
                    if (sp.param != null)
                        cmd.Parameters.AddRange(sp.param);
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {

                        DataSet ds = new DataSet();
                        sda.Fill(ds);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            return DataSetModel<T>(ds, 0);
                        }
                        else
                        {
                            return default(T);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                return default(T);
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">转换类型</typeparam>
        /// <param name="dataSet">数据源</param>
        /// <param name="tableIndex">需要转换表的索引</param>
        /// <returns>T</returns>
        public static T DataSetModel<T>(DataSet dataSet, int tableIndex)
        {
            //确认参数有效
            if (dataSet == null || dataSet.Tables.Count <= 0 || tableIndex < 0)
                return default(T);

            DataTable dt = dataSet.Tables[tableIndex];
            if (dt.Rows.Count <= 0)
            {
                return default(T);
            }

            //创建泛型对象
            T _t = Activator.CreateInstance<T>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
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
                                switch (typeName)
                                {
                                    case "Int32":
                                        info.SetValue(_t, Convert.ToInt32(dt.Rows[i][j]), null);
                                        break;
                                    case "DateTime":
                                        info.SetValue(_t, Convert.ToDateTime(dt.Rows[i][j]), null);
                                        break;
                                    case "String":
                                        info.SetValue(_t, Convert.ToString(dt.Rows[i][j]), null);
                                        break;
                                    case "Nullable`1":
                                        var value = dt.Rows[i][j];
                                        if (value == null)
                                        {
                                            info.SetValue(_t, null, null);
                                        }
                                        else
                                        {
                                            try
                                            {
                                                info.SetValue(_t, Convert.ToInt32(dt.Rows[i][j]), null);
                                            }
                                            catch
                                            {
                                                info.SetValue(_t, Convert.ToDateTime(dt.Rows[i][j]), null);
                                            }
                                            
                                        }
                                        break;
                                    case "Boolean":
                                        info.SetValue(_t, Convert.ToBoolean(dt.Rows[i][j]), null);
                                        break;
                                    case "Decimal":
                                        info.SetValue(_t, Convert.ToDecimal(dt.Rows[i][j]), null);
                                        break;
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
            }
            return _t;
        }







        private static void PrepareCommands(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText, SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
            {
                cmd.Transaction = trans;
            }
            cmd.CommandType = CommandType.Text;
            if (cmdParms != null)
            {
                foreach (SqlParameter parameter in cmdParms)
                {
                    if (((parameter.Direction == ParameterDirection.InputOutput) || (parameter.Direction == ParameterDirection.Input)) && (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }



        public static Dictionary<string, SqlDbType> GetEntityPropName<T>() where T : new()
        {
            T local = Activator.CreateInstance<T>();
            Dictionary<string, SqlDbType> dictionary = new Dictionary<string, SqlDbType>();
            Type type = typeof(T);
            bool flag = true;
            foreach (PropertyInfo info in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (flag)
                {
                    if (info.GetCustomAttributes(typeof(KeyAttribute), false).Length == 1)
                    {
                        flag = false;
                    }
                    else
                    {
                        dictionary.Add(info.Name.ToLower(), GetMsSqlType(info.PropertyType.Name));
                    }

                }
                else
                {

                    if (info.PropertyType.IsGenericType && info.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        dictionary.Add(info.Name.ToLower(), GetMsSqlType(info.PropertyType.GetGenericArguments()[0].Name));
                    }
                    else
                    {
                        dictionary.Add(info.Name.ToLower(), GetMsSqlType(info.PropertyType.Name));
                    }



                }



            }

            return dictionary;
        }

        public static Dictionary<string, object> GetEntityValue<T>(T d) where T : new()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            Type type = typeof(T);
            bool flag = true;
            foreach (PropertyInfo info in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (flag)
                {
                    if (info.GetCustomAttributes(typeof(KeyAttribute), false).Length == 1)
                    {
                        flag = false;
                    }
                    else
                    {
                        dictionary.Add(info.Name.ToLower(), info.GetValue(d, null));
                    }

                }
                else
                {

                    dictionary.Add(info.Name.ToLower(), info.GetValue(d, null));
                }
            }

            return dictionary;
        }






        public static Dictionary<string, SqlDbType> GetEntityPropNames<T>(out string id) where T : new()
        {
            id = string.Empty;
            T local = Activator.CreateInstance<T>();
            Dictionary<string, SqlDbType> dictionary = new Dictionary<string, SqlDbType>();
            Type type = typeof(T);
            bool flag = true;
            foreach (PropertyInfo info in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {

                if (flag)
                {
                    if (info.GetCustomAttributes(typeof(KeyAttribute), false).Length == 1)
                    {
                        flag = false;
                        id = info.Name.ToLower();
                    }

                }
                if (info.PropertyType.IsGenericType && info.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    dictionary.Add(info.Name.ToLower(), GetMsSqlType(info.PropertyType.GetGenericArguments()[0].Name));
                }
                else
                {
                    dictionary.Add(info.Name.ToLower(), GetMsSqlType(info.PropertyType.Name));
                }
            }

            return dictionary;
        }




      




  

        public static Dictionary<string, object> GetEntityValues<T>(T d) where T : new()
        {
         
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            Type type = typeof(T);
            bool flag = true;
            foreach (PropertyInfo info in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                //if (flag)
                //{
                //    if (info.GetCustomAttributes(typeof(KeyAttribute), false).Length == 1)
                //    {
                //        flag = false;
                //        id = info.Name.ToLower();
                //    }

                //}
                dictionary.Add(info.Name.ToLower(), info.GetValue(d, null));
              
            }

            return dictionary;
        }




        public static string StrColumnsAndPrams(Dictionary<string, SqlDbType> dicColumns, string ids)
        {
            string str = "";
            int num = 0;
        
            foreach (string str2 in dicColumns.Keys)
            {
                if (str2 != ids)
                {
                    if ((num + 1) == dicColumns.Count)
                    {
                        str = str + string.Format("{0}=@{0}", str2);
                    }
                    else
                    {
                        str = str + string.Format("{0}=@{0},", str2);
                    }
                }
                num++;
            }
            return str;
        }


        public static int ExecuteSql(string SQLString, params SqlParameter[] cmdParms)
        {
            int num2;
            using (SqlConnection connection = new SqlConnection(ConnectionStringLocalTransaction))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    try
                    {
                        PrepareCommand(command, connection, null, SQLString, cmdParms);
                        num2 = command.ExecuteNonQuery();
                        command.Parameters.Clear();

                    }
                    catch (SqlException ex)
                    {
                        Log.Error(ex.Message, ex);
                        throw ex;
                    }
                }
            }
            return num2;
        }

        /// <summary>
        /// 执行sql 语句返回主键
        /// </summary>
        /// <param name="SQLString"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public static object ExecuteSqlScalar(string SQLString, params SqlParameter[] cmdParms)
        {
            object num2;
            using (SqlConnection connection = new SqlConnection(ConnectionStringLocalTransaction))
            {
    
                using (SqlCommand command = new SqlCommand())
                {
                    try
                    {
                        PrepareCommand(command, connection, null, SQLString, cmdParms);
                        num2 = command.ExecuteScalar();
                        command.Parameters.Clear();

                    }
                    catch (SqlException ex)
                    {
                        Log.Error(ex.Message, ex);
                        throw ex;
                    }
                }
            }
            return num2;
        }



        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText, SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
            {
                cmd.Transaction = trans;
            }
            cmd.CommandType = CommandType.Text;
            if (cmdParms != null)
            {
                foreach (SqlParameter parameter in cmdParms)
                {
                    if (((parameter.Direction == ParameterDirection.InputOutput) || (parameter.Direction == ParameterDirection.Input)) && (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }
        public static void ExecuteSqlTran(Hashtable SQLStringList)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStringLocalTransaction))
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                SqlTransaction trans = connection.BeginTransaction();
                cmd.Transaction = trans;
                try
                {
                    foreach (DictionaryEntry entry in SQLStringList)
                    {
                        string cmdText = entry.Key.ToString();
                        SqlParameter[] cmdParms = (SqlParameter[])entry.Value;
                        PrepareCommand(cmd, connection, trans, cmdText, cmdParms);
                        object num = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                    }
                    trans.Commit();
                }
                catch(Exception ex)
                {
                    Log.Error(ex.Message, ex);
                    trans.Rollback();
                    throw;
                }
            }
        }


        public static void ExecuteSqlTran(MyHashTable SQLStringList, string id)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStringLocalTransaction))
            {
                 
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                SqlTransaction trans = connection.BeginTransaction();
                cmd.Transaction = trans;
                bool flag = false;
                int ids = 0;
                if (!string.IsNullOrEmpty(id))
                    flag = true;
                try
                {
                    foreach (string entry in SQLStringList.Keys)
                    {
                        string cmdText = entry;
                        SqlParameter[] cmdParms = (SqlParameter[])SQLStringList[entry];
                        if (ids > 0)
                        {
                            for (int i = 0; i < cmdParms.Length; i++)
                            {
                                if (cmdParms[i].ParameterName == id.Trim())
                                {
                                    cmdParms[i].Value=ids;
                                    ids = 0;
                                    flag = false;
                                }
                            }
                        }
                      
                        PrepareCommand(cmd, connection, trans, cmdText, cmdParms);
                        if (flag)
                        {
                            ids = Convert.ToInt32(cmd.ExecuteScalar());

                        }
                        else 
                        {
                            cmd.ExecuteNonQuery();
                        }
                         
                        cmd.Parameters.Clear();
                    }
                    trans.Commit();
                }
                catch(Exception ex)
                {
                    Log.Error(ex.Message, ex);
                    trans.Rollback();
                    throw;
                }
            }
        }


        public static int ExecuteSqlTran(Hashtable SQLStringList, Hashtable SQLPramsList)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStringLocalTransaction))
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                SqlTransaction trans = connection.BeginTransaction();
                cmd.Transaction = trans;
                try
                {
                    foreach (DictionaryEntry entry in SQLStringList)
                    {
                        string cmdText = entry.Value.ToString();
                        SqlParameter[] cmdParms = (SqlParameter[])SQLPramsList[entry.Key];
                        PrepareCommand(cmd, connection, trans, cmdText, cmdParms);
                        int num = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                          
                    }
                    trans.Commit();
                    return 1;
                }
                catch(Exception ex)
                {
                    Log.Error(ex.Message, ex);
                    trans.Rollback();
                    return -1;
                }
            }
        }


        public static SqlDbType GetMsSqlType(string PropertyType)
        {
            SqlDbType variant = SqlDbType.Variant;
            switch (PropertyType)
            {
                case "Int64":
                    return SqlDbType.BigInt;

                case "Object":
                    return SqlDbType.Binary;

                case "Boolean":
                    return SqlDbType.Bit;

                case "DateTime":
                    return SqlDbType.DateTime;

                case "Decimal":
                    return SqlDbType.Decimal;

                case "Double":
                    return SqlDbType.Float;

                case "Int32":
                    return SqlDbType.Int;

                case "Single":
                    return SqlDbType.Real;

                case "Int16":
                    return SqlDbType.SmallInt;

                case "Byte":
                    return SqlDbType.TinyInt;

                case "String":
                    return SqlDbType.VarChar;
              
            }
            return variant;
        }
        public static string StrColumns(Dictionary<string, SqlDbType> dicColumns)
        {
            string str = "";
            int num = 0;
            foreach (string str2 in dicColumns.Keys)
            {
                if ((num + 1) == dicColumns.Count)
                {
                    str = str + str2;
                }
                else
                {
                    str = str + str2 + ",";
                }
                num++;
            }
            return str;
        }


        public static string StrColumnsPrams(Dictionary<string, SqlDbType> dicColumns)
        {
            string str = "";
            int num = 0;
            foreach (string str2 in dicColumns.Keys)
            {
                if ((num + 1) == dicColumns.Count)
                {
                    str = str + "@" + str2;
                }
                else
                {
                    str = str + "@" + str2 + ",";
                }
                num++;
            }
            return str;
        }


        public static DataSet Query(string SQLString)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStringLocalTransaction))
            {
                DataSet dataSet = new DataSet();
                try
                {
                    connection.Open();


                    new SqlDataAdapter(SQLString, connection).Fill(dataSet, "ds");
                }
                catch (SqlException exception)
                {
                    throw new Exception(exception.Message);
                }
                return dataSet;
            }
        }



      
        #endregion

    }
    /// <summary>
    /// 存储过程参数
    /// </summary>
    public class StoreProcedure
    {
        public StoreProcedure(string _procedureName,SqlParameter[] _param)
        {
            this.ProcedureName = _procedureName;
            this.param = _param;
        }
        public StoreProcedure()
        { }
        public string ProcedureName { get; set; }

        public SqlParameter[] param { get; set; } 
    }
}
