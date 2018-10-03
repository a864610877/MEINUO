using System;
using System.Collections.Generic;
using System.Linq;
using Ecard.Models;
using Ecard.Mvc.ViewModels;
using Ecard.Services;
using Microsoft.Practices.Unity;
using Moonlit;

namespace Ecard.Mvc.Models
{
    public class ViewModelBase : IMessageProvider
    {
        private readonly List<MessageEntry> _messages = new List<MessageEntry>();

        [NoRender, Dependency]
        public TransactionHelper TransactionHelper { get; set; }
        [NoRender, Dependency]
        public IUnityContainer UnityContainer { get; set; }
        [NoRender, Dependency]
        public Site HostSite { get; set; }

        [Dependency]
        [NoRender]
        public LogHelper Logger { get; set; }
        [Dependency]
        [NoRender]
        public SecurityHelper SecurityHelper { get; set; }

        [Dependency]
        [NoRender]
        public I18NManager I18NManager { get; set; }

        //[Dependency]
        //[NoRender]
        //public ISystemDealLogService SystemDealLogService { get; set; }

        #region IMessageProvider Members

        public IEnumerable<MessageEntry> GetMessages()
        {
            return _messages.AsEnumerable();
        }

        #endregion

        protected string Localize(string key, string value)
        {
            return I18NManager.Get(GetType().FullName, key, value);
        }

        protected string Localize(string key)
        {
            return Localize(key, key);
        }

        private void AddMessage(string title, MessageType messageType, params string[] messages)
        {
            MessageEntry msg = _messages.FirstOrDefault(x => x.Title == title && x.MessageType == messageType);
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
        protected void AddError(int logType, string message, params  object[] args)
        {
            var text = string.Format(Localize("messages." + message), args);
            AddMessage("´íÎó", MessageType.Error, text);
            if (logType != 0)
                Logger.Error(logType, text);
        }
        protected void AddError(int logType, Exception ex, string message, params  object[] args)
        {
            var text = string.Format(Localize("messages." + message), args);
            AddMessage("´íÎó", MessageType.Error, text);
            if (logType != 0)
                Logger.Error(logType, ex);
        }
        protected void AddMessage(string message, params  object[] args)
        {
            AddMessage("ÏûÏ¢", MessageType.Message, string.Format(Localize("messages." + message), args));
        }

        public IEnumerable<MessageEntry> GetMessages(MessageType? messageType = null)
        {
            if (messageType == null)
                return _messages;
            return _messages.Where(x => x.MessageType == messageType.Value);
        }

     
    }

    public interface IMessageProvider
    {
        IEnumerable<MessageEntry> GetMessages();
    }

}