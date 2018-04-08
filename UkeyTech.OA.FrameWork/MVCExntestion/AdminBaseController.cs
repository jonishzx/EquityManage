using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Principal;
using System.Web.Security;
using Clover.Web.Core;
using StructureMap;
using Clover.Component.Excel;
using Clover.Config;
using UkeyTech.WebFW.DAO;

namespace UkeyTech.OA.WebApp.Extenstion
{
    /// <summary>
    /// 后台管理 登录 handler
    /// </summary>
    public class AdminBaseController : Controller
    {
        protected string SuccessView = "~/Areas/Admin/Views/Shared/Waiting.aspx";


        /// <summary>
        /// 界面是否编辑状态
        /// </summary>
        public bool IsEdit
        {
            get { return ViewData["EDIT"] != null && (bool)ViewData["EDIT"]; }
            set { ViewData["EDIT"] = value; }
        }

        /// <summary>
        /// 页码信息
        /// </summary>
        public int PageIndex { get { return Utility.GetFormIntParm("page", 1); } }

        /// <summary>
        /// 页记录条数
        /// </summary>
        public int PageSize { get { return Utility.GetFormIntParm("rows", 1); } }

        public void AppendRegisterScriptBlock(string script)
        {
            if (ViewData["ScriptBlock"] != null)
                ViewData["ScriptBlock"] += ";" + script;
            else
                ViewData["ScriptBlock"] = script;
        }

        public void RegisterScriptBlock(string script)
        {
            ViewData["ScriptBlock"] = script;
        }

        /// <summary>
        /// 刷新打开者的内容并关闭通过window.open方法打开的窗体
        /// </summary>
        public ActionResult RefreshParentAndCloseWindow()
        {
            return RefreshParentAndClose("CloseTheWin();");
        }

        private string GetJsStr(string source)
        {
            return System.Text.RegularExpressions.Regex.Replace(source, "(\r)|(\n)|(')", "");
        }

        /// <summary>
        /// 刷新打开者的内容并关闭通过SetWin方法打开的窗体
        /// </summary>
        public ActionResult RefreshParentAndCloseFrame()
        {
            return RefreshParentAndClose("p.CloseWin();");
        }

        public ActionResult RefreshParentAndClose(string closemethod)
        {
            ViewData["ScriptBlock"] = "var p = GetRealParent();p.RunBackFunc();" + closemethod + ";";
            return View(SuccessView);
        }

        public void ShowMessage(string title, string message, string url)
        {
            ViewData["ScriptBlock"] = "$(document).ready(function(){MsgAlertAndNav('" + title
               + "','" + message + "','" + url + "');});";
        }

      
        public void ShowMessageAndRefreshParent(string title, string message)
        {
            ViewData["ScriptBlock"] = "$(document).ready(function(){MsgAlert('" + title
               + "','" + message + "'); var p = GetRealParent();p.RunBackFunc();});";
        }

        /// <summary>
        /// 显示信息并定时关闭窗体
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="timeoutSec"></param>
        public void ShowMessageAndClose(string title, string message, int timeoutSec)
        {
            ViewData["ScriptBlock"] = "$(document).ready(function(){MsgAlert('" + title
               + "','" + message + "'); setTimeout(\"CloseTheWin()\", " + timeoutSec * 1000 + ");});";
        }

        /// <summary>
        /// 显示信息,点击确定后关闭窗体
        /// </summary>
        /// <param name="title">提示标题</param>
        /// <param name="message">提示信息</param>
        public void ShowMessageUnClose(string title, string message)
        {
            ViewData["ScriptBlock"] = "$(document).ready(function(){MsgAlert('" + title
               + "','" + message + "',null,function(){CloseTheWin();}); });";
        }

        public void ShowMessage(string title, string message)
        {
            ShowMessage(title, message, string.Empty);
        }

        public void ShowMessage(string message)
        {
            ShowMessage("信息", message, string.Empty);
        }

        public void ShowMessageAndRoute(string message, string url)
        {
            ShowMessage("提示", message, url);
        }

        public void ShowError(string title, string message, string url)
        {
            ShowError(title, message, url, string.Empty);
        }

        /// <summary>
        /// 发送错误提示到前端
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="message">消息</param>
        /// <param name="url">提示后跳转的URL</param>
        /// <param name="myRenderAction">消息里面附加的动作</param>
        public void ShowError(string title, string message, string url, string myRenderAction)
        {
            if (!string.IsNullOrEmpty(url))
            {
                ViewData["ScriptBlock"] = "$(document).ready(function(){ShowErrorAndNav('" + title
                    + "','" + message.Replace("'", "\"").Replace("\r", "").Replace("\n", "") + "','" + url + "');});";
            }
            else {
                ViewData["ScriptBlock"] = "$(document).ready(function(){ShowError('" + title
                 + "','" + message.Replace("'", "\"").Replace("\r", "").Replace("\n", "") + "'"
                 + (!string.IsNullOrEmpty(myRenderAction) ? "," + myRenderAction : "") + ");});";
            }
        }

        public void ShowError(string title, string message, string url, Exception ex)
        {
            if (!string.IsNullOrEmpty(url))
            {
                ViewData["ScriptBlock"] = "$(document).ready(function(){MsgAlertAndNav('" + title
                    + "','" + message.Replace("'", "\"").Replace("\r", "").Replace("\n", "") + "','" + url + "');});";
            }
            else
            {
                ViewData["ScriptBlock"] = "$(document).ready(function(){MsgAlert('" + title
                 + "','" + message.Replace("'", "\"").Replace("\r", "").Replace("\n", "") + "');});";
            }
        }

        public void ShowError(string title, string message)
        {
            ShowError(title, message, string.Empty);
        }

        public void ShowError(string message)
        {
            ShowError("错误", message, string.Empty);
        }

        private IWebContext _webcontext;

        /// <summary>
        /// 获取当前上下文
        /// </summary>
        public IWebContext WebContext
        {
            get
            {

#if DEBUG
                //IWebContext _webcontext = ObjectFactory.GetInstance<IWebContext>();


                //Clover.Core.Domain.IAccount user = new UkeyTech.WebFW.Model.Admin();
                //user.UniqueId = "90EC66D8-F9A3-40DC-B532-7E350BDF3169";
                //user.UserName = "Mock测试管理员";
                //user.AccountCode = "Admin";
                //_webcontext.CurrentUser = user;


                //user.UniqueId = "7B415DA9-EAB9-4973-9B40-4DC225CB6608";
                //user.UserName = "Mock模块用户01";
                //user.AccountCode = "tester1013";
                //_webcontext.CurrentUser = user;
#endif

                return _webcontext;
            }
        }

        public AdminBaseController()
        {
            string message;
            if (!WebSiteConfig.CheckAdminIpAccess(out message))
            {
                Response.Write(message);
                Response.End();
            }

            _webcontext = ObjectFactory.GetInstance<IWebContext>();

            //兼容windows集成登录
            if (WebContext.CurrentUser == null && System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.User != null && !string.IsNullOrEmpty(System.Web.HttpContext.Current.User.Identity.Name))
            {
                var dal = ObjectFactory.GetInstance<AdminDAO>();
                var replaceDomainName = (System.Configuration.ConfigurationManager.AppSettings["replaceDomainName"] ?? "") == "1";
                var mappingAccount = System.Web.HttpContext.Current.User.Identity.Name;

                if (replaceDomainName)
                {
                    var sp = mappingAccount.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                    mappingAccount = sp.Length > 1 ? sp[1] : sp[0];
                }
                //查找全局信息
                var user = dal.GetAdminByMappingAccount(mappingAccount);
                _webcontext.CurrentUser = user;
            }

            if (WebContext.CurrentUser != null)
            {
                ViewData["CurrentUserId"] = WebContext.CurrentUser.UniqueId;
                ViewData["CurrentUserName"] = WebContext.CurrentUser.UserName;
            }

        }

        /// <summary>
        /// View 指向的默认 路径
        /// </summary>
        public string ViewBaseFilePath { get; set; }

        public List<Clover.Permission.Model.UserFuncPMResult> Permissions
        {
            get
            {
                return Helper.CurrUserPermission;
            }
        }


        protected ContentResult Success(string flagcode, string content)
        {
            return Content(flagcode + ":" + content);
        }

        /// <summary>
        /// 返回json成功提示
        /// </summary>
        /// <returns></returns>
        protected ContentResult Success()
        {
            return Success("");
        }
        /// <summary>
        /// 返回json成功提示
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        protected ContentResult Success(string content)
        {
            return Success("1", content);
        }

        /// <summary>
        /// 返回json失败提示
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        protected ContentResult Fail(string content)
        {
            return Fail("0", content.Replace("'", ""));
        }

        protected ContentResult Fail(string flagcode, string content)
        {
            return Content(flagcode + ":" + content.Replace("'",""));
        }

        protected ActionResult ExportExcel(string sheetname, System.Data.DataTable dt, Dictionary<string, string> dict, string[] titles, string[] columns, string outputFileName, string templatepath)
        {

            return ExportExcel(sheetname, dt, dict, titles, columns, 1, 0, outputFileName, templatepath);
        }


        protected ActionResult ExportExcel(string sheetname, System.Data.DataTable dt, Dictionary<string, string> dict, string[] titles, string[] columns, int startRowIndex, int startColumnIndex, string outputFileName, string templatepath)
        {

            ExcelUtilities eu = new ExcelUtilities();

            var extension = ".xls";
            //var outputFileName = "数据";

            var vtemplatepath = new System.IO.FileInfo(HttpContext.Server.MapPath(templatepath));
            var outputdir = HttpContext.Server.MapPath("~/Download/Temp");
            if (!System.IO.Directory.Exists(outputdir))
                System.IO.Directory.CreateDirectory(outputdir);
            var outputfile = System.IO.Path.Combine(outputdir, Guid.NewGuid().ToString() + extension);

            eu.FillDataToExcelFile(dt, 65536, sheetname, startRowIndex, startColumnIndex,
                titles,
                columns,
                 dict,
                 (vtemplatepath.Exists ? vtemplatepath.FullName : ""),
                 outputfile);

            return File(outputfile, Utility.ConvertFileType(extension), outputFileName + extension);
        }
    }
}
