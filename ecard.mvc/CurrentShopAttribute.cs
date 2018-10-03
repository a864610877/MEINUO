using System.Linq;
using System.Web.Mvc;
using Ecard.Models;
using Ecard.Services;
using Microsoft.Practices.Unity;

namespace Ecard.Mvc
{
    public class CurrentShopAttribute : CustomModelBinderAttribute, IModelBinder
    {
        public override IModelBinder GetBinder()
        {
            return this;
        }

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var membershipService = EcardContext.Container.Resolve<IMembershipService>();
            var helper = EcardContext.Container.Resolve<SecurityHelper>();

            var user = helper.GetCurrentUser();
            if (user == null) return null;
            return null;
        }
    }
}