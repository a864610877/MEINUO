using System.Collections.Generic;
using System.Linq;
using Ecard.Mvc.Models;
using Ecard.Mvc.ViewModels;

namespace Ecard.Mvc
{
    public class SimpleMessageProvider : IMessageProvider
    {
        public MessageType MessageType { get; set; }
        private readonly string[] _warnings;

        public SimpleMessageProvider(MessageType messageType, string[] warnings)
        {
            MessageType = messageType;
            _warnings = warnings;
        }

        public IEnumerable<MessageEntry> GetMessages()
        {
            return _warnings.Select(x => new MessageEntry("¾¯¸æ", MessageType, x));
        }
    }
}