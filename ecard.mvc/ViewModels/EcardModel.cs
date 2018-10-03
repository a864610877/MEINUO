//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;

using Ecard.Models;
using Ecard.Models.PageContainers;
using Ecard.Mvc.Models;
using Microsoft.Practices.Unity;
using Moonlit;
using Oxite.Model;
using Oxite.Mvc.Infrastructure;
using Oxite.Mvc.ViewModels;

namespace Ecard.Mvc.ViewModels
{
    /// <summary>
    /// 通用 Model
    /// </summary>
    public class EcardModel
    {
        /// <summary>
        /// 防篡改标记 ?
        /// </summary>
        // TODO: 防篡改标记 ?
        [NoRender]
        public AntiForgeryToken AntiForgeryToken { get; set; }
        /// <summary>
        /// 事务
        /// </summary>
        [NoRender, Dependency]
        public TransactionHelper TransactionHelper { get; set; }
        [NoRender, Dependency]
        public LogHelper Logger { get; set; }
        /// <summary>
        /// 容器
        /// </summary>
        [NoRender]
        public PageContainer Container { get; set; }
        /// <summary>
        /// 站点相关信息
        /// </summary>
        [NoRender]
        public SiteViewModel Site { get; set; }
        /// <summary>
        /// 用户信息
        /// </summary>
        [NoRender]
        public User User { get; set; }
        /// <summary>
        /// 用户信息
        /// </summary>
        [NoRender]
        public UserModel UserModel { get; set; }
        /// <summary>
        /// 皮肤信息
        /// </summary>
        private string skinName;
        [NoRender]
        public string SkinName
        {
            get
            {
                if (string.IsNullOrEmpty(skinName))
                    return "";

                return skinName;
            }

            set
            {
                skinName = value; 
            }
        }
        private readonly List<MessageEntry> _messages = new List<MessageEntry>();
        protected void AddMessage(string message, params  object[] args)
        {
            AddMessage("消息", MessageType.Message, string.Format(Localize(message), args));
        }
        protected void AddError(string message, params  object[] args)
        {
            AddMessage("错误", MessageType.Error, string.Format(Localize(message), args));
        }
        public void AddMessage(string title, MessageType messageType, params  string[] messages)
        {
            var msg = _messages.FirstOrDefault(x => x.Title == title && x.MessageType == messageType);
            if (msg == null)
            {
                msg = new MessageEntry(title, messageType, messages);
                _messages.Add(msg);
            }
            else
            {
                msg.Messages.AddRange(messages);
            }
        }
        public void AddMessage(MessageEntry message)
        {
            _messages.Add(message);
        }
        public IEnumerable<MessageEntry> GetMessages(MessageType? messageType = null)
        {
            if (messageType == null)
                return _messages;
            return _messages.Where(x => x.MessageType == messageType.Value);
        }
        [NoRender]
        public bool IsAuthenticated
        {
            get { return User != null && User.Name != "Anonymous"; }
        }

        /// <summary>
        /// 保存 type, type instance ?
        /// </summary>
        private readonly Dictionary<Type, object> modelItems = new Dictionary<Type, object>();

        public void AddModelItem<T>(T modelItem) where T : class
        {
            modelItems[typeof(T)] = modelItem;
        }
        /// <summary>
        /// 同容器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetModelItem<T>() where T : class
        {
            if (modelItems.ContainsKey(typeof(T)))
                return modelItems[typeof(T)] as T;

            return null;
        }

        //TODO: (erikpo) Fix Localize methods. These are temporary stubs
        /// <summary>
        /// 提供多语言支持
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Localize(string key)
        {
            return Localize(key, key);
        }
        /// <summary>
        /// 提供多语言支持
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public string Localize(string key, string defaultValue)
        {
            //ICollection<Phrase> phrases = GetModelItem<ICollection<Phrase>>();

            //if (phrases != null)
            //    return phrases.Where(p => p.Key == key && p.Language == Site.Language).Select(p => p.Value).FirstOrDefault() ?? defaultValue;
            var type = this.GetModelType();

            return I18NManager.Get(type.FullName, key, defaultValue);
        }
        public virtual Type GetModelType()
        {
            return this.GetType();
        }
        [NoRender]
        [Dependency]
        public I18NManager I18NManager { get; set; }

        [NoRender]
        public List<MenuItem> Menus
        {
            get { return GetModelItem<List<MenuItem>>(); }
            set { AddModelItem<List<MenuItem>>(value); }
        }

        public bool CheckPermission(string permission)
        {
            return User.Roles.Any(x => x.HasPermissions(permission));
        }

        public IEnumerable<MessageEntry> GetMessages()
        {
            return _messages;
        }
    }

    public abstract class UserModel
    {
        public abstract User CurrentUser { get; }
        public static implicit operator User(UserModel userModel)
        {
            return userModel == null ? null : userModel.CurrentUser;
        }
    }

    public class MessageEntry
    {
        public MessageType MessageType { get; set; }
        public List<string> Messages { get; private set; }
        public string Title { get; set; }

        public MessageEntry(string title, MessageType messageType, params string[] messages)
        {
            Title = title;
            MessageType = messageType;
            Messages = new List<string>(messages);
        }
    }
    public enum MessageType
    {
        Message,
        Error
    }
}
