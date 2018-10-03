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
    public class SqlCommodityService : ICommodityService
    {

         private readonly DatabaseInstance _databaseInstance;
         private const string TableName = "fz_Commoditys";

         public SqlCommodityService(DatabaseInstance databaseInstance)
         {
             _databaseInstance = databaseInstance;
         }
        public int Insert(Models.Commodity item)
        {
            return _databaseInstance.Insert(item,TableName);
        }

        public int Update(Models.Commodity item)
        {
            return _databaseInstance.Update(item,TableName);
        }

        public int Delete(Models.Commodity item)
        {
            return _databaseInstance.Delete(item, TableName);
        }

        public Commodity GetById(int commodityId)
        {
            return _databaseInstance.GetById<Commodity>(TableName, commodityId);
        }

        public Commodity GetBycommodityNo(string commodityNo)
        {
            string sql = "select * from fz_Commoditys where commodityNo=@commodityNo";
            return new QueryObject<Commodity>(_databaseInstance, sql, new { commodityNo = commodityNo }).FirstOrDefault();
        }

        public DataTables<Commodity> Query(CommodityRequest request)
        {
            if (request.commodityState==0)
            {
                request.commodityState = null;
            }
            SqlParameter[] param = { 
                                     new SqlParameter("@commodityName",request.commodityName),
                                     new SqlParameter("@commdityKeyword",request.commdityKeyword),
                                     new SqlParameter("@commodityNo",request.commodityNo),
                                     new SqlParameter("@commodityState",request.commodityState),
                                     new SqlParameter("@startSubmitTime",request.startSubmitTime),
                                     new SqlParameter("@endSubmitTime",request.endSubmitTime),
                                     new SqlParameter("@commodityCategoryId",request.commodityCategoryId),
                                     new SqlParameter("@pageIndex",request.PageIndex),
                                     new SqlParameter("@pageSize",request.PageSize),
                                   };
            StoreProcedure sp = new StoreProcedure("P_getCommodity", param);
            return _databaseInstance.GetTables<Commodity>(sp);
        }


        public DataTables<Commodity> Query(int start,int end)
        {
            SqlParameter[] param = { 
                                     new SqlParameter("@end",end),
                                     new SqlParameter("@start",start),
                                   };
            StoreProcedure sp = new StoreProcedure("P_getIndexCommodity", param);
            return _databaseInstance.GetTables<Commodity>(sp);
        }


        public bool AddStorage(int commodityId, int num)
        {
            if (num <= 0)
                return false;
            var item = _databaseInstance.GetById<Commodity>(TableName, commodityId);
            if (item != null)
            {
                item.commodityInventory += num;
                return _databaseInstance.Update(item, TableName) > 0;
            }
            return false;
        }

        public bool MinussellQuantity(int commodityId, int num)
        {
            if (num <= 0)
                return false;
            var item = _databaseInstance.GetById<Commodity>(TableName, commodityId);
            if (item != null)
            {
                item.sellQuantity -= num;
                item.sellQuantity1 -= num;
                return _databaseInstance.Update(item, TableName) > 0;
            }
            return false;
        }

        public bool RejectStorage(int commodityId, int num)
        {
            if (num <= 0)
                return false;
            var item = _databaseInstance.GetById<Commodity>(TableName, commodityId);
            if (item != null)
            {
                if (num > item.commodityInventory)
                    return false;
                item.commodityInventory -= num;
                return _databaseInstance.Update(item, TableName) > 0;
            }
            return false;
        }


        public int GetMaxId()
        {
            string sql = "select max(commodityId) as commodityId  from fz_Commoditys";
            var query= new QueryObject<Commodity>(_databaseInstance, sql, null).FirstOrDefault();
            if (query != null)
            {
                return query.commodityId;
            }
            else
            {
                return 1;
            }
        }
    }
}
