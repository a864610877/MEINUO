using Ecard.Infrastructure;
using Ecard.Models;
using Ecard.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Services
{
    public interface IReviewService
    {
        int Insert(Review item);

        int Update(Review item);

        int Delete(Review item);

        Review GetById(int id);

        DataTables<Reviewss> MicroMallQuery(ReviewRequest request);


        DataTables<Reviewss> Query(ReviewRequest request);
        /// <summary>
        /// 获取商品评论
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        List<Review> GetGoodsReview(int goodsId);

        /// <summary>
        /// 获取商品评论 (扩展)
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        List<ReviewExpress> GetGoodsReviewNew(int goodsId);

    }
}
