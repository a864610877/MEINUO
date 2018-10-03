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
    public class SqlJuMeiMallService : IJuMeiMallService
    {
        private readonly DatabaseInstance _databaseInstance;
        private const string TableName = "fz_Commoditys";
        private const string TableName2 = "fz_ShoppingCarts";
        public SqlJuMeiMallService(DatabaseInstance databaseInstance)
        {
            _databaseInstance = databaseInstance;
        }
        public int Insert(Models.Commodity item)
        {
            return _databaseInstance.Insert(item, TableName);
        }

        public int Update(Models.Commodity item)
        {
            return _databaseInstance.Update(item, TableName);
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

        public DataTables<Commodity> Query(JuMeiMallIndexRequest request)
        {
            SqlParameter[] param = { 
                                     new SqlParameter("@commodityName",null),
                                     new SqlParameter("@pageIndex",request.PageIndex),
                                     new SqlParameter("@pageSize",request.PageSize),
                                     new SqlParameter("@commodityState", CommodityStates.putaway),
                                   };
            StoreProcedure sp = new StoreProcedure("P_getAllMallGoods", param);
            return _databaseInstance.GetTables<Commodity>(sp);
        }

        public List<Commodity> QueryByCategoryId(int commodityCategoryId)
        {
            string sql = "select top 4 * from fz_Commoditys where commodityCategoryId=@commodityCategoryId and commodityState=@commodityState order by commodityRank";
            return new QueryObject<Commodity>(_databaseInstance, sql, new { commodityCategoryId = commodityCategoryId, commodityState = CommodityStates.putaway }).ToList();
        }


        public List<fz_CommodityCategorys> FASQuery()
        {
            string sql = "select * from fz_CommodityCategorys where 1=1";
            return new QueryObject<fz_CommodityCategorys>(_databaseInstance, sql, null).ToList();

        }



        public DataTables<Commodity> QueryList(JuMeiMallSearchRequest request)
        {
            SqlParameter[] param = { 
                                     new SqlParameter("@commodityName",request.commodityName),
                                     new SqlParameter("@commodityCategoryId",request.commodityCategoryId),
                                     new SqlParameter("@pageIndex",request.PageIndex),
                                     new SqlParameter("@pageSize",request.PageSize),
                                     new SqlParameter("@commodityState", CommodityStates.putaway),
                                   };
            StoreProcedure sp = new StoreProcedure("P_getAllMallGoods", param);
            return _databaseInstance.GetTables<Commodity>(sp);
        }

        public GoodsDetailsExpress GetDetailsExById(int commodityId)
        {
            var GoodsDetailsResult = new GoodsDetailsExpress();
            string sql = "select * from fz_Commoditys where commodityId=@commodityId";
            var CommodityResult = new QueryObject<Commodity>(_databaseInstance, sql, new { commodityId = commodityId }).FirstOrDefault();
            //string sql = "select * from fz_Commoditys where commodityId=@commodityId";
            if (CommodityResult != null)
            {
                GoodsDetailsResult.commodityDetails = CommodityResult.commodityDetails;
                GoodsDetailsResult.commodityId = CommodityResult.commodityId;
                GoodsDetailsResult.commodityFreight = CommodityResult.commodityFreight;
                GoodsDetailsResult.commodityInventory = CommodityResult.commodityInventory;
                GoodsDetailsResult.commodityName = CommodityResult.commodityName;
                GoodsDetailsResult.commodityPrice = CommodityResult.commodityPrice;
                GoodsDetailsResult.commodityPrice1 = CommodityResult.commodityPrice1;
                GoodsDetailsResult.IsPinkage = CommodityResult.IsPinkage;
                GoodsDetailsResult.images = CommodityResult.images;
                GoodsDetailsResult.commodityJifen = CommodityResult.commodityJifen;
                GoodsDetailsResult.specificationId = CommodityResult.specificationId;
                GoodsDetailsResult.sellQuantity = CommodityResult.sellQuantity;
                if (CommodityResult.IsPinkage == false)
                {
                    GoodsDetailsResult.totalPrice = CommodityResult.commodityPrice + CommodityResult.commodityFreight;
                }
                else
                {
                    GoodsDetailsResult.totalPrice = CommodityResult.commodityPrice;
                }

            }


            return GoodsDetailsResult;

        }

        public GoodsDetails GetDetailsById(int commodityId)
        {
            var GoodsDetailsResult = new GoodsDetails();
            string sql = "select * from fz_Commoditys where commodityId=@commodityId";
            var CommodityResult = new QueryObject<Commodity>(_databaseInstance, sql, new { commodityId = commodityId }).FirstOrDefault();
            //string sql = "select * from fz_Commoditys where commodityId=@commodityId";
            if (CommodityResult != null)
            {
                GoodsDetailsResult.commodityDetails = CommodityResult.commodityDetails;
                GoodsDetailsResult.commodityId = CommodityResult.commodityId;
                GoodsDetailsResult.commodityFreight = CommodityResult.commodityFreight;
                GoodsDetailsResult.commodityInventory = CommodityResult.commodityInventory;
                GoodsDetailsResult.commodityName = CommodityResult.commodityName;
                GoodsDetailsResult.commodityPrice = CommodityResult.commodityPrice;
                GoodsDetailsResult.commodityPrice1 = CommodityResult.commodityPrice1;
                GoodsDetailsResult.IsPinkage = CommodityResult.IsPinkage;
                GoodsDetailsResult.images = CommodityResult.images;
                if (CommodityResult.IsPinkage == false)
                {
                    GoodsDetailsResult.totalPrice = CommodityResult.commodityPrice + CommodityResult.commodityFreight;
                }
                else
                {
                    GoodsDetailsResult.totalPrice = CommodityResult.commodityPrice;
                }

            }


            return GoodsDetailsResult;

        }


        public Account GetUserInfoByOpenId(string OpenId)
        {
            string sql = "select * from fz_Accounts where OpenID=@OpenId";
            return new QueryObject<Account>(_databaseInstance, sql, new { OpenId = OpenId }).FirstOrDefault();

        }


        public ShoppingCart GetByAccountIdAndCommodityId(int accountId, int commodityId, string specification)
        {
            string sql = "select * from fz_ShoppingCarts where userId=@accountId and commodityId=@commodityId and specification = @specification";
            return new QueryObject<ShoppingCart>(_databaseInstance, sql, new { accountId = accountId, commodityId = commodityId, specification = specification }).FirstOrDefault();
        }


        public int InsertCart(ShoppingCart item)
        {
            return _databaseInstance.Insert(item, TableName2);
        }

        public int UpdateCart(ShoppingCart item)
        {
            return _databaseInstance.Update(item, TableName2);
        }

        public int DeleteCart(ShoppingCart item)
        {
            return _databaseInstance.Delete(item, TableName2);
        }


        public List<Commodity> QueryHotSale()
        {
            string sql = "select * from fz_Commoditys where  commodityState=@commodityState order by sellQuantity desc,commodityRank asc";
            return new QueryObject<Commodity>(_databaseInstance, sql, new { commodityState= CommodityStates.putaway}).ToList();
        }
    }
}
