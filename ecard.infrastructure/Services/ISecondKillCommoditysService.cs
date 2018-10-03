using Ecard.Infrastructure;
using Ecard.Models;
using Ecard.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Services
{
    public interface ISecondKillCommoditysService
    {
        int Insert(SecondKillCommoditys item);

        int Update(SecondKillCommoditys item);

        int Delete(SecondKillCommoditys item);

        SecondKillCommoditys GetById(int id);

        SecondKillCommoditys GetBycommodityId(int commodityId);

        QueryObject<SecondKillCommoditysss> Query();
        DataTables<SecondKillCommoditysss> Query(SecondKillCommoditysRequest request);


    }
}
