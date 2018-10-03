using System.Runtime.Serialization;

namespace Ecard.Infrastructure
{
    [DataContract]
    public class GetGardsResult
    {
        public GetGardsResult()
        {
            Accounts = new string[0];
        }

        [DataMember]
        public string[] Accounts { get; set; }
    }
}