namespace Ecard.Models
{
    /// <summary>
    /// 有名字的实体
    /// </summary>
    public interface INamedEntity
    {
        string Name { get; set; }
        string DisplayName { get; set; }
    }
}