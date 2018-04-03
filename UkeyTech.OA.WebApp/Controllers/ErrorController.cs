using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UkeyTech.OA.WebApp.Controllers
{
    [HandleError]
    public class ErrorController : Controller
    {
        /// <summary>
        /// 无法找到页面错误
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult NoPageFound()
        {
            return View();
        }

        public ActionResult NotAuth()
        {
            return View();
        }

        /// <summary>
        /// 内部页面错误
        /// </summary>
        /// <returns></returns>
        public ActionResult InternalError()
        {
            return View("Index");
        }
    }
}
