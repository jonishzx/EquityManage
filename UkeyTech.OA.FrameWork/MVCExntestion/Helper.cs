using System;
using System.ComponentModel;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Reflection;
using System.Web;
using System.Text;
using System.Web.UI;
using Clover.Core.Common;
using Clover.Web.Core;
using Clover.Core.Collection;
using Clover.Web.HTMLRender;
using Clover.Permission.BLL;
using StructureMap;
using System.Text.RegularExpressions;
using Clover.Config;
using Clover.Config.FileUpload;
using Module = Clover.Permission.Model.Module;
using System.Collections;
using System.Drawing;
using Clover.Core.Caching;

namespace UkeyTech.OA.WebApp
{
    public class Helper
    {
        public static readonly string AdminRoleCode = "Administrator";
        public static readonly string PermissionRoleCode = "PMAdministrator";
        public static readonly string EmptyRows = "{\"total\":0, \"rows\":[]}";

        public static readonly string DictComboGridPath = "~/Areas/Admin/Views/Shared/DictComboGrid.ascx";
        public static readonly string DictDropDownListPath = "~/Areas/Admin/Views/Shared/DictDropDownList.ascx";
        public static readonly string DictSQLDropDownListPath = "~/Areas/Admin/Views/Shared/DictSQLDropDownList.ascx";
        public static readonly string DictRadioButtonListPath = "~/Areas/Admin/Views/Shared/DictRadioButtonList.ascx";
        public static readonly string PopupControlPath = "~/Areas/Admin/Views/Shared/PopupControl.ascx";
        public static readonly string DictComboTreePath = "~/Areas/Admin/Views/Shared/DictComboTree.ascx";
        public static readonly string AdvSearchPath = "~/Areas/Admin/Views/Shared/AdvSearch.ascx";
        public static readonly string AttachmentPath = "~/Areas/Admin/Views/Shared/Attachments.ascx";
        public static readonly string AttachmentExPath = "~/Areas/Admin/Views/Shared/AttachmentsEx.ascx";
        public static readonly string WorkflowLogPath = "~/Areas/Admin/Views/WorkFlow/WorkFlowLogs.ascx";

        public static readonly string WorkflowOption = "~/Areas/GTEPurchase/Views/WorkFlowCommon/ApproveOpinionControl.ascx";

        //获取工作日
        public static int getWorkDays(int year, int month)
        {
            int m = System.DateTime.DaysInMonth(year, month);
            int mm = 0;
            for (int i = 1; i <= m; i++)
            {
                System.DateTime date = Convert.ToDateTime(year + "-" + month + "-" + i);
                switch (date.DayOfWeek)
                {
                    case System.DayOfWeek.Monday:
                    case System.DayOfWeek.Thursday:
                    case System.DayOfWeek.Tuesday:
                    case System.DayOfWeek.Wednesday:
                    case System.DayOfWeek.Friday:
                        mm = mm + 1;
                        break;
                }
            }
            return mm;
        }

        /// <summary>
        /// 检查密码复杂度（大写字母、小写字母、数字、符号任意3种）
        /// </summary>
        /// <param name="pwd">输入的密码</param>
        /// <returns></returns>
        public static bool CheckPasswordComplexity(string pwd)
        {
            var count = 0;
            if (string.IsNullOrEmpty(pwd) || pwd.Length < 6 || pwd.Length > 16)
                return false;
            if(Regex.IsMatch(pwd, "[0-9]"))
                count++;
            if(Regex.IsMatch(pwd, "[a-z]"))
            count++;
            if(Regex.IsMatch(pwd, "[A-Z]"))
            count++;
            if(Regex.IsMatch(pwd, "[~!@#$%^*()_+]"))
            count++;
            return count >= 3;
        }

        public static string UploadTempFile(string uploadtype, string guid, HttpPostedFileBase file, out string msg)
        {
            FilesUploadInfo filecfg = FileUploadConfig.GetFUConfig(uploadtype);

            FileInfo finfo = new FileInfo(file.FileName);
            string filepath = string.Empty;

            int uploadsize = file.ContentLength;
            if (uploadsize > filecfg.MaxFileSize * 1024) //大小判断
            {
                msg = "最大文件大小只允许:" + filecfg.MaxFileSize + "kb";
            }
            else if (!filecfg.ExtAllowedList.Contains(finfo.Extension.Replace(".", "")))
            {
                msg = "只允许上传以下文件格式:" + filecfg.ExtAllowed;
            }
            else
            {
                //文件上传
                filepath = UploadFile(filecfg, guid, file, finfo.Name, out msg);
            }

            return filepath;
        }

        /// <summary>
        /// 文件上传操作
        /// </summary>
        /// <param name="filecfg"></param>
        /// <param name="guid"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private static string UploadFile(FilesUploadInfo filecfg, string guid, HttpPostedFileBase file, string filename, out string msg)
        {
            string filepath = string.Empty;
            try
            {

                string rootpath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(filecfg.TempPath), guid);
                if (!Directory.Exists(rootpath))
                {
                    Directory.CreateDirectory(rootpath);
                }

                filepath = Path.Combine(rootpath, filename);

                file.SaveAs(filepath);

                string imgurl = Utility.ConvertAbsoulteUrl(filecfg.TempPath + "/" + guid + "/" + filename);
                msg = "success##" + imgurl;

            }
            catch (Exception ex)
            {
                Clover.Core.Alias.log.Current.Error("文件上传失败", ex);
                msg = "文件上传失败,请联系管理员";
            }

            return filepath;

        }

        /// <summary>
        /// 序列化DataTable的字符串
        /// </summary>
        /// <param name="dt">输入的DataTable</param>
        /// <param name="tablename">数据表名称</param>
        /// <param name="totalcount">总行数</param>
        /// <returns>返回经过json格式化的DataTable字符串</returns>
        public static string GetDataTableJsonStr(System.Data.DataTable dt, string tablename, int totalcount)
        {

            StringBuilder sb = new StringBuilder();
            sb.Append("{\"total\":" + totalcount.ToString() + ",\"rows\":");
            dt.TableName = tablename;
            sb.Append(Newtonsoft.Json.JsonConvert.SerializeObject(dt, new Newtonsoft.Json.Converters.IsoDateTimeConverter()));
            sb.Append("}");
            return sb.ToString();
        }

        public static string GetDataTableJsonStr(System.Data.DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Newtonsoft.Json.JsonConvert.SerializeObject(dt, new Newtonsoft.Json.Converters.IsoDateTimeConverter()));
            return sb.ToString();
        }


        /// <summary>
        /// 序列化DataTable的字符串
        /// </summary>
        /// <param name="dt">输入的DataTable</param>
        /// <param name="totalcount">总行数</param>
        /// <returns>返回经过json格式化的DataTable字符串</returns>
        public static string GetDataTableJsonStr(System.Data.DataTable dt, int totalcount)
        {
            return GetDataTableJsonStr(dt, "Temp", totalcount);
        }

        /// <summary>
        /// 序列化List<T>的字符串
        /// </summary>
        /// <param name="dt">输入的List</param>
        /// <param name="totalcount">总行数</param>
        /// <returns>返回经过json格式化的DataTable字符串</returns>
        public static string GetListJsonStr<T>(List<T> list, int totalcount)
        {
            return GetListJsonStr<T>(list, totalcount, null);
        }


        public static string GetListJsonStr<T>(List<T> list, int totalcount, List<List<KeyValuePair<string, string>>> footer)
        {

            string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(list, new Newtonsoft.Json.Converters.IsoDateTimeConverter());

            StringBuilder sb = new StringBuilder();
            sb.Append("{\"total\":" + totalcount.ToString() + ",\"rows\":");
            sb.Append(jsonstr != "null" ? jsonstr : "[]");
            if (footer != null && footer.Count > 0)
            {
                sb.Append(",\"footer\":[ ");
                foreach (var o in footer)
                {
                    sb.Append("{");
                    foreach (var p in o)
                    {
                        sb.Append("\"" + p.Key + "\":" + "\"" + p.Value + "\"");
                    }
                    sb.Append("},");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("]");
            }
            sb.Append("}");
             return sb.ToString();
        }

        /// <summary>
        /// 序列化List<T>的字符串
        /// </summary>
        /// <param name="dt">输入的List</param>
        /// <param name="totalcount">总行数</param>
        /// <returns>返回经过json格式化的DataTable字符串</returns>
        public static string GetTreeListJsonStr<T>(List<T> list, int totalcount) where T : Clover.Core.Collection.ISNode
        {
            if (list.Count == 0)
                return EmptyRows;

            StringBuilder sb = new StringBuilder();
            sb.Append("{\"total\":" + totalcount.ToString() + ",\"rows\":");
            sb.Append("[");
            foreach (var m in list)
            {
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(m, new Newtonsoft.Json.Converters.IsoDateTimeConverter());
                sb.Append(json);
                sb.Append(",");
            }

            sb.Remove(sb.Length - 1, 1);
            sb.Append("]");
            string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(list, new Newtonsoft.Json.Converters.IsoDateTimeConverter());


            sb.Append("}");
            return sb.ToString();
        }

        /// <summary>
        /// 序列化Datatable的字符串
        /// </summary>
        /// <param name="dt">输入的DataTable</param>
        /// <param name="totalcount">总行数</param>
        /// <returns>返回经过json格式化的DataTable字符串</returns>
        public string DataTableJsonStr(DataTable dt, int totalcount)
        {
            return DataTableJsonStr(dt, totalcount, null);
        }

        /// <summary>
        /// 序列化Datatable的字符串
        /// </summary>
        /// <param name="dt">输入的DataTable</param>
        /// <param name="totalcount">总行数</param>
        /// <param name="fields">字段</param>
        /// <returns>返回经过json格式化的DataTable字符串</returns>
        public string DataTableJsonStr(DataTable dt, int totalcount, string[] fields)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            builder.AppendFormat("\"total\":\"{0}\",\"rows\":[", totalcount);
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                foreach (DataRow row in dt.Rows)
                {
                    int num;
                    builder.Append("{");
                    if (fields == null)
                    {
                        num = 0;
                        while (num < dt.Columns.Count)
                        {
                            builder.AppendFormat("\"{0}\":\"{1}\",", dt.Columns[num].ColumnName, row[dt.Columns[num].ColumnName].ToString().Replace(@"\", @"\\"));
                            num++;
                        }
                    }
                    else
                    {
                        for (num = 0; num < fields.Length; num++)
                        {
                            builder.AppendFormat("\"{0}\":\"{1}\",", fields[num], row[fields[num]].ToString().Replace(@"\", @"\\"));
                        }
                    }
                    builder.Remove(builder.Length - 1, 1);
                    builder.Append("},");
                }
                builder.Remove(builder.Length - 1, 1);
            }
            builder.Append("]}");
            return builder.ToString();
        }

        public static string ShowUIElement(object viewdata)
        {
            if (viewdata == null)
                return string.Empty;

            if ((bool)viewdata)
                return " display:none;";
            else
                return string.Empty;
        }

        public static readonly string DefaultSkinID = "skin_0";
        public static string GetSkin()
        {
            var val = CookieHelper.GetCookieValue("sysadmin_skin");
            if (!string.IsNullOrEmpty(val))
                return val;
            else
                return DefaultSkinID;
        }

        readonly static CacheManager cmgr = ObjectFactory.GetInstance<CacheManager>();
        const string permissionCacheKey = "UserFuncPermission";


        private static string getPermssionCacheKey(IWebContext ctx)
        {
            if (ctx.CurrentUser != null)
                return permissionCacheKey + "_" + ctx.CurrentUser.UniqueId;
            return string.Empty;
        }

        /// <summary>
        /// 清除权限
        /// </summary>
        public static void CleanUserPermission()
        {
            var webcontext = ObjectFactory.GetInstance<IWebContext>();
            string userpermissionKey = getPermssionCacheKey(webcontext);           
            cmgr.Backend.Remove(userpermissionKey);           
        }
        public static List<Clover.Permission.Model.UserFuncPMResult> CurrUserPermission
        {
            get
            {
                //缓存方式暂时不使用
                /*
                var webcontext = ObjectFactory.GetInstance<IWebContext>();

                if (webcontext.CurrentUser == null)
                    return null;

                string userpermissionKey = getPermssionCacheKey(webcontext);

                if (!cmgr.Backend.Contains(userpermissionKey))
                {
                    SetPermissionCache(webcontext, userpermissionKey);
                }
                return cmgr.Get(userpermissionKey) as List<Clover.Permission.Model.UserFuncPMResult>;
               */
               
                return SessionHelper.GetFromSession<List<Clover.Permission.Model.UserFuncPMResult>>("PermissionSession");
            }
            set
            {
                //缓存方式暂时不使用
                /*
                var webcontext = ObjectFactory.GetInstance<IWebContext>();

                if (webcontext.CurrentUser == null)
                    return;

                string userpermissionKey = getPermssionCacheKey(webcontext);
                DateTime expireddatetime = DateTime.Now.AddHours(3); //三小时过期
                if (value == null) {
                    cmgr.Remove(userpermissionKey);
                }
                if (!cmgr.Backend.Contains(userpermissionKey))
                {
                    var funcbll = ObjectFactory.GetInstance<FuncPermissionBLL>();
                    var modulelist = funcbll.GetSystemFuncPermission(webcontext.CurrentUser.UniqueId, WebSiteConfig.Config.SystemNo);
                    cmgr.Backend.Set(userpermissionKey, modulelist, expireddatetime);
                }
                cmgr.Backend.Set(userpermissionKey, value, expireddatetime);
                */
             
                SessionHelper.SetInSession("PermissionSession", value);
            }
        }

        private static void SetPermissionCache(IWebContext webcontext, string userpermissionKey)
        {
            if (!cmgr.Backend.Contains(userpermissionKey))
            {
                var funcbll = ObjectFactory.GetInstance<FuncPermissionBLL>();
                var modulelist = funcbll.GetSystemFuncPermission(webcontext.CurrentUser.UniqueId, WebSiteConfig.Config.SystemNo);
                DateTime expireddatetime = DateTime.Now.AddHours(3);
                cmgr.Backend.Set(userpermissionKey, modulelist, expireddatetime);
            }
        }

        /// <summary>
        /// 检查用户权限
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string CheckFuncAccess(string code)
        {
            string[] codes = StringHelper.SplitString(code, ".");
            if (codes.Length == 2)
                return CheckFuncAccess(codes[0], codes[1]);
            else
                return "fasle";
        }

        private static readonly FunctionBLL funcbll = ObjectFactory.GetInstance<FunctionBLL>();
        private static readonly FunctionDataRuleBLL datarulebll = ObjectFactory.GetInstance<FunctionDataRuleBLL>();

        /// <summary>
        /// 根据权限代码获取权限查询字符串
        /// </summary>
        /// <param name="dataRuleCode"></param>
        /// <returns></returns>
        public static string GetDataPermissionRule(string dataRuleCode)
        {
            return FunctionDataRuleBLL.GetFDRuleStr(dataRuleCode);
        }

        /// 根据模块代码及功能代码获取权限查询字符串
        /// </summary>
        /// <param name="dataRuleCode"></param>
        /// <returns></returns>
        public static string GetMFDataPermissionRule(string modulecode, string functioncode)
        {
            var funcs = CurrUserPermission.FindAll(x => x.ModuleCode == modulecode && x.FunctionCode == functioncode);

            string dataruleids = "";//如果没有设置权限，那么就默认为0 看不到
            if (funcs != null && funcs.Count > 0)
            {
                foreach (var fun in funcs)
                {
                    //var it = funcs.Find(x => x.DataPermissionId != null && !x.IsDeny);
                    if (fun.DataPermissionId != null && !fun.IsDeny)
                    {
                        dataruleids = dataruleids + fun.DataPermissionId + ",";
                    }
                }

                dataruleids = !string.IsNullOrEmpty(dataruleids) ? dataruleids.Substring(0, dataruleids.Length - 1) : "0";
            }
            else
            {
                dataruleids = "0";
            }



            //if (funcs.Count > 0)
            //{
            //    funcs.Sort(new DataRuleComparer());
            //    var dataruleid = funcs[0].DataPermissionId;
            //    if (dataruleid.HasValue)
            //    {
            //        var datarule = GetDataPermissionRuleByIds(dataruleids);
            //        if (!string.IsNullOrEmpty(datarule))
            //        {
            //            return datarule;
            //        }
            //    }
            //}


            var datarule = GetDataPermissionRuleByIds(dataruleids);
            if (!string.IsNullOrEmpty(datarule))
            {
                return datarule;
            }

            return string.Empty;
        }
        private class DataRuleComparer : IComparer<Clover.Permission.Model.UserFuncPMResult>
        {
            public int Compare(Clover.Permission.Model.UserFuncPMResult x, Clover.Permission.Model.UserFuncPMResult y)
            {
                if (x.DataPermissionPriority.HasValue && y.DataPermissionPriority.HasValue)
                    return x.DataPermissionPriority.Value - y.DataPermissionPriority.Value;
                else
                    return 0;
            }
        }

        /// <summary>
        /// 根据权限代码获取权限查询字符串
        /// </summary>
        /// <param name="dataRuleCode"></param>
        /// <returns></returns>
        public static string GetDataPermissionRuleById(int id)
        {
            return datarulebll.GetFDRuleStr(id);
        }

        /// <summary>
        /// 根据权限代码获取权限查询字符串
        /// </summary>
        /// <param name="dataRuleCode"></param>
        /// <returns></returns>
        public static string GetDataPermissionRuleByIds(string ids)
        {
            return datarulebll.GetUserDataRuleStrings(ids);
        }

        // UkeyTech.OA.WebApp.Helper
        public static string GetDefaultStyleSheet()
        {
            string text = "~/Content/default." + Clover.Config.WebSiteConfig.Config.SystemNo + ".css";
            if (File.Exists(HttpContext.Current.Server.MapPath(text)))
            {
                return text;
            }
            return "~/Content/default.css";
        }

        /// <summary>
        /// 获取指定方法的数据权限的可访问的用户
        /// </summary>
        /// <param name="WebContext"></param>
        /// <param name="methodname"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetControllerAcceessUsers(IWebContext WebContext, string methodname, Type type)
        {

            var method = type.GetMethod(methodname);
            var attr = method.GetCustomAttributes(typeof(UkeyTech.OA.WebApp.Extenstion.CloverAuthorizeAttribute), true)[0] as UkeyTech.OA.WebApp.Extenstion.CloverAuthorizeAttribute;
            string module = attr.ModuleCode;
            string functioncode = attr.FuncCode;

            var funcs = CurrUserPermission.FindAll(x => x.ModuleCode == module && x.FunctionCode == functioncode);

            int? dataruleid = null;
            if (funcs != null && funcs.Count > 0)
            {
                var it = funcs.Find(x => x.DataPermissionId != null);
                if (it != null)
                {
                    dataruleid = it.DataPermissionId;
                }
            }
            if (dataruleid.HasValue) //具有数据权限,则通过数据权限获取相应的用户
            {
                var list = datarulebll.GetSysAdminRuleOutput(WebContext, dataruleid.Value);
                return string.Join("','", list.ToArray());
            }
            //无数据权限,返回当前用户ID
            return WebContext.CurrentUser.UniqueId;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="WebContext"></param>
        /// <param name="where"></param>
        /// <param name="OrCondition">带OR 的条件</param>
        /// <param name="methodname"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetFmtWhereCondition(IWebContext WebContext, string where, string OrCondition, string methodname, Type type)
        {
            var method = type.GetMethod(methodname);
            var attrs = method.GetCustomAttributes(typeof(UkeyTech.OA.WebApp.Extenstion.CloverAuthorizeAttribute), true);
            if (attrs.Length > 0)
            {
                var attr = attrs[0] as UkeyTech.OA.WebApp.Extenstion.CloverAuthorizeAttribute;
                string module = attr.ModuleCode;
                string functioncode = attr.FuncCode;
                return GetFmtWhereCondition(WebContext, where, OrCondition, module, functioncode);
            }
            return where;
        }
        /// <summary>
        /// 获取格式化的条件(可格局条件)
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public static string GetFmtWhereCondition(IWebContext WebContext, string where, string OrCondition, string Module, string permission)
        {
            string rst = string.Empty;
            var funcs = CurrUserPermission.FindAll(x => x.ModuleCode == Module && x.FunctionCode == permission);
            string dataruleids = "";//如果没有设置权限，那么就默认为0 看不到
            if (funcs != null && funcs.Count > 0)
            {
                foreach (var fun in funcs)
                {
                    //var it = funcs.Find(x => x.DataPermissionId != null && !x.IsDeny);
                    if (fun.DataPermissionId  != null && !fun.IsDeny )
                    {
                        dataruleids = dataruleids + fun.DataPermissionId + ",";
                    }
                }
                dataruleids = !string.IsNullOrEmpty(dataruleids) ? dataruleids.Substring(0, dataruleids.Length - 1) : "0";
            }
            else
            {
                dataruleids = "0";
            }
            
            if (!string.IsNullOrEmpty(dataruleids))
            {
                var datarule = GetDataPermissionRuleByIds(dataruleids);
                if (!string.IsNullOrEmpty(datarule))
                {
                    if (!string.IsNullOrEmpty(OrCondition))
                    {
                        datarule = "( (" + datarule + ")" + " OR " + OrCondition + " )";
                    }
                    where = !string.IsNullOrEmpty(where) ? (where + " and " + datarule) : datarule;
                }
            }
            return Clover.Data.BaseDAO.ParseSQLCommand(WebContext, where);
        }
        /// <summary>
        ///  获取当前用户可用权限代码的部分
        /// </summary>
        /// <param name="modulecode">模块</param>
        /// <param name="funccode">代码,使用 , 号分隔</param>
        /// <returns></returns>
        public static string GetPermissionJson(string ModuleCode, bool withdatarule)
        {
            var list = funcbll.GetModuleFunctions(ModuleCode);
            List<string> moduelfunc = new List<string>();
            foreach (var m in list)
            {
                moduelfunc.Add(m.FunctionCode);
            }

            return Helper.GetPermissionJson(ModuleCode, withdatarule, moduelfunc.ToArray());
        }

        public static string GetPermissionJson(string ModuleCode, params string[] funccode)
        {
            return GetPermissionJson(ModuleCode, false, funccode);

        }
        /// <summary>
        ///  获取当前用户可用权限代码的部分
        /// </summary>
        /// <param name="modulecode">模块</param>
        /// <param name="funccode">代码,使用 , 号分隔</param>
        /// <returns></returns>
        public static string GetPermissionJson(string ModuleCode, bool withdatarule, params string[] funccode)
        {
            IWebContext _webcontext = ObjectFactory.GetInstance<IWebContext>();
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            foreach (string fcode in funccode)
            {
                sb.Append("\"" + fcode + "\":");

                if (_webcontext.CurrentUser == null)
                    sb.Append("false");

                if (_webcontext.CurrentUser.UniqueId != SystemVar.AdminId)
                {
                    if (CurrUserPermission != null && CurrUserPermission.Exists(x => { return x.FunctionCode == fcode && x.ModuleCode == ModuleCode; }))
                        sb.Append("true");
                    else
                        sb.Append("false");
                }
                else
                    sb.Append("true");

                sb.Append(",");
            }

            return sb.ToString().TrimEnd(new char[] { ',' }) + "}";
        }

        /// <summary>
        /// 检查当前操作员的功能权限
        /// </summary>
        /// <param name="modulecode"></param>
        /// <param name="funccode"></param>
        /// <returns></returns>
        public static string CheckFuncAccess(string modulecode, string funccode)
        {
            IWebContext _webcontext = ObjectFactory.GetInstance<IWebContext>();
            if (_webcontext.CurrentUser == null)
                return "false";

            if (_webcontext.CurrentUser.UniqueId != SystemVar.AdminId)
            {
                if (CurrUserPermission != null)
                {
                    if (modulecode.IndexOf(",") >= 0) //多个权限代码
                    {
                        var modulecodes = modulecode.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        var funccodes = funccode.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        var samecompare = modulecodes.Length == funccodes.Length;
                        var findit = false;
                        foreach (var s in modulecodes)
                        {
                            if (samecompare) //模块代码与功能代码同样数目
                            {
                                foreach (var funccode1 in funccodes)
                                {
                                    if (CurrUserPermission.Exists(
                                        x => { return x.FunctionCode == funccode1 && x.ModuleCode == s; }))
                                        findit = true;
                                }
                            }
                            else
                            {
                                //功能代码只有一个
                                if (CurrUserPermission.Exists(
                                    x => { return x.FunctionCode == funccode && x.ModuleCode == s; }))
                                    findit = true;
                            }

                        }

                        return findit ? "true" : "false";
                    }
                    else
                    {
                        return
                            CurrUserPermission.Exists(
                                x => { return x.FunctionCode == funccode && x.ModuleCode == modulecode; })
                                ? "true"
                                : "false";
                    }
                }
                return CurrUserPermission.Exists(x => { return x.FunctionCode == funccode && x.ModuleCode == modulecode; }) ? "true" : "false";
            }
            else
                return "true";
        }

        /// <summary>
        /// 输出树节点html
        /// </summary>
        /// <param name="parentid"></param>
        /// <returns></returns>
        public static string RenderChildrenNodes(string systemid)
        {
            //获取每个子权限的父权限  
            ModuleBLL mbll = ObjectFactory.GetInstance<ModuleBLL>();

            Tree<Module> tree = mbll.GetModuleTree();

            List<Module> nodelist = mbll.GetSystemModules(int.Parse(systemid));

            Tree<Module> tree2 = new Tree<Module>(nodelist);
            return ModuleTreeRender.RenderNodeS(tree2, "");
        }


        /// <summary>
        /// 输出树节点html
        /// </summary>
        /// <param name="parentid"></param>
        /// <returns></returns>
        public static string RenderChildrenNodesVisible(IWebContext context, List<int> modulecodes, string parentid)
        {
            //获取每个子权限的父权限  
            ModuleBLL mbll = ObjectFactory.GetInstance<ModuleBLL>();

            Tree<Module> tree = new Tree<Module>(mbll.GetEnabledModules());

            if (context.CurrentUser.UniqueId != string.Empty && Clover.Web.Core.SystemVar.AdminId
              != context.CurrentUser.UniqueId)
            {
                List<Module> nodelist = new List<Module>();

                foreach (var code in modulecodes)
                {
                    Module it = tree.GetById(code.ToString());

                    if (it == null)
                        continue;

                    if (!nodelist.Contains(it))
                        nodelist.Add(it);

                    Module parentit = tree.GetById(it.ParentId);

                    while (parentit != null)
                    {
                        if (!nodelist.Contains(parentit))
                            nodelist.Add(parentit);

                        if (!string.IsNullOrEmpty(parentit.ParentId))
                            parentit = tree.GetById(parentit.ParentId.ToString());
                        else
                            parentit = null;
                    }
                }

                Tree<Module> tree2 = new Tree<Module>(nodelist);
                return ModuleTreeRender.RenderNodeSWithPrivilege(tree2, parentid);
            }
            else
            {
                return ModuleTreeRender.RenderNodeS(tree, parentid.ToString());
            }
        }

        /// <summary>
        /// 输出树节点html
        /// </summary>
        /// <param name="parentid"></param>
        /// <returns></returns>
        public static string RenderChildrenNodes(IWebContext context, List<int> modulecodes, string parentid)
        {
            //获取每个子权限的父权限  
            ModuleBLL mbll = ObjectFactory.GetInstance<ModuleBLL>();

            Tree<Module> tree = mbll.GetModuleTree();

            if (context.CurrentUser.UniqueId != string.Empty && Clover.Web.Core.SystemVar.AdminId
              != context.CurrentUser.UniqueId)
            {
                List<Module> nodelist = new List<Module>();

                foreach (int code in modulecodes)
                {
                    Module it = tree.GetById(code.ToString());

                    if (!nodelist.Contains(it))
                        nodelist.Add(it);

                    Module parentit = tree.GetById(it.ParentId);

                    if (!nodelist.Contains(parentit))
                        nodelist.Add(parentit);
                }

                Tree<Module> tree2 = new Tree<Module>(nodelist);
                return ModuleTreeRender.RenderNodeS(tree2, parentid);
            }
            else
            {
                return ModuleTreeRender.RenderNodeS(tree, parentid.ToString());
            }
        }

        public static string MyRenderControl(System.Web.UI.Control control)
        {
            StringBuilder result = new StringBuilder(1024);
            control.RenderControl(new HtmlTextWriter(new StringWriter(result)));
            return result.ToString();
        }

        /// <summary>
        /// 将输入的过滤条件转换成SQL脚本
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string CastTheFilterExpression(string input)
        {

            if (Regex.IsMatch(input, "\\[(\\d)+,(\\d)+\\]"))
            {
                return "{0}" + input.Replace("[", "between ").Replace("]", "").Replace(",", " and ");
            }
            else if (Regex.IsMatch(input, "\\((\\d)+(,)(\\d)+\\]|\\[(\\d)+(,)(\\d)+\\)|\\((\\d)+(,)(\\d)+\\)"))
            {
                return input.Replace("(", "{0}>").Replace("[", "{0}>=").Replace(",", " and ").Replace(")", ">{0}").Replace("]", ">={0}");
            }
            return input;
        }

        #region role
        private static string BuildJsonTree<T>(TreeNode<T> tree, string parentId, bool existsCheckColumn) where T : ISNode
        {
            StringBuilder builder = new StringBuilder();
            List<TreeNode<T>> list = tree.getChildren();
            string str = tree.getNode().Id.ToString();
            builder.Append("{\"id\":" + ("\"" + str + "\"") +
                  ",\"text\":\"" + tree.getNode().Name + "\""
                  + (existsCheckColumn ? (",\"checked\":" + "checked") : string.Empty));

            if (list.Count > 0)
            {
                builder.Append(",\"children\":[");
                foreach (TreeNode<T> row in list)
                {

                    string str2 = BuildJsonTree(row, str, existsCheckColumn);
                    builder.Append(((str2 == string.Empty) ? string.Empty : (str2 + ",")));
                }
                builder.Remove(builder.Length - 1, 1).Append("]");
            }
            builder.Append("},");

            return builder.ToString().Trim(new char[] { ',' });
        }

        public static string ToJsonTree<T>(Tree<T> data) where T : ISNode
        {
            string str = "[";
            if ((data != null) && (data.GetRoot().Count > 0))
            {
                List<TreeNode<T>> list = data.FindRoot();
                foreach (var m in list)
                {
                    str += BuildJsonTree(m, m.getNode().Id, false) + ",";
                }
            }

            str = str.ToString().Trim(new char[] { ',' }) + "]";
            return str;
        }


        public static string ToJsonTree<T>(TreeNode<T> data) where T : ISNode
        {
            string str = "[";
            if (data != null && data.getNode() != null)
            {
                str += BuildJsonTree(data, data.getNode().Id, false) + ",";
            }
            str = str.ToString().Trim(new char[] { ',' }) + "]";
            return str;
        }
        #endregion

        #region 权限
        /// <summary>
        /// 返回禁止禁止访问的权限
        /// </summary>
        /// <param name="funcname"></param>
        /// <param name="url"></param>
        /// <param name="autoToLogin">自动跳转登录界面</param>
        /// <returns></returns>
        public static System.Web.Mvc.ViewResult ForbiddenView(string funcname, string url)
        {
            return ForbiddenView(funcname, url, false);
        }

        /// <summary>
        /// 返回禁止禁止访问的权限
        /// </summary>
        /// <param name="funcname"></param>
        /// <param name="url"></param>
        /// <param name="autoToLogin">自动跳转登录界面</param>
        /// <returns></returns>
        public static System.Web.Mvc.ViewResult ForbiddenView(string funcname, string url, bool autoToLogin)
        {
            System.Web.Mvc.ViewResult view = new System.Web.Mvc.ViewResult();
            var _webcontext = ObjectFactory.GetInstance<IWebContext>();
            view.ViewName = "~/Areas/Admin/Views/Shared/Privilege.aspx";
            view.TempData["ErrorName"] = funcname;
            view.TempData["ReturnUrl"] = url;
            view.TempData["AutoToLogin"] = autoToLogin;
            view.TempData["IsLogin"] = _webcontext.CurrentUser != null;
            if (_webcontext.CurrentUser != null)
                view.TempData["CurrentUser"] = string.Format("{0}({1})",
                    _webcontext.CurrentUser.UserName, _webcontext.CurrentUser.AccountCode);
            else
            {
                view.TempData["CurrentUser"] = "尚未登录";
            }

            return view;
        }

        #endregion

        #region Enum

        /// <summary>
        /// 根据枚举类型返回类型中的所有值，文本及描述
        /// </summary>
        /// <param name="type"></param>
        /// <returns>返回三列数组，第0列为Description,第1列为Value，第2列为Text</returns>
        public static List<string[]> GetEnumOpt(Type type)
        {
            List<string[]> Strs = new List<string[]>();
            FieldInfo[] fields = type.GetFields();
            for (int i = 1, count = fields.Length; i < count; i++)
            {
                string[] strEnum = new string[3];
                FieldInfo field = fields[i];
                //值列
                strEnum[1] = ((int)Enum.Parse(type, field.Name)).ToString();
                //文本列赋值
                strEnum[2] = field.Name;

                object[] objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (objs == null || objs.Length == 0)
                {
                    strEnum[0] = field.Name;
                }
                else
                {
                    DescriptionAttribute da = (DescriptionAttribute)objs[0];
                    strEnum[0] = da.Description;
                }

                Strs.Add(strEnum);
            }
            return Strs;
        }

        // <summary>
        /// 获取枚举类子项描述信息
        /// </summary>
        /// <param name="enumSubitem">枚举类子项</param>        
        public static string GetEnumDescription(object enumSubitem)
        {
            enumSubitem = (Enum)enumSubitem;
            string strValue = enumSubitem.ToString();

            FieldInfo fieldinfo = enumSubitem.GetType().GetField(strValue);

            if (fieldinfo != null)
            {

                Object[] objs = fieldinfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (objs == null || objs.Length == 0)
                {
                    return strValue;
                }
                else
                {
                    DescriptionAttribute da = (DescriptionAttribute)objs[0];
                    return da.Description;
                }
            }
            else
            {
                return "不限";
            }

        }
        #endregion

        #region 金额转化
        /// <summary>
        /// 金额转化
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal TryParseDecimal(object value)
        {
            decimal res = 0;
            return decimal.TryParse(value.ToString(), out res) ? res : 0;
        }
        #endregion

        #region 条形码
        /// <summary>
        /// 39码条形码的字符
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string BarCodeToHTML39(string s)
        {
            Hashtable ht = new Hashtable();

            #region 39码 12位
            ht.Add('A', "110101001011");
            ht.Add('B', "101101001011");
            ht.Add('C', "110110100101");
            ht.Add('D', "101011001011");
            ht.Add('E', "110101100101");
            ht.Add('F', "101101100101");
            ht.Add('G', "101010011011");
            ht.Add('H', "110101001101");
            ht.Add('I', "101101001101");
            ht.Add('J', "101011001101");
            ht.Add('K', "110101010011");
            ht.Add('L', "101101010011");
            ht.Add('M', "110110101001");
            ht.Add('N', "101011010011");
            ht.Add('O', "110101101001");
            ht.Add('P', "101101101001");
            ht.Add('Q', "101010110011");
            ht.Add('R', "110101011001");
            ht.Add('S', "101101011001");
            ht.Add('T', "101011011001");
            ht.Add('U', "110010101011");
            ht.Add('V', "100110101011");
            ht.Add('W', "110011010101");
            ht.Add('X', "100101101011");
            ht.Add('Y', "110010110101");
            ht.Add('Z', "100110110101");
            ht.Add('0', "101001101101");
            ht.Add('1', "110100101011");
            ht.Add('2', "101100101011");
            ht.Add('3', "110110010101");
            ht.Add('4', "101001101011");
            ht.Add('5', "110100110101");
            ht.Add('6', "101100110101");
            ht.Add('7', "101001011011");
            ht.Add('8', "110100101101");
            ht.Add('9', "101100101101");
            ht.Add('+', "100101001001");
            ht.Add('-', "100101011011");
            ht.Add('*', "100101101101");
            ht.Add('/', "100100101001");
            ht.Add('%', "101001001001");
            ht.Add('$', "100100100101");
            ht.Add('.', "110010101101");
            ht.Add(' ', "100110101101");
            #endregion

            #region 39码 9位
            //ht.Add('0', "000110100");
            //ht.Add('1', "100100001");
            //ht.Add('2', "001100001");
            //ht.Add('3', "101100000");
            //ht.Add('4', "000110001");
            //ht.Add('5', "100110000");
            //ht.Add('6', "001110000");
            //ht.Add('7', "000100101");
            //ht.Add('8', "100100100");
            //ht.Add('9', "001100100");
            //ht.Add('A', "100001001");
            //ht.Add('B', "001001001");
            //ht.Add('C', "101001000");
            //ht.Add('D', "000011001");
            //ht.Add('E', "100011000");
            //ht.Add('F', "001011000");
            //ht.Add('G', "000001101");
            //ht.Add('H', "100001100");
            //ht.Add('I', "001001100");
            //ht.Add('J', "000011100");
            //ht.Add('K', "100000011");
            //ht.Add('L', "001000011");
            //ht.Add('M', "101000010");
            //ht.Add('N', "000010011");
            //ht.Add('O', "100010010");
            //ht.Add('P', "001010010");
            //ht.Add('Q', "000000111");
            //ht.Add('R', "100000110");
            //ht.Add('S', "001000110");
            //ht.Add('T', "000010110");
            //ht.Add('U', "110000001");
            //ht.Add('V', "011000001");
            //ht.Add('W', "111000000");
            //ht.Add('X', "010010001");
            //ht.Add('Y', "110010000");
            //ht.Add('Z', "011010000");
            //ht.Add('-', "010000101");
            //ht.Add('.', "110000100");
            //ht.Add(' ', "011000100");
            //ht.Add('*', "010010100");
            //ht.Add('$', "010101000");
            //ht.Add('/', "010100010");
            //ht.Add('+', "010001010");
            //ht.Add('%', "000101010");
            #endregion

            s = "" + s.ToUpper() + "";

            string result_bin = "";//二进制串

            try
            {
                foreach (char ch in s)
                {
                    result_bin += ht[ch].ToString();
                    result_bin += "0";//间隔，与一个单位的线条宽度相等
                }
            }
            catch { return "存在不允许的字符！"; }

            string result_html = "";//HTML代码
            string color = "";//颜色
            foreach (char c in result_bin)
            {
                color = c == '0' ? "#FFFFFF" : "#000000";
                result_html += "<div style=\"width:" + 1 + "px;height:" + 30 + "px;float:left;background:" + color + ";\"></div>";
            }
            result_html += "<div style=\"clear:both\"></div>";

            int len = ht['*'].ToString().Length;
            foreach (char c in s)
            {
                result_html += "<div style=\"width:" + (1 * (len + 1)) + "px;float:left;color:#000000;text-align:center;\">" + c + "</div>";
            }
            result_html += "<div style=\"clear:both\"></div>";

            return "<div style=\"background:#FFFFFF;padding:5px;font-size:" + (1 * 12) + "px;font-family:'楷体';\">" + result_html + "</div>";
        }


        public static string GetBarCodeImgPath(string billNO,uint height)
        {
            var imagepath = System.Web.HttpContext.Current.Server.MapPath("~/upload/CodeImg");
            if (!Directory.Exists(imagepath))
                Directory.CreateDirectory(imagepath);
            imagepath += "\\" + Guid.NewGuid().ToString() + ".gif";

            //Code39 _Code39 = new Code39();
            //_Code39.Height = 60;
            //_Code39.Magnify = 0;
            //_Code39.ViewFont = new Font("宋体", 12);
            //System.Drawing.Image _CodeImage = _Code39.GetCodeImage(billNO, Code39.Code39Model.Code39Normal, true);
            //_CodeImage.Save(imagepath, System.Drawing.Imaging.ImageFormat.Gif);

            //System.Drawing.Image _CodeImage = _Code.GetCodeImage(billNO);//BarCode.GetDrawBarImage(billNO);
            //System.IO.MemoryStream _Stream = new System.IO.MemoryStream();
            //_CodeImage.Save(_Stream, System.Drawing.Imaging.ImageFormat.Jpeg);
            //_CodeImage.Save(imagepath);

            //BarCode.Code128 _Code = new BarCode.Code128();
            //System.Drawing.Bitmap imgTemp = _Code.GetCodeImage(billNO, BarCode.Code128.Encode.Code128A);
            //imgTemp.Save(imagepath, System.Drawing.Imaging.ImageFormat.Gif);131644670299  

            BarCode.Code128 _Code = new BarCode.Code128();
            _Code.ValueFont = new Font("宋体", 15);
            _Code.Height = 80; //height;
            _Code.Magnify =1;
            System.Drawing.Bitmap imgTemp = _Code.GetCodeImage(billNO, BarCode.Code128.Encode.Code128A);
            imgTemp.Save(imagepath, System.Drawing.Imaging.ImageFormat.Gif); 

            return imagepath;
        }

        public static string GetBarCodeImg(string billNO, float fontSize)
        {
            var imagepath = System.Web.HttpContext.Current.Server.MapPath("~/upload/CodeImg");
            if (!Directory.Exists(imagepath))
                Directory.CreateDirectory(imagepath);
            imagepath += "\\" + Guid.NewGuid().ToString() + ".gif";

            BarCode.Code128 _Code = new BarCode.Code128();
            _Code.ValueFont = new Font("宋体", fontSize);
            _Code.Height = 80;
            _Code.Magnify = 1;
            System.Drawing.Bitmap imgTemp = _Code.GetCodeImage(billNO, BarCode.Code128.Encode.Code128A);
            imgTemp.Save(imagepath, System.Drawing.Imaging.ImageFormat.Gif);

            return imagepath;
        }

        /// <summary>
        /// 获取币种简称
        /// </summary>
        /// <param name="currency"></param>
        /// <returns></returns>
        public static string GetCurrency(string currency) {
            //if(currency.Length>3)
            //    currency = currency.Substring(3);
            //switch(currency){
            //    case "人民币":
            //        return "RMB";
            //    case "日元":
            //        return "JPY";
            //    case "美元":
            //        return "USB";
            //    case "欧元":
            //        return "EUR";
            //}
            if (currency.Contains("人民币"))
            {
                return "RMB";
            }
            else if (currency.Contains("日元"))
            {
                return "JPY";
            }
            else if (currency.Contains("美元"))
            {
                return "USB";
            }
            else if (currency.Contains("欧元"))
            {
                return "EUR";
            }
            return string.Empty;
        }
        #endregion
    }
}
