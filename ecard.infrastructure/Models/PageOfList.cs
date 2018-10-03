//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Oxite.Model;

namespace Ecard.Models
{
    public class PageOfList<T> : IList<T>, IPageOfList
    {
        private readonly Func<int, int, string, IEnumerable<T>> _func;
        private IList<T> _list = null;
        IList<T> Items
        {
            get
            {
                if (_list == null)
                    _list = _func(PageIndex, (PageSize == 0 ? int.MaxValue : PageSize), OrderBy).ToList();
                return _list;
            }
        }

        public PageOfList(string orderby, int pageSize)
        {
            this._list = new List<T>();
            OrderBy = orderby;
            PageIndex = 1;
            PageSize = pageSize;
            TotalItemCount = 0;
        }

        public PageOfList(IList<T> items, string orderby, int pageIndex, int pageSize, int totalItemCount)
        {
            _list = items;
            OrderBy = orderby;
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalItemCount = totalItemCount;
            OrderBy = orderby;
        }
        public PageOfList(Func<int, int, string, IEnumerable<T>> func, string orderby, int pageIndex, int pageSize, int totalItemCount)
        {
            _func = func;
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalItemCount = totalItemCount;
            OrderBy = orderby;
        }
        public static int GetPageCount(int totalItemCount, int pageSize)
        {
            if (pageSize == 0)
                return 1;
            return (int)Math.Ceiling(totalItemCount / (double)pageSize);
        }
        #region IPageOfList<T> Members

        private int _pageIndex;
        public int PageIndex
        {
            get
            {
                if (this.TotalPageCount < _pageIndex)
                    PageIndex = (TotalPageCount == 0 ? 1 : TotalPageCount);

                return _pageIndex;
            }
            set { _pageIndex = value; }
        }

        public int PageSize { get; set; }
        public string OrderBy { get; set; }
        public int TotalItemCount { get; set; }

        public int TotalPageCount
        {
            get
            {
                return GetPageCount(TotalItemCount, PageSize);

            }
        }

        #endregion

        public IEnumerator<T> GetEnumerator()
        {
            return this.Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            Items.Add(item);
            //throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(T item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { return this.TotalItemCount; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public int IndexOf(T item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public T this[int index]
        {
            get { return Items[index]; }
            set { Items[index] = value; }
        }
    }
}
