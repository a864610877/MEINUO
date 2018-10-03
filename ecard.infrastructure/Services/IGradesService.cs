using Ecard.Infrastructure;
using Ecard.Models;
using Ecard.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Services
{
    public interface IGradesService
    {
        int Insert(Grades item);

        int Update(Grades item);

        int Delete(Grades item);

        Grades GetById(int id);

        QueryObject<Grades> QueryAll();
        DataTables<Grades> Query(GradesRequest request);
    }
}
