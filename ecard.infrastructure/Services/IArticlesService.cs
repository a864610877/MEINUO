using Ecard.Infrastructure;
using Ecard.Models;
using Ecard.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Services
{
    public interface IArticlesService
    {
        int Insert(Articles item);

        int Update(Articles item);

        int Delete(Articles item);

        Articles GetById(int id);

        QueryObject<Articles> QueryAll();
        DataTables<Articles> Query(ArticlesRequest request);

    }
}
