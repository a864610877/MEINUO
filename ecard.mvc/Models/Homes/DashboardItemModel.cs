using Ecard.Models;
using Moonlit;

namespace Ecard.Mvc.Models.Homes
{
    public class DashboardItemModel
    {
        private readonly MethodDescriptor _method;
        private readonly DashboardItem _item;

        public DashboardItemModel(MethodDescriptor method, DashboardItem item)
        {
            _method = method;
            _item = item;
        }

        public string Icon
        {
            get
            {
                return GetIconName(Item.Controller ,Item.Action);
            }
        }
        public string Text
        {
            get
            { 
                return _method.Name;
            }
        }
        public static string GetIconName(string controllerName, string action)
        {
            if (controllerName.EndsWith("Controller"))
                controllerName = controllerName.Substring(0, controllerName.Length - 10);
            return controllerName + "-" + action;
        }
        public DashboardItem Item
        {
            get { return _item; }
        }
    }
}