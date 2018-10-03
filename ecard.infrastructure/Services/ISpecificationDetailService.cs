using Ecard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Services
{
    /// <summary>
    /// 规格详情接口
    /// </summary>
    public interface ISpecificationDetailService
    {
        /// <summary>
        ///  插入
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        int Insert(SpecificationDetail item);
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        int Update(SpecificationDetail item);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        int Delete(SpecificationDetail item);
        /// <summary>
        /// 根据id搜索实现
        /// </summary>
        /// <param name="specificationDetailId"></param>
        /// <returns></returns>
        SpecificationDetail GetById(int specificationDetailId);
        /// <summary>
        ///  根据规格specificationId,获取规格明细
        /// </summary>
        /// <param name="specificationId"></param>
        /// <returns></returns>
        QueryObject<SpecificationDetail> GetByspecificationId(int specificationId);
    }

}
