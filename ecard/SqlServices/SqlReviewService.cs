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
    public class SqlReviewService : IReviewService
    {
         private readonly DatabaseInstance _databaseInstance;
         private const string TableName = "fz_Reviews";

         public SqlReviewService(DatabaseInstance databaseInstance)
         {
             _databaseInstance = databaseInstance;
         }
        public int Insert(Models.Review item)
        {
            return _databaseInstance.Insert(item,TableName);
        }

        public int Update(Models.Review item)
        {
            return _databaseInstance.Update(item,TableName);
        }

        public int Delete(Models.Review item)
        {
            return _databaseInstance.Delete(item, TableName);
        }

        public Models.Review GetById(int id)
        {
            return _databaseInstance.GetById<Review>(TableName, id);
        }

        public Infrastructure.DataTables<Models.Reviewss> MicroMallQuery(Requests.ReviewRequest request)
        {
            SqlParameter[] param = { 
                                     new SqlParameter("@CommodityId",request.CommodityId),
                                     new SqlParameter("@State",request.State),
                                     new SqlParameter("@UserId",request.UserId),
                                     new SqlParameter("@pageIndex",request.PageIndex),
                                     new SqlParameter("@pageSize",request.PageSize),
                                   };
            StoreProcedure sp = new StoreProcedure("P_getMicroMallReviews", param);
            return _databaseInstance.GetTables<Reviewss>(sp);
        }


        public DataTables<Reviewss> Query(Requests.ReviewRequest request)
        {
            SqlParameter[] param = { 
                                     //new SqlParameter("@CommodityId",request.CommodityId),
                                     new SqlParameter("@UserName",request.UserName),
                                     new SqlParameter("@CommodityNo",request.CommodityNo),
                                     new SqlParameter("@State",request.State),
                                     //new SqlParameter("@UserId",request.UserId),
                                     new SqlParameter("@pageIndex",request.PageIndex),
                                     new SqlParameter("@pageSize",request.PageSize),
                                   };
            StoreProcedure sp = new StoreProcedure("P_getReviews", param);
            return _databaseInstance.GetTables<Reviewss>(sp);
        }


        public List<Review> GetGoodsReview(int goodsId)
        {

            string sql = "select * from fz_Reviews where [State]=2 and CommodityId=@CommodityId order by SubmitTime desc";
           
            return new QueryObject<Review>(_databaseInstance, sql, new { CommodityId=goodsId }).ToList();
        }

        public List<ReviewExpress> GetGoodsReviewNew(int goodsId)
        {

            string sql = @"SELECT a.*,d.Name userName,d.photo,c.Name cateName FROM fz_Reviews a WITH(NOLOCK) JOIN fz_Commoditys b WITH(NOLOCK)
                            ON a.CommodityId = b.commodityId LEFT JOIN fz_Specifications c WITH(NOLOCK)
                            ON b.specificationId = c.specificationId JOIN dbo.Users d WITH(NOLOCK)
                            ON a.UserId = d.UserId WHERE a.CommodityId = @CommodityId AND a.State=2 ORDER BY a.SubmitTime DESC";

            return new QueryObject<ReviewExpress>(_databaseInstance, sql, new { CommodityId = goodsId }).ToList();
        }
    }
}
