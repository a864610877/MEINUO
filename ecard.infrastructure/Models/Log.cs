using System;
using System.ComponentModel.DataAnnotations;

namespace Ecard.Models
{
    public class Log
    {
        public Log()
        {
            SubmitTime = DateTime.Now;
        }

        public Log(string title, string content, int logType, User user)
            : this(title, content, logType)
        {
            if (user != null)
            {
                this.UserId = user.UserId;
                this.UserName = user.Name;
            }
        }
        public Log(string title, string content, int logType)
            : this()
        {
            this.Title = title;
            this.Content = content;
            this.LogType = logType;
        }

        [Key]
        public int LogId { get; set; }
        public DateTime SubmitTime { get; set; }
        public int UserId { get; set; }
        public int? AddIn { get; set; }
        public string UserName { get; set; }
        public string SerialNo { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        [Bounded(typeof(LogTypes))]
        public int LogType { get; set; }
    }
}