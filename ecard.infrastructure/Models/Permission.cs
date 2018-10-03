using System.Runtime.Serialization;

namespace Ecard.Models
{
    public class Permission : INamedEntity
    {
        [DataMember]
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public int PermissionId { get { return (Name ?? "").GetHashCode(); } }
        public string Category { get; set; }
        public UserType[] UserTypes { get; set; }
    }
    public interface ICategory
    {
        string Category { get;   }
    }
}