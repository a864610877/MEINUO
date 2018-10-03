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
    /// 商品规格接口
    /// </summary>
    public interface ISpecificationService
    {
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        int Insert(Specification item);
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        int Update(Specification item);

        /// <summary>
        /// 获取插入数据的自增ID
        /// </summary>
        /// <returns></returns>
        object QueryIdentity();
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        int Delete(Specification item);
        /// <summary>
        /// 根据id获取实体
        /// </summary>
        /// <param name="specificationId"></param>
        /// <returns></returns>
        Specification GetById(int specificationId);
        /// <summary>
        /// 根据id 获取实体极其明细 
        /// </summary>
        /// <param name="specificationId"></param>
        /// <returns></returns>
        SpecificationAndSpecificationDetail GetSpecificationAndSpecificationDetailById(int specificationId);
        /// <summary>
        /// 根据条件获取规格列表
        /// </summary>
        /// <returns></returns>
        DataTables<Specification> Query(SpecificationRequest request);

        /// <summary>
        /// 查询所有规格
        /// </summary>
        /// <returns></returns>
        QueryObject<Specification> QueryAll();
    }
    
}
