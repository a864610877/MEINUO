using System.Collections.Generic;
using System.Linq;

using Ecard.Models;
using Oxite.Model;

namespace Ecard.Mvc.Models
{
    public static class CheckedItemExtensions
    {
        public static int[] GetCheckedIds(this IEnumerable<CheckedItem> items)
        {
            if (items == null) return new int[0];
            return items.Where(x => x.Checked).Select(x => x.Id).ToArray();
        }
    }
    public class CheckedItem
    {
        public bool Checked { get; set; }
        public int Id { get; set; }
        public object Item { get; set; }
    }
    public class CheckedItem<T> : CheckedItem
    {
        public CheckedItem()
        {

        }
        public CheckedItem(int id, T item, bool check = true)
        {
            Id = id;
            Item = item;
            Checked = check;
        }

        public new T Item { get { return (T)base.Item; } set { base.Item = value; } }
    }
}
