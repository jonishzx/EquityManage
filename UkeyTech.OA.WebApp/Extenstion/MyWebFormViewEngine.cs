using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UkeyTech.OA.WebApp.Extenstion
{
    public class MyWebFormViewEngine  :WebFormViewEngine
    {
        public MyWebFormViewEngine()
        {
            ViewLocationFormats = new[]
            {
                "~/Views/{1}/{0}.aspx",
                "~/Views/Shared/{0}.aspx",
                "~/Views/Areas/{1}/{0}.aspx"
            };
            AreaViewLocationFormats = new[]
            {
                "~/Areas/{2}/Views/{1}/{0}.aspx",
                "~/Areas/{2}/Views/Shared/{0}.aspx",

                "~/Areas/{2}/Views/Base/{1}/{0}.aspx",
                "~/Areas/{2}/Views/User/{1}/{0}.aspx",
                "~/Areas/{2}/Views/Case/{1}/{0}.aspx",

                "~/Areas/{2}/Views/Config/Shared/{0}.aspx",
                "~/Areas/{2}/Views/User/Shared/{0}.aspx",
                "~/Areas/{2}/Views/Case/Shared/{0}.aspx"
            };
        }

        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            return base.FindView(controllerContext, viewName, masterName, useCache);
        }

        public static void Config()
        {
            ViewEngines.Engines.Add(new MyWebFormViewEngine());
        }
    }
}