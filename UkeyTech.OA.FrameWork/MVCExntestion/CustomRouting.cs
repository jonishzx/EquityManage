using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace UkeyTech.OA.WebApp.Extenstion
{
    /// <summary>
    /// 自定义路由(Folder,如~/Views/Home/Routing/)
    /// </summary>
    public class CustomRouting : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string value = null;
            if (!Folder.Contains(".aspx"))
            {
                value = filterContext.RouteData.Values["action"].ToString() + ".aspx";
                filterContext.RouteData.Values["action"] = Folder + value;
            }
            else
            {
                value = Folder;
                filterContext.RouteData.Values["action"] = value;
            }
          
            base.OnActionExecuting(filterContext);
        }

        public CustomRouting(string folder)
        { this.Folder = folder; }

        public string Folder { get; set; }
    }
}