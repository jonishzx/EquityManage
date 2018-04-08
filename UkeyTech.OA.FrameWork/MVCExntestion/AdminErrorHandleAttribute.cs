using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Clover.Web.Core;

namespace UkeyTech.OA.WebApp.Extenstion
{
     /// <summary>
     /// 管理台程序错误控制
     /// </summary>
     public class AdminErrorHandleAttribute : HandleErrorAttribute
     {
        public string ErrorName{get;set;}
        public string ErrorMessage{get;set;}

        public override void OnException(ExceptionContext filterContext)
        {
            try{                
                Clover.Core.Alias.log.Current.Error("管理台运行时发生错误错误", filterContext.Exception);
            }
            catch{}

            //添加错误信息
         
            filterContext.Controller.TempData["ErrorName"] = ErrorName;

            filterContext.Controller.TempData["ErrorMessage"] = ErrorMessage;

            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            if (!filterContext.IsChildAction && (!filterContext.ExceptionHandled && filterContext.HttpContext.IsCustomErrorEnabled))
            {
                Exception innerException = filterContext.Exception;
                if ((new HttpException(null, innerException).GetHttpCode() == 500) && this.ExceptionType.IsInstanceOfType(innerException))
                {
                    string controllerName = (string) filterContext.RouteData.Values["controller"];
                    string actionName = (string) filterContext.RouteData.Values["action"];
                    HandleErrorInfo model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);
                    filterContext.Result = new ViewResult { ViewName = this.View, MasterName = this.Master, ViewData = new ViewDataDictionary<HandleErrorInfo>(model), TempData = filterContext.Controller.TempData};
                    filterContext.ExceptionHandled = true;
                    filterContext.HttpContext.Response.Clear();
                    filterContext.HttpContext.Response.StatusCode = 500;
                    filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
                }
            }
        }
    }
}
