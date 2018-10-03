using System;
using System.Linq;
using System.Text;

namespace Ecard.Models
{
    public class DashboardItem
    {
        public string Action { get; set; }
        public string Controller { get; set; }
    }
    public interface IControllerFinder
    {
        Type FindController(string controllerName);
    }
}
