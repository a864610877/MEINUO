using System;
using System.Text.RegularExpressions;
using System.Threading; 
using log4net;

namespace Ecard
{
    public class SmsHelper
    {
        private readonly Ecard.Models.ISmsService _smsService;

        public SmsHelper(Ecard.Models.ISmsService smsService)
        {
            _smsService = smsService;
        }

        public void Send(string mobile, string msg, DateTime? reservateTime = null, DateTime? expiredTime = null)
        {
            if (string.IsNullOrWhiteSpace(mobile) || string.IsNullOrWhiteSpace(msg))
                return;
            mobile = mobile.Trim();

            if (!Regex.IsMatch(mobile, @"\d{11}")) return;

            if (expiredTime == null)
            {
                if (reservateTime != null)
                    expiredTime = reservateTime.Value.AddMinutes(5);
                else
                    expiredTime = DateTime.Now.AddMinutes(5);
            }
            var item = new Ecard.Models.Sms()
            {
                Message = msg,
                Mobile = mobile,
                ReservateTime = reservateTime,
                ExpiredTime = expiredTime.Value,
            };
            _smsService.Create(item);
        }
    }
}