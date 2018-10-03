using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ecard.Models;
using Ecard.Mvc.ViewModels;

namespace Ecard.Mvc.Models
{
    public class MultiCheckList<T> : List<CheckedItem<T>>, IItemList
        where T : IKeyObject
    {
        private readonly IList<T> _items = new List<T>();

        public MultiCheckList(IEnumerable<T> items)
        {
            _items = items.ToList();
        }

        public MultiCheckList()
        {

        }

        public void Merge()
        {

            this.Clear();
            foreach (var item in _items)
            {
                this.Add(new CheckedItem<T>(item.Id, item, true));
            }
        }
        public void Merge(IEnumerable<T> items)
        {
            this.Clear();
            var list = items.ToList();
            foreach (var item in list)
            {
                bool check = _items.Any(x => x.Id == item.Id);

                this.Add(new CheckedItem<T>(item.Id, item, check));
            }
        }

        public int[] GetCheckedIds()
        {
            return this.Where(x => x.Checked).Select(x => x.Id).ToArray();
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

    ////-------
    //public class CommodityList<T> : List<CheckedItem<T>>, IItemList
    //    where ListCommodity : IKeyObject
    //{
    //    private readonly IList<T> _items = new List<T>();

    //    public CommodityList(IEnumerable<T> items)
    //    {
    //        _items = items.ToList();
    //    }

    //    public CommodityList()
    //    {

    //    }

    //    public void Merge()
    //    {

    //        this.Clear();
    //        foreach (var item in _items)
    //        {
    //            this.Add(new CheckedItem<T>(item.Id, item, true));
    //        }
    //    }
    //    public void Merge(IEnumerable<T> items)
    //    {
    //        this.Clear();
    //        var list = items.ToList();
    //        foreach (var item in list)
    //        {
    //            bool check = _items.Any(x => x.Id == item.Id);

    //            this.Add(new CheckedItem<T>(item.Id, item, check));
    //        }
    //    }

    //    public int[] GetCheckedIds()
    //    {
    //        return this.Where(x => x.Checked).Select(x => x.Id).ToArray();
    //    }

    //    public Type GetItemType()
    //    {
    //        return typeof(ListCommodity);
    //    }

    //    public IEnumerable<T> Items
    //    {
    //        get { return this; }
    //    }
    //}
    ////-------
}