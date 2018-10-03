using Ecard.Infrastructure;
using Ecard.Models;
using Ecard.Requests;
using Ecard.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Infrastructure
{
    public interface IAdService
    {
        int Insert(Ad item);

        int Update(Ad item);

        int Delete(Ad item);

        Ad GetById(int id);

        QueryObject<Ad> MicroMallQuery();
        DataTables<Ad> Query(AdRequest request);
    }
}
