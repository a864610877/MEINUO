namespace Ecard.Mvc.Models
{
    public enum RollbackType
    {
        None,
        Cancel,
        Undo,
    }
    public class ClickAlert
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }
}