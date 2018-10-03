using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Mvc
{
  public   class ResultMsg
    {
      public ResultMsg()
        {
            Code = -1;
            CodeText = string.Empty;
            CodeValue = string.Empty;
        }
        public int Code { get; set; }
        public string CodeText { get; set; }
        public string CodeValue { get; set; }
    }
}
