using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Ecard.Infrastructure
{
    public class DataTables<T>
    {
        private List<T> _modelList;
        public List<T> ModelList
        {
            get { return _modelList; }
            set { _modelList = value; }

        }

        private int _totalCount;
        public int TotalCount
        {
            get { return _totalCount; }
            set { _totalCount = value; }

        }
    }
    /// <summary>
    /// 存储过程参数
    /// </summary>
    public class StoreProcedure
    {
        public StoreProcedure(string _procedureName, SqlParameter[] _param)
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
