using Ecard.Infrastructure;
using Ecard.Models;
using Ecard.Requests;
using Ecard.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Ecard.SqlServices
{
    /// <summary>
    /// 商品规格接口实现方法
    /// </summary>
    public class SqlSpecificationService : ISpecificationService
    {
        private readonly DatabaseInstance _databaseInstance;
        private const string TableName = "fz_Specifications";

        public SqlSpecificationService(DatabaseInstance databaseInstance)
        {
            _databaseInstance = databaseInstance;
        }
        public int Insert(Specification item)
        {
            return _databaseInstance.Insert(item, TableName);
        }

        public int Update(Specification item)
        {
            return _databaseInstance.Update(item, TableName);
        }

        public int Delete(Specification item)
        {
            return _databaseInstance.Delete(item, TableName);
        }

        public object QueryIdentity()
        {
            string sql = "select @@identity";
            return _databaseInstance.ExecuteScalar(sql, null);
        }

        public Specification GetById(int specificationId)
        {
            return _databaseInstance.GetById<Specification>(TableName, specificationId);
        }
        public QueryObject<Specification> QueryAll()
        {
            var sql = @"select * from fz_Specifications";
            return new QueryObject<Specification>(_databaseInstance, sql, null);
        }

        public SpecificationAndSpecificationDetail GetSpecificationAndSpecificationDetailById(int specificationId)
        {
            var item = _databaseInstance.GetById<Specification>(TableName, specificationId);
            if (item == null)
                return null;
            string sql = "select * from fz_SpecificationDetails where specificationId=@specificationId";
            var list = new QueryObject<SpecificationDetail>(_databaseInstance, sql, new { specificationId = specificationId }).ToList();
            SpecificationAndSpecificationDetail model= new SpecificationAndSpecificationDetail();
            model.model = item;
            model.list = list;
            return model;

        }

        public DataTables<Specification> Query(SpecificationRequest request)
        {
            SqlParameter[] param = { 
                                     new SqlParameter("@Name",request.Name),
                                     new SqlParameter("@pageIndex",request.PageIndex),
                                     new SqlParameter("@pageSize",request.PageSize),
                                   };
            StoreProcedure sp = new StoreProcedure("P_getSpecification", param);
            return _databaseInstance.GetTables<Specification>(sp);
        }


       
    }
}
