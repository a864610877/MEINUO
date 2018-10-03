using Ecard.Infrastructure;
using Ecard.Models;
using Ecard.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Ecard.SqlServices
{
    public class SqlSecondKillCommoditysService : ISecondKillCommoditysService
    {
        private readonly DatabaseInstance _databaseInstance;
        private const string TableName = "SecondKillCommoditys";

         public SqlSecondKillCommoditysService(DatabaseInstance databaseInstance)
         {
             _databaseInstance = databaseInstance;
         }
        public int Insert(Models.SecondKillCommoditys item)
        {
            return _databaseInstance.Insert(item,TableName);
        }

        public int Update(Models.SecondKillCommoditys item)
        {
            return _databaseInstance.Update(item, TableName);
        }

        public int Delete(Models.SecondKillCommoditys item)
        {
            return _databaseInstance.Delete(item, TableName);
        }

        public Models.SecondKillCommoditys GetById(int id)
        {
            return _databaseInstance.GetById<SecondKillCommoditys>(TableName, id);
        }

        public QueryObject<Models.SecondKillCommoditysss> Query()
        {
            string sql = "select sk.*,c.commodityNo,c.commodityName,c.images,c.commodityPrice from SecondKillCommoditys sk with(nolock) join fz_Commoditys c with(nolock) on sk.commodityId=c.commodityId where c.commodityState=1";
            return new QueryObject<SecondKillCommoditysss>(_databaseInstance, sql, null);
        }

        public Infrastructure.DataTables<Models.SecondKillCommoditysss> Query(Requests.SecondKillCommoditysRequest request)
        {
            SqlParameter[] param = { 
                                     new SqlParameter("@commodityNo",request.commodityNo),
                                     new SqlParameter("@pageIndex",request.PageIndex),
                                     new SqlParameter("@pageSize",request.PageSize),
                                   };
            StoreProcedure sp = new StoreProcedure("P_getSecondKillCommoditys", param);
            return _databaseInstance.GetTables<SecondKillCommoditysss>(sp);
        }


        public SecondKillCommoditys GetBycommodityId(int commodityId)
        {
            string sql = "select * from SecondKillCommoditys where commodityId=@commodityId";
            return new QueryObject<SecondKillCommoditys>(_databaseInstance, sql, new { commodityId = commodityId }).FirstOrDefault();
        }
    }
}
