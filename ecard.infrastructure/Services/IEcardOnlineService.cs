using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using Ecard.Infrastructure;

namespace Ecard.Services
{
    public class ServiceProxy<T> : ClientBase<T> where T : class
    {
        public T Proxy { get { return Channel; } }
    }
}