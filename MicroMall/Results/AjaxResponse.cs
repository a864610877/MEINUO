﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oxite.Mvc.Results
{
    public class AjaxResponse
    {
        public AjaxResponse()
        {
            Success = false;
        }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
