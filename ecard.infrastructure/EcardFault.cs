using System;
using System.Configuration;
using System.Runtime.Serialization;

namespace Ecard.Infrastructure
{
    public class SerialNoHelper
    {
        static SerialNoHelper()
        {
            if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["serialNoPrefix"]))
                _serialNoPrefix = ConfigurationManager.AppSettings["serialNoPrefix"];
        }

        private static string _serialNoPrefix = "WN";
        private static int Index = 0;
        public static string Create()
        {
            Index++;
            if (Index >= 1000)
                Index = 1;
            return _serialNoPrefix + DateTime.Now.ToFileTime().ToString() + Index.ToString().PadLeft(4, '0');
        }
    }
}