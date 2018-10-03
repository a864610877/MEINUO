using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using Ecard.Models;

namespace Ecard.Services
{
    public class EmailService
    {
        private readonly Site _site;

        public EmailService(Site site)
        {
            _site = site;
        }

        public void SendMail(string template, string to, string title, Dictionary<string, string> context)
        {
            var adminEmail = ConfigurationManager.AppSettings["adminEmail"];
            var file = HttpContext.Current.Server.MapPath("~/template/mails/" + template);
            var content = File.ReadAllText(file);
            foreach (var kv in context)
            {
                content = content.Replace("#" + kv.Key + "#", kv.Value);
            } 
            using (SmtpClient client = new SmtpClient())
            {
                MailMessage msg = new MailMessage(adminEmail, to);
                msg.Subject = title;
                msg.Body = content;
                msg.IsBodyHtml = true;
                client.Send(msg);
            }
        }
    }
}
