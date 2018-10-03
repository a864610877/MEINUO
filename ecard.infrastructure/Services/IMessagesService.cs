using Ecard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Services
{
    public interface IMessagesService
    {
        int Insert(Fz_Messages item);
        int Update(Fz_Messages item);

        QueryObject<Fz_Messages> GetSatySend();
    }
}
