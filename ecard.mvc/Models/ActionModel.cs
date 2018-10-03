using System.Web.Routing;

namespace Ecard.Mvc.Models
{
    public class ActionModel
    {
        public string FormatedConfirmMessage
        {
            get { return ConfirmMessage; }
        }
        public string Text { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
        public RouteValueDictionary RouteValues { get; set; }
        public string ConfirmMessage { get; set; }
        public bool IsPost { get; set; }
    }
}