using Ecard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Services
{
    /// <summary>
    /// 城市操作接口
    /// </summary>
    public interface ICityService
    {
        QueryObject<City> Query(int provinceId);
        City GetById(int cityId);

    }
}
