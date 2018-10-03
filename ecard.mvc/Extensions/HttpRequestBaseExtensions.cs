using System;
using System.Collections.Generic;
using System.Net;
using System.Web;

namespace Ecard.Mvc
{
    public static class HttpRequestBaseExtensions
    {
        public static IPAddress GetUserIPAddress(this HttpRequestBase request)
        {
            IPAddress address;

            if (!IPAddress.TryParse(request.UserHostAddress, out address))
            {
                address = null;
            }

            return address;
        }

        public static IDictionary<string, object> GetQueryStrings(this HttpRequestBase request)
        {
            IDictionary<string, object> dict = new Dictionary<string, object>();
            foreach (object queryString in request.QueryString)
            {
                string key = queryString.ToString();
                string value = request[key];
                if (string.Equals(key, "timestamp", StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }
                dict.Add(key, value);
            }
            return dict;
        }

        public static string GenerateAntiForgeryToken(this HttpRequestBase request, string key, string salt)
        {
            return (key + salt + request.UserAgent).ComputeHash();
        }
    }
}