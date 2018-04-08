using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Principal;
using System.Web.Security;
using Clover.Web.Core;
using StructureMap;


namespace UkeyTech.OA.WebApp.Extenstion
{
    /// <summary>
    /// 后台管理基础handler
    /// </summary>
    public class AdminLoginController : AdminBaseController
    {
        
        public AdminLoginController()
        {                  
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            if (WebContext.CurrentUser == null)
            {
                ViewData["ErrorName"] = "你无权限访问，请重新登录。";

                filterContext.Result = 
                    Helper.ForbiddenView("主页", filterContext.HttpContext.Request.Url.PathAndQuery, true);
            }            

        }
    }
}
