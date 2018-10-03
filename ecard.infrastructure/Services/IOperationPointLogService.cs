using Ecard.Infrastructure;
using Ecard.Models;
using Ecard.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Services
{
    public class OperationPointLogRequest : PageRequest
    {
        public int? userId { get; set; }

        public DateTime? startTime { get; set; }

        public DateTime? endTime { get; set; }
    }
    public interface IOperationPointLogService
    {
        int Insert(OperationPointLog item);

        int Update(OperationPointLog item);

        int Delete(OperationPointLog item);

        OperationPointLog GetById(int id);
        DataTables<OperationPointLogModel> Query(OperationPointLogRequest request);

    }
}
