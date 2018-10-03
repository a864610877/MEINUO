using Ecard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Services
{
    public interface ISetWeChatService
    {
        SetWeChat GetById(int id);
        SetWeChat GeByFirst();
        int Update(SetWeChat item);
    }
}
