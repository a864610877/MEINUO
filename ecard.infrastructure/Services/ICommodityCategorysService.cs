using Ecard.Infrastructure;
using Ecard.Models;
using Ecard.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Services
{
    public interface ICommodityCategorysService
    {
        int Insert(fz_CommodityCategorys item);
        int Update(fz_CommodityCategorys item);

        fz_CommodityCategorys GetById(int id);
        int Delete(fz_CommodityCategorys item);

        /// <summary>
        /// 根据条件获取商品列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        DataTables<fz_CommodityCategorys> Query(CommodityCategorysRequest request);
        List<fz_CommodityCategorys> GetAll();
    }
}
