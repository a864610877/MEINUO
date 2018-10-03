using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ecard.Models;
using Ecard.Mvc.ViewModels;
using Ecard.Services;
using Microsoft.Practices.Unity;
using Moonlit;

namespace Ecard.Mvc.Models.Homes
{
    public class DashboardHome : EcardModelList<DashboardItemModel>
    {
        [Dependency]
        [NoRender]
        public IDashboardItemRepository DashboardItemRepository { get; set; }
        [Dependency]
        [NoRender]
        public IControllerFinder ControllerFinder { get; set; }
        [Dependency]
        [NoRender]
        public SecurityHelper SecurityHelper { get; set; }

        public void Ready()
        {
            var user = SecurityHelper.GetCurrentUser();
            var dashboardItemModels = (from x in DashboardItemRepository.Query()
                                       let controllerType = ControllerFinder.FindController(x.Controller)
                                       let method = ViewModelDescriptor.GetTypeDescriptor(controllerType).GetMethod(x.Action)
                                       where method != null && method.Permission.Check(user.CurrentUser)
                                       select new DashboardItemModel(method, x)).ToList();
            List = dashboardItemModels;
        }
    }
}
