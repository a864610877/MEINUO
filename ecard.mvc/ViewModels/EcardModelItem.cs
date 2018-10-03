//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System;
using Ecard.Mvc.Models;

namespace Ecard.Mvc.ViewModels
{
    /// <summary>
    /// Model, 即包含一个 Item 的 Model
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EcardModelItem<T> : EcardModel, IItemType
        where T : class
    {
        public EcardModelItem(T item, IMessageProvider message = null)
        {
            Item = item;
            AddMessages(message);
            if (message == null)
            {
                AddMessages(item as IMessageProvider);
            }
        }

        private void AddMessages(IMessageProvider message)
        {
            if (message != null)
            {
                foreach (var messageEntry in message.GetMessages())
                {
                    this.AddMessage(messageEntry);
                }
            }
        }

        public EcardModelItem()
        {

        }
        public override Type GetModelType()
        {
            return typeof(T);
        }
        public T Item { get; set; }

        public Type GetItemType()
        {
            return typeof(T);
        }


    }
}
