using System;
using System.Collections;
using System.Collections.Generic;
using Ecard.Mvc.ViewModels;

namespace Ecard.Mvc.Models
{
    public class ItemList<T> : List<T>, IItemList
    {
        public ItemList(IEnumerable<T> items)
            : base(items)
        {

        }
        public ItemList()
        {

        }
        public Type GetItemType()
        {
            return typeof(T);
        }

        public IEnumerable Items
        {
            get { return this; }
        }
    }
}