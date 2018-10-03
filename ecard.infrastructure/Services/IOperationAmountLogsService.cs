using Ecard.Infrastructure;
using Ecard.Models;
using Ecard.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Services
{
    public interface IOperationAmountLogsService
    {
        int Insert(fz_OperationAmountLogs item);
        int Update(fz_OperationAmountLogs item);
        DataTables<fz_OperationAmountLogs> GetByUserId(OperationAmountLogUserIdRequest request);

        fz_OperationAmountLogs GetByUserId(int userId);
    }

    public class OperationAmountLogUserIdRequest:PageRequest
    {
        public int userId { get; set; }

        public DateTime? startTime { get; set; }

        public DateTime? endTime { get; set; }
    }
}
