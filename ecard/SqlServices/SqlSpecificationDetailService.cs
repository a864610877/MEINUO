using Ecard.Models;
using Ecard.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.SqlServices
{
    /// <summary>
    /// 接口实现方法
    /// </summary>
    public class SqlSpecificationDetailService : ISpecificationDetailService
    {
         private readonly DatabaseInstance _databaseInstance;
        private const string TableName = "fz_SpecificationDetails";

        public SqlSpecificationDetailService(DatabaseInstance databaseInstance)
        {
            _databaseInstance = databaseInstance;
        }
        public int Insert(Models.SpecificationDetail item)
        {
            return _databaseInstance.Insert(item,TableName);
        }

        public int Update(Models.SpecificationDetail item)
        {
            return _databaseInstance.Update(item, TableName);
        }

        public int Delete(Models.SpecificationDetail item)
        {
            return _databaseInstance.Delete(item, TableName);
        }

        public SpecificationDetail GetById(int specificationDetailId)
        {
            return _databaseInstance.GetById<SpecificationDetail>(TableName, specificationDetailId);
        }

        public QueryObject<SpecificationDetail> GetByspecificationId(int specificationId)
        {
            string sql = "select * from fz_SpecificationDetails where specificationId=@specificationId";
            return new QueryObject<SpecificationDetail>(_databaseInstance, sql, new { specificationId = specificationId });
        }
    }
}
