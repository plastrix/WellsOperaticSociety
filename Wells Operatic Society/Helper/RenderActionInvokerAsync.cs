﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace WellsOperaticSociety.Web.Helper
{
    public class RenderActionInvokerAsync : System.Web.Mvc.Async.AsyncControllerActionInvoker
    {

        protected override ActionDescriptor FindAction(ControllerContext controllerContext, ControllerDescriptor controllerDescriptor, string actionName)
        {
            var ad = base.FindAction(controllerContext, controllerDescriptor, actionName);

            if (ad == null)
            {
                //check if the controller is an instance of IRenderMvcController
                if (controllerContext.Controller is IRenderMvcController)
                {
                    return new ReflectedActionDescriptor(
                        controllerContext.Controller.GetType().GetMethods()
                            .First(x => x.Name == "Index" &&
                                        x.GetCustomAttributes(typeof(NonActionAttribute), false).Any() == false),
                        "Index",
                        controllerDescriptor);

                }
            }
            return ad;
        }

    }
}