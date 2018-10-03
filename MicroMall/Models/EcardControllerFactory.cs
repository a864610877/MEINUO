//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System;
using System.Web.Mvc;
using Microsoft.Practices.Unity;

namespace MicroMall.Models
{
    public class EcardControllerFactory : DefaultControllerFactory
    {
        private readonly IUnityContainer container;

        public EcardControllerFactory(IUnityContainer container)
        {
            this.container = container;
        } 
        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            try
            {
                if (controllerType == null) return null;
                IController icontroller = container.Resolve(controllerType) as IController;
                if (typeof(Controller).IsAssignableFrom(controllerType))
                {
                    Controller controller = icontroller as Controller;

                    if (controller != null)
                        controller.ActionInvoker = container.Resolve<IActionInvoker>();
                     
                    return icontroller;
                }

                return icontroller;
            }
            catch (Exception)
            {
                return base.GetControllerInstance(requestContext, controllerType);
            }
        }
    }
}
