//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
namespace Ecard.Models
{
    public interface IPageOfList
    {
        int PageIndex { get; set; }
        int PageSize { get; set; }
        int TotalPageCount { get; }
        int TotalItemCount { get; }
        string OrderBy { get;  }
    }
}
