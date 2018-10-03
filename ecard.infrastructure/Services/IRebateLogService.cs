using Ecard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Services
{
    public interface IRebateLogService
    {
        List<RebateLog> GetRebateLog(int accountId);
    }

    
}
