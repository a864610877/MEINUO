//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Ecard.Models;
using Ecard.Services;

namespace Ecard
{
    public static class PageOfListExtensions
    {
        public static PageOfList<TResult> ToList<T, TResult>(this QueryObject<T> query, int pageIndex, int pageSize, string orderBy, Func<T, TResult> selector) where T : new()
        {
            var totalItemCount = (int)query.Count();
            IEnumerable<T> list = query.ToList((pageIndex - 1) * pageSize, pageSize, orderBy);
            return ToList(pageIndex, pageSize, orderBy, selector, list, totalItemCount);
        }

        private static PageOfList<TResult> ToList<T, TResult>(int pageIndex, int pageSize, string orderBy, Func<T, TResult> selector,
                                                     IEnumerable<T> list, int totalItemCount)
            where T : new()
        {
            var pageCount = PageOfList<TResult>.GetPageCount(totalItemCount, pageSize);
            if (pageCount < pageIndex)
                pageIndex = pageCount == 0 ? 1 : pageCount;
            var items = list.Select(selector).ToList();

            return new PageOfList<TResult>(items, orderBy, pageIndex, pageSize, totalItemCount);
        }

        public static PageOfList<TResult> ToList<T, TResult>(this QueryObject<T> query, IQueryRequest queryRequest, Func<T, TResult> selector) where T : new()
        {
            return query.ToList(queryRequest.PageIndex, queryRequest.PageSize, queryRequest.OrderBy, selector);
        }
        public static PageOfList<TResult> ToList<T, TResult>(this IEnumerable<T> query, IQueryRequest queryRequest, Func<T, TResult> selector) where T : new()
        {
            int recordCount = query.Count();
            query = query.Skip(queryRequest.PageSize*(queryRequest.PageIndex - 1)).Take(queryRequest.PageSize);
            return ToList(queryRequest.PageIndex, queryRequest.PageSize, queryRequest.OrderBy, selector, query, recordCount);
        }
    }
}