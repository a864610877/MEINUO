using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Services
{
    public interface ILog4netService
    {
        void Insert(Exception ex);
        void Insert(string msg);
    }
}
