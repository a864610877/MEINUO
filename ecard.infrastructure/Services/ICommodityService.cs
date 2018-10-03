using Ecard.Infrastructure;
using Ecard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Services
{
    /// <summary>
    /// 商品操作接口
    /// </summary>
    public interface ICommodityService
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
        /// 根据条件获取商品列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        DataTables<Commodity> Query(CommodityRequest request);

        DataTables<Commodity> Query(int IndexSize, int start);

        /// <summary>
        /// 添加库存
        /// </summary>
        /// <param name="commodityId"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        bool AddStorage(int commodityId, int num);
        /// <summary>
        /// 减库存
        /// </summary>
        /// <param name="commodityId"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        bool RejectStorage(int commodityId, int num);

        bool MinussellQuantity(int commodityId, int num);

        /// <summary>
        /// 获取最大的id
        /// </summary>
        /// <param name="commodityId"></param>
        /// <returns></returns>
        int GetMaxId();


        

    }
}
