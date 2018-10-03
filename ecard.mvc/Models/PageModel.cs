using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ecard.Models;
using Ecard.Mvc.ViewModels;

namespace Ecard.Mvc.Models
{
    public class PageModel
    {
        public PageModel()
        {
            CurrentMenu = new Dictionary<int, MenuItem>();
        }
        public List<MenuItem> Menus { get; set; }
        public SiteViewModel Site { get; set; }
        internal Dictionary<int, MenuItem> CurrentMenu { get; set; }
        public User User { get; set; }
    }
}
