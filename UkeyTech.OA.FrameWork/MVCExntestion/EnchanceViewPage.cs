using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UkeyTech.OA.WebApp.Extenstion
{
    public class EnchanceViewPage<T> : System.Web.Mvc.ViewPage<T>
    {
        /// <summary>
        /// 界面是否编辑状态
        /// </summary>
        public bool IsEdit
        {
            get { return ViewData["EDIT"] != null && (bool)ViewData["EDIT"]; }
            set { ViewData["EDIT"] = value; }
        } 
    }

}