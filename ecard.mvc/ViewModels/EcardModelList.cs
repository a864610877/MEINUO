//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Ecard.Models;
using Ecard.Mvc.Models;

namespace Ecard.Mvc.ViewModels
{
    /// <summary>
    /// 提供集合的 Model
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EcardModelList<T> : EcardModel, IItemList
    {
        public int GetIndex(T obj)
        {
            for (int i = 0; i < List.Count; i++)
            {
                if (object.ReferenceEquals(obj, List[i]))
                    return i;
            }
            return 0;
        }

        [NoRender]
        public IList<T> List { get; set; }

        public CheckedItem GetCheckItem(object target)
        {
            return CheckItems.FirstOrDefault(x => x.Item == target);
        }

        public Type GetItemType()
        {
            return typeof(T);
        }
        [ClearModelState]
        [NoRender]
        public List<CheckedItem> CheckItems { get; set; }
        IEnumerable IItemList.Items
        {
            get { return List; }
        }
    }
}
