using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ecard.Models;
using Moonlit;

namespace Ecard.Services
{
    public interface IMenuService
    {
        List<MenuItem> GetMenus(User user);
    }

}