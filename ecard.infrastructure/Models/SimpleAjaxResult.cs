using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Models
{
    public class SimpleAjaxResult
    {
        public SimpleAjaxResult()
        {
            Success = true;
        }

        public SimpleAjaxResult(string error)
        {
            Message = error;
        }
        public string Message { get; set; }
        public bool Success { get; set; }
    }
    public class DataAjaxResult : SimpleAjaxResult
    {
        public DataAjaxResult()
        {
        }

        public DataAjaxResult(string error)
            : base(error)
        {
        }
        public object Data1 { get; set; }
    }
}
