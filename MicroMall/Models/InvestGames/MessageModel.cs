using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroMall.Models.InvestGames
{
    public class MessageModel
    {
        public string access_token { get; set; }

        public int expires_in { get; set; }

        public int errcode { get; set; }

        public string errmsg { get; set; }
    }
}