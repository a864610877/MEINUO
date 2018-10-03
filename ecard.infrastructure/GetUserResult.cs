using System.Runtime.Serialization;

namespace Ecard.Infrastructure
{
    [DataContract]
    public class GetUserResult
    { 
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string UserDisplayName { get; set; }
        [DataMember]
        public string Mobile { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
    }
}