namespace Ecard.Models
{
    /// <summary>
    /// �����ֵ�ʵ��
    /// </summary>
    public interface INamedEntity
    {
        string Name { get; set; }
        string DisplayName { get; set; }
    }
}