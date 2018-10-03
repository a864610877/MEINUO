using System.Runtime.Serialization;

namespace Ecard.Infrastructure
{
    [DataContract]
    public class MakeDogResult
    {
        [DataMember]
        public string Key { get; set; }
    }
}