using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oxite
{
    public class GlobalEnv
    {
        public static string GetImageUrls(string picture)
        {
            return "/images/" + picture;
        }
    }
}
