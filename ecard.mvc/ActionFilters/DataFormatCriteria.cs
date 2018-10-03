//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System;
using System.Web.Mvc;

namespace Ecard.Mvc.ActionFilters
{
    /// <summary>
    /// 通过比较 route 中 dataFormat 来评判
    /// </summary>
    public class DataFormatCriteria : IActionFilterCriteria
    {
        private readonly string _format;
        public DataFormatCriteria(string format)
        {
            this._format = format;
        }

        public bool Match(ActionFilterRegistryContext context)
        {
            return string.Equals(_format, context.ControllerContext.RouteData.Values["dataFormat"] as string, StringComparison.OrdinalIgnoreCase);
        }
    }
}
