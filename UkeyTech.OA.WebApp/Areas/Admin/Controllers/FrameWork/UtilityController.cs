using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Web.Mvc;
using Clover.Core.Logging;
using Clover.Core.Alias;
using Clover.Web.Core;
using Clover.Config.FileUpload;
using Clover.Config;
using StructureMap;
using UkeyTech.WebFW.DAO;
using System;
using System.Text;

namespace UkeyTech.OA.WebApp.Areas.Admin.Controllers
{
    public class UtilityController : UkeyTech.OA.WebApp.Extenstion.AdminBaseController
    {
        /// <summary>
        /// 获取当前会话操作员ID
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCurrSessionUserId()
        {
            var webcontext = ObjectFactory.GetInstance<IWebContext>();

            if (webcontext != null && webcontext.CurrentUser != null)
                return Content(webcontext.CurrentUser.UniqueId);
            else
                return Content("");
        }

        /// <summary>
        /// 获取当前会话操作员ID
        /// </summary>
        /// <returns></returns>
        public ActionResult GetUrlContent(string url)
        {
            WebRequest wr = WebRequest.Create(url);
            string result = null;

            wr.Method = "GET";
            wr.Timeout = 10*1000;
            var wrsp = wr.GetResponse();
            Stream sm = wrsp.GetResponseStream();
            if (sm != null)
            using (var stramrd = new StreamReader(sm))
            {
                result = stramrd.ReadToEnd();
            }

            return Content(result);
        }

        /// <summary>
        /// 获取指定控制器
        /// </summary>
        /// <returns></returns>
        public ActionResult GetForm(string url)
        {
            var wr = WebRequest.Create(url);
            string result = null;

            wr.Method = "GET";
            wr.Timeout = 10*1000;
            var wrsp = wr.GetResponse();
            Stream sm = wrsp.GetResponseStream();
            if (sm != null)
            {
                using (var stramrd = new StreamReader(sm))
                {
                    result = stramrd.ReadToEnd();
                }
            }
            return Content(result);
        }

        #region 弹出选择
        private readonly FormColumnDAO columndal = ObjectFactory.GetInstance<FormColumnDAO>();

        private bool GetColumnData() {
            string selecttypeid = Utility.GetParm("TypeId", "");
            string selecttypecode = Utility.GetParm("TypeCode", "");

            //获取数据源的列
            List<SYSDataColumn> colList = null;
            if (!string.IsNullOrEmpty(selecttypeid))
            {
                var dictDao = ObjectFactory.GetInstance<DictionaryDAO>();
                var model = dictDao.GetModel(selecttypeid);
                if (model != null && !string.IsNullOrEmpty(model.ExtAttr))
                {
                    //如果存在扩展属性,优先使用扩展属性的内容
                    ViewData["Columns"] = new List<SYSDataColumn>();
                    ViewData["SelectTypeId"] = selecttypeid;
                    ViewData["ExtendColumns"] = model.ExtAttr;
                    return true;
                }
                colList = columndal.GetPreViewDataColumns(selecttypeid);
            }
            else if (selecttypecode != string.Empty)
                colList = columndal.GetPreViewDataColumnsByCode(selecttypecode);

            if (colList != null && colList.Count > 0)
            {
                ViewData["Columns"] = colList;
                ViewData["SelectTypeId"] = selecttypeid;
                return true;
            }
            return false;
        }
        public ActionResult PopupSelectView()
        {
            var flag = GetColumnData();
            if (flag)
                return View("PopupSelectGrid");
            return Content("无数据");
        }

        /// <summary>
        /// 弹出多选/单选表格
        /// </summary>
        /// <returns></returns>
        public ActionResult PopupMutilSelectGrid()
        {
            var flag = GetColumnData();
            if (flag)
                return View("PopupMutilSelectGrid");
            return Content("无数据");
        }

        /// <summary>
        /// 获取可选择的内容
        /// </summary>
        /// <param name="selecttypeid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetPopupSelectDataList(string selecttypeid, string where, string ModuleCode, string Sort, string FunctionCode)
        {
            int rowscount = 0;
            if (string.IsNullOrEmpty(where))
            {
                where = "";
            }
            string strwhere = "", datarule = "";
            if (!string.IsNullOrEmpty(ModuleCode) && !string.IsNullOrEmpty(FunctionCode))
            {
                datarule = Helper.GetMFDataPermissionRule(ModuleCode, FunctionCode);
            }

            strwhere = Clover.Data.BaseDAO.ParseSQLCommand(WebContext, where, null, Helper.GetDataPermissionRule, datarule);

            DataTable dt = columndal.GetPreViewData(WebContext, selecttypeid, PageSize, PageIndex, strwhere, datarule, Sort, out rowscount);
            return Content(Helper.GetDataTableJsonStr(dt, rowscount));

        }      

        /// <summary>
        /// 弹出多选/单选树表格
        /// </summary>
        /// <returns></returns>
        public ActionResult PopupTreeGrid()
        {
            var flag = GetColumnData();
            if (flag)
                return View("PopupTreeGrid");
            return Content("无数据");

        }
        /// <summary>
        /// 获取可选择的内容(树形式展现)
        /// </summary>
        /// <param name="selecttypeid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetPopupTreeDataList(string selecttypeid, string where, string ModuleCode, string Sort, string FunctionCode, string ID)
        {
            string parentIDField = "_parentId";

            int rowscount = 0;
            if (string.IsNullOrEmpty(where))
            {
                where = "";
            }
            string strwhere = "", datarule = "";
            if (!string.IsNullOrEmpty(ModuleCode) && !string.IsNullOrEmpty(FunctionCode))
            {
                datarule = Helper.GetMFDataPermissionRule(ModuleCode, FunctionCode);
            }

            //动态查询根节点
            if (string.IsNullOrEmpty(where))
                where = " 1=1 ";

            if (string.IsNullOrEmpty(ID))
            {
                where += "AND " + parentIDField + " is null";
            }
            else {
                where += "{?? AND " + parentIDField + " = #ID#}";
            }

            strwhere = Clover.Data.BaseDAO.ParseSQLCommand(WebContext, where, null, Helper.GetDataPermissionRule, datarule);

           
            DataTable dt = columndal.GetPreViewData(WebContext, selecttypeid, int.MaxValue, PageIndex, strwhere, datarule, Sort, out rowscount);
            dt.Columns.Add("state");

            if (dt.Rows.Count == 0)
                return Content("");

            //检查父节子点
            StringBuilder sb = new StringBuilder();
            sb.Append(parentIDField + " in (");
           
            foreach (DataRow r in dt.Rows)
            {
                sb.Append("'" + r["ID"].ToString() + "',");
            } 
            strwhere = sb.ToString().TrimEnd(',') + ")";
            DataTable childdt = columndal.GetPreViewData(WebContext, selecttypeid, int.MaxValue, PageIndex, strwhere, datarule, Sort, out rowscount);
            bool hasChild = false;
            foreach (DataRow r in dt.Rows)
            {
                hasChild = childdt.Select("_parentID = '" + r["ID"].ToString() + "'").Length > 0;
                r["state"] = hasChild ? "closed" : "open";
            }
            childdt.Dispose();

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(dt, new Newtonsoft.Json.Converters.IsoDateTimeConverter()));
             
        }      
        #endregion

        #region 文件上传
        public ActionResult UploadFile()
        {
            string guid = Request.Params["guid"];
            string type = Request.Params["type"];
            string filename = Request.Params["file"];

            string msg = "";

            //删除临时的产品图片
            if (type == "DelTempImages")
            {
                try
                {
                    string delfile = Utility.ConvertPsyPath(filename.Replace(Request.Url.Authority, "")
                        .Replace(Request.Url.Scheme + ":", ""));
                    System.IO.File.Delete(delfile);
                    msg = "success";

                }
                catch (System.Exception ex)
                {
                    log.Current.Error("删除临时图片失败", ex);
                    msg = "删除失败";
                }
            }  //删除文件夹
            else if (type == "DelTempDir")
            {
                try
                {
                    string delfile = Utility.ConvertPsyPath(filename.Replace(Request.Url.Authority, "")
                        .Replace(Request.Url.Scheme + ":", ""));
                    Directory.Delete(delfile, true);
                    msg = "success";

                }
                catch (System.Exception ex)
                {
                    log.Current.Error("删除临时图片失败", ex);
                    msg = "删除失败";
                }
            }
            //普通上传文件
            else if (type == "CommonUpload")
            {

                try
                {
                    FilesUploadInfo cfilecfg = FileUploadConfig.GetFUConfig(type);
                    //Uri uri = new Uri();
                    FileInfo cfinfo = new FileInfo(Request.Files[0].FileName);
                    string targetdir = Server.MapPath(Utility.GetParm("target", ""));
                    int uploadsize = Request.Files[0].ContentLength;

                    if (string.IsNullOrEmpty(targetdir) || !Directory.Exists(targetdir))
                    {
                        msg = "目标路径无效";
                    }
                    else if (uploadsize > cfilecfg.MaxFileSize * 1024) //大小判断
                    {
                        msg = "最大文件大小只允许:" + cfilecfg.MaxFileSize + "kb";
                    }
                    else if (!cfilecfg.ExtAllowedList.Contains(cfinfo.Extension.Replace(".", "").ToLower()))
                    {
                        msg = "只允许上传以下文件格式:" + cfilecfg.ExtAllowed;
                    }
                    else
                    {
                        //保存文件
                        string filepath = Path.Combine(targetdir, cfinfo.Name);
                        Request.Files[0].SaveAs(filepath);

                    }//end else

                    msg = "success";

                }
                catch (System.Exception ex)
                {
                    log.Current.Error("删除临时图片失败", ex);
                    msg = "删除失败";
                }
            }
            else
            {
                FilesUploadInfo filecfg = FileUploadConfig.GetFUConfig(type);

                FileInfo finfo = new FileInfo(Request.Files[0].FileName);

                int uploadsize = Request.Files[0].ContentLength;
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
                    msg = UploadFile(filecfg, guid, finfo.Name);
                }//end else
            }

           return Content(msg);
        }

        /// 文件上传操作
        /// </summary>
        /// <param name="filecfg"></param>
        /// <param name="guid"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UploadFile(FilesUploadInfo filecfg, string guid, string filename)
        {
            string msg = "";

            try
            {
                if (string.IsNullOrEmpty(guid))
                {
                    msg = "参数无效";
                }
                else
                {
                    string rootpath = Path.Combine(Server.MapPath(filecfg.TempPath), guid);
                    if (!Directory.Exists(rootpath))
                    {
                        Directory.CreateDirectory(rootpath);
                    }

                    string filepath = Path.Combine(rootpath, filename);

                    Request.Files[0].SaveAs(filepath);
                }

                string imgurl = Utility.ConvertAbsoulteUrl(filecfg.TempPath + "/" + guid + "/" + filename);
                msg = "success##" + imgurl;

            }
            catch (System.Exception ex)
            {                
                log.Current.Error("文件上传失败", ex);
                msg = "文件上传失败,请联系管理员";
            }

            return msg;

        }
        #endregion

        #region 文件下载
        public ActionResult Download(string psyFilePath, string outputFileName)
        {

            FileInfo fino = new FileInfo(psyFilePath);
            if (fino.Exists)
            {
                var contentType = Utility.ConvertFileType(fino.Extension);

                return File(psyFilePath, contentType, outputFileName + fino.Extension);
            }
            else
            {
                return Content("文件地址已失效");
            }
        }
        public ActionResult Download(string psyFilePath)
        {
            return Download(psyFilePath, "数据");
        }


        #endregion
    }
}