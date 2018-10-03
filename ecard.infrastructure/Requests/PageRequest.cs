using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Requests
{
    public class PageRequest
    {
        private int _pageIndex;
        private int _pageSize;

        public int PageIndex
        {
            get
            {
                if (_pageIndex == 0)
                    _pageIndex = 1;
                return _pageIndex;
            }
            set { _pageIndex = value; }
        }
        public int PageSize
        {
            get
            {
                if (_pageSize == 0)
                    _pageSize = 10;
                return _pageSize;
            }
            set { _pageSize = value; }
        }
    }
}
