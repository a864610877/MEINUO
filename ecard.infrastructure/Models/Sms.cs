using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Ecard.Services;

namespace Ecard.Models
{

    public class Sms
    {
        public Sms()
        {
            State = SmsStates.Normal;
            SubmitTime = DateTime.Now;
            RetryCount = 3;
        }

        public DateTime SubmitTime { get; set; }

        [Key]
        public int SmsId { get; set; }
        public string Message { get; set; }
        public string Mobile { get; set; }
        public int RetryCount { get; set; }
        [Bounded(typeof(SmsStates))]
        public int State { get; set; }

        public DateTime? ReservateTime { get; set; }
        public DateTime ExpiredTime { get; set; }
    }
    public class SmsStates : States
    {
        public const int Complated = 10;
    }

    public interface ISmsService
    {
        void Create(Sms item);
        Sms GetById(int id);
        void Update(Sms item);
        void Delete(Sms item);
        QueryObject<Sms> Query(int[] states);
    }

}
