using System;
using System.Web;
using Microsoft.Practices.Unity;

namespace Ecard
{
    public class EcardContext
    {
        private EcardContext()
        {
        }

        public static EcardContext Current
        {
            get
            {
                var context = (EcardContext) HttpContext.Current.Items["______OxiteContext"];
                if (context == null)
                {
                    HttpContext.Current.Items["______OxiteContext"] = new EcardContext();
                }
                return (EcardContext) HttpContext.Current.Items["______OxiteContext"];
            }
        }

        public static IUnityContainer Container
        {
            get { return AppDefaults.Container; }
            set { AppDefaults.Container = value; }
        }

        public static string CreateToken()
        {
            var random = new Random();
            return Guid.NewGuid().ToString("N").Replace("-", "").Substring(random.Next(0, 10), 8).ToLower().Replace(
                "f", "e");
        }

        public static string TokenToBinnaryString(string key)
        {
            byte[] binnary = Convert.FromBase64String(key);
            return BitConverter.ToString(binnary).Replace("-", "");
        }
    }
}