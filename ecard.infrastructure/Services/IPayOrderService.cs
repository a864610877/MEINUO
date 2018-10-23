using Ecard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Services
{
    public interface IPayOrderService
    {
        int Insert(PayOrder item);
        int Update(PayOrder item);
    }
}
