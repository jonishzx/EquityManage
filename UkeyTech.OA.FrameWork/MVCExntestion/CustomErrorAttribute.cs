using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;

namespace UkeyTech.OA.FrameWork.MVCExntestion
{
    public class CustomExceptionAttribute : FilterAttribute, IExceptionFilter   //HandleErrorAttribute
    {

        public void OnException(ExceptionContext filterContext)
        {
            string controllerName = (string)filterContext.RouteData.Values["controller"];
            string actionName = (string)filterContext.RouteData.Values["action"];
            filterContext.Controller.TempData["QueryPath"] = filterContext.HttpContext.Request.Url.PathAndQuery;
            HandleErrorInfo model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);
           
            ViewResult result = new ViewResult();
            result.ViewData = new ViewDataDictionary<HandleErrorInfo>(model);
            result.TempData = filterContext.Controller.TempData;
    

            if (filterContext.ExceptionHandled == true)
            {
                var httpExce = filterContext.Exception;
                if (httpExce is HttpException && ((HttpException)httpExce).GetHttpCode() != 500)//为什么要特别强调500 因为MVC处理HttpException的时候，如果为500 则会自动
                //将其ExceptionHandled设置为true，那么我们就无法捕获异常
                {
                    return;
                }
            }
            //HttpException httpException = filterContext.Exception as HttpException;
            if (filterContext.Exception != null)
            {
                filterContext.Controller.ViewBag.UrlRefer = filterContext.HttpContext.Request.UrlReferrer;
                
                result.ViewName = "~/Views/Error/Index.aspx";
             
                filterContext.Result = result;
            }
            //写入日志 记录
            filterContext.ExceptionHandled = true;//设置异常已经处理
        }
    }
}
