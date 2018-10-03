using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Moonlit;

namespace Ecard.Mvc
{
    public class RandomCodeHelper
    {
        protected object GetValue(string key)
        {
            return HttpContext.Current.Session["RandomCodeHelper_Code_" + key];
        }
        protected void SetValue(string key, object obj)
        {
            HttpContext.Current.Session["RandomCodeHelper_Code_" + key] = obj;
        }

        protected DateTime? GetTimeOut(string key)
        {
            return (DateTime?)HttpContext.Current.Session["RandomCodeHelper_TimeOut" + key];
        }
        protected void SetTimeOut(string key, TimeSpan timeSpan)
        {
            HttpContext.Current.Session["RandomCodeHelper_TimeOut" + key] = DateTime.Now.Add(timeSpan);
        }
        public T CreateObject<T>(string key, T obj, TimeSpan timeSpan)
        {
            SetTimeOut(key, timeSpan);
            SetValue(key, obj);
            return obj;
        }

        public string CreateHexString(string key, int len, TimeSpan timeSpan)
        {
            var s = Guid.NewGuid().ToString("N").Substring(0, len);
            s = BitConverter.ToString(Encoding.ASCII.GetBytes(s)).Replace("-", "");
            SetTimeOut(key, timeSpan);
            SetValue(key, s);
            return s;
        }

        public T GetObject<T>(string key, bool throwIfTimeout = true)
        {
            var timeout = GetTimeOut(key);
            if (timeout < DateTime.Now)
            {
                if (throwIfTimeout)
                    throw new ApplicationException("²Ù×÷³¬Ê±");
                else return default(T);
            }
            SetTimeOut(key, TimeSpan.Zero);
            var value = GetValue(key);
            SetValue(key, null);
            return (T)value;
        }
    }
}