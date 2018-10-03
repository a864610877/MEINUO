using System;
using System.Web;
using Microsoft.Practices.Unity;

namespace Ecard.Mvc
{
    public class PerWebRequestLifetimeManager : LifetimeManager
    {
        private readonly string _key;

        public PerWebRequestLifetimeManager(string key)
        {
            _key = key;
        }

        public PerWebRequestLifetimeManager()
        {
            _key = "PerWebRequestLifetime_" + Guid.NewGuid().ToString("N");
        }

        public override object GetValue()
        {
            HttpContext context = HttpContext.Current;
            if (context == null)
            {
                return null;
            }
            return context.Items[_key];
        }

        public override void SetValue(object newValue)
        {
            HttpContext context = HttpContext.Current;
            if (context == null) return;
            context.Items[_key] = newValue;
        }

        public override void RemoveValue()
        {
            HttpContext context = HttpContext.Current;
            if (context == null) return;
            context.Items.Remove(_key);
        }
    }
}