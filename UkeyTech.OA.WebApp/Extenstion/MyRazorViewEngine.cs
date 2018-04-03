using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UkeyTech.OA.WebApp.Extenstion
{
    public class MyRazorViewEngine : RazorViewEngine
    {
        public MyRazorViewEngine()
        {
            ViewLocationFormats = new[]
            {
                    "~/Views/{1}/{0}.cshtml",
                    "~/Views/Shared/{0}.cshtml",
                    "~/Views/Areas/{1}/{0}.cshtml"
            };
            AreaViewLocationFormats = new[]
            {
                    "~/Areas/{2}/Views/{1}/{0}.cshtml",
                    "~/Areas/{2}/Views/Shared/{0}.cshtml"
             };
            AreaPartialViewLocationFormats = new[]
            {
                 "~/Areas/{2}/Views/{1}/{0}.cshtml",
                 "~/Areas/{2}/Views/Shared/{0}.cshtml"
            };
        }
        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            return base.FindView(controllerContext, viewName, masterName, useCache);

        }
        public static void Config()
        {
            ViewEngines.Engines.Add(new MyRazorViewEngine());
        }
    }
}