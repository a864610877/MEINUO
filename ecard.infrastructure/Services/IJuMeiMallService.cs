using Ecard.Infrastructure;
using Ecard.Models;
using Ecard.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Services
{
    /// <summary>
    /// 欣美聚诚商品信息借口
    /// </summary>
    public interface IJuMeiMallService
    {
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        int Insert(Commodity item);
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        int Update(Commodity item);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        int Delete(Commodity item);
        /// <summary>
        /// 根据id，查询实体
        /// </summary>
        /// <param name="commodityId"></param>
        /// <returns></returns>
        Commodity GetById(int commodityId);

        /// <summary>
        /// 根据商品编码查询
        /// </summary>
        /// <param name="commodityNo"></param>
        /// <returns></returns>
        Commodity GetBycommodityNo(string commodityNo);
        /// <summary>
        /// 获取商品所有列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        DataTables<Commodity> Query(JuMeiMallIndexRequest request);

        /// <summary>
        /// 获取商品byCategoryId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<Commodity> QueryByCategoryId(int id);
        /// <summary>
        /// FAS搜索
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        List<fz_CommodityCategorys> FASQuery();
        
        /// <summary>
        /// 根据名称获取商品列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        DataTables<Commodity> QueryList(JuMeiMallSearchRequest request);
        //DataTables<Commodity> Query(int IndexSize, int start);


        GoodsDetails GetDetailsById(int commodityId);

        GoodsDetailsExpress GetDetailsExById(int commodityId);

        Account GetUserInfoByOpenId(string OpenId);


        ShoppingCart GetByAccountIdAndCommodityId(int accountId, int commodityId, string specification);


        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        int InsertCart(ShoppingCart item);
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        int UpdateCart(ShoppingCart item);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        int DeleteCart(ShoppingCart item);

        /// <summary>
        /// 获取销量前50的商品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<Commodity> QueryHotSale();

    }


   

    /// <summary>
    /// 商品详情的扩展
    /// </summary>
    public class GoodsDetailsExpress: GoodsDetails
    {
        public decimal commodityJifen{ get; set; }
        public List<ReviewExpress> listReviewNew { get; set; }
    }

    

    /// <summary>
    /// 商品详情
    /// </summary>
    public class GoodsDetails
    {
        public int commodityId { get; set; }

        /// <summary>
        /// 取第二张开始为details显示
        /// </summary>
        public string images { get; set; }

        /// <summary>
        /// 销售价格
        /// </summary>
        public decimal commodityPrice { get; set; }

        /// <summary>
        /// 原价
        /// </summary>
        public decimal commodityPrice1 { get; set; }
        public string commodityName { get; set; }

        public string commodityRemark { get; set; }

        /// <summary>
        /// 是否包邮
        /// </summary>
        public bool IsPinkage { get; set; }

        /// <summary>
        /// 运费
        /// </summary>
        public decimal commodityFreight { get; set; }

        /// <summary>
        /// 总价格=销售价格+运费
        /// </summary>
        public decimal totalPrice { get; set; }

        /// <summary>
        /// 商品详情
        /// </summary>
        public string commodityDetails { get; set; }

        /// <summary>
        /// 库存
        /// </summary>
        public int commodityInventory { get; set; }
        /// <summary>
        /// 销售数量
        /// </summary>
        public int sellQuantity { get; set; }
        /// <summary>
        /// 属性id
        /// </summary>
        public string specificationId { get; set; }
        /// <summary>
        /// 宝贝评价总条数
        /// </summary>
        public int reviewCount { get; set; }
        /// <summary>
        /// 评价列表
        /// </summary>
        public List<Review> listReview { get; set; }
        /// <summary>
        /// 轮播图片
        /// </summary>
        public List<string> listImg { get; set; }

        /// <summary>
        /// 商品属性
        /// </summary>
        public List<SpecificationAndSpecificationDetail> listAattribute { get; set; }


    }

    public class SelectModel1
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
