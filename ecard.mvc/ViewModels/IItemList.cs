using System;
using System.Collections;

namespace Ecard.Mvc.ViewModels
{
    public interface IItemType
    {
        Type GetItemType();
    }
    public interface IItemList : IItemType
    {
        IEnumerable Items { get; }
    }
}