using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Web.Mvc;
using Clover.Core.Common;
using Clover.Core.Logging;
using Clover.Core.Alias;
using Clover.Permission;
using Clover.Web.Core;
using Clover.Config.FileUpload;
using Clover.Config;
using StructureMap;
using UkeyTech.WebFW.Model;
using UkeyTech.WebFW.DAO;
using UkeyTech.OA.WebApp.Extenstion;
namespace UkeyTech.OA.WebApp.Areas.Admin.Controllers
{
    public class AttachmentController : UkeyTech.OA.WebApp.Extenstion.AdminBaseController
    {
        AttachmentDAO _attachmentdal = ObjectFactory.GetInstance<AttachmentDAO>();
        const string COMMONUPLOADTAG = "CommonUpload";

        //默认视图
        [CloverAuthorize(ModuleCode = "Attachment", FuncCode = Consts.Browse, FuncName = "")]
        public ActionResult Attachment()
        {
            return View();
        }

        #region 附件列表输出
        // json 列表输出
        [CloverAuthorize(ModuleCode = "EmpFile", FuncCode = Consts.Browse, FuncName = "获取员工相关文件资料")]
        [HttpPost]
        public ActionResult GetEmpAttachmentList()
        {
            string where = "1=1 AND TargetType = 'emp' {?? AND TargetID = '$TargetID$'}";
            return RenderListContent(where);
        }

        // json 列表输出
        [CloverAuthorize(ModuleCode = "Message", FuncCode = Consts.Browse, FuncName = "获取用户消息的文件资料")]
        [HttpPost]
        public ActionResult GetMessageList()
        {
            string where = "1=1 AND {?? TargetType = '$TargetType$'} {?? AND TargetID = '$TargetID$'}";
            return RenderListContent(where);
        }

        // json 列表输出
        [CloverAuthorize(ModuleCode = "EmpLeave", FuncCode = Consts.Browse, FuncName = "获取短期投资流程文件资料")]
        [HttpPost]
        public ActionResult GetEmpEmpLeaveList()
        {
            string where = "1=1 AND {?? TargetType = '$TargetType$'} {?? AND TargetID = '$TargetID$'}";
            return RenderListContent(where);
        }

        //[CloverAuthorize(ModuleCode = "Attachment", FuncCode = Consts.Browse, FuncName = "获取所有附件资料")]
        [HttpPost]
        public ActionResult GetAttachmentList()
        {
            string where = "1=1 {?AND TargetType = #TargetType#} {? AND TargetID = #TargetID#}";
            return RenderListContent(where);
        }
        // json 列表输出
        [CloverAuthorize(ModuleCode = "ShortTermCaptial", FuncCode = Consts.Browse, FuncName = "获取短期投资流程文件资料")]
        [HttpPost]
        public ActionResult GetFincLoanBusiList()
        {
            string where = "1=1 AND {?? TargetType = '$TargetType$'} {?? AND TargetID = '$TargetID$'}";
            return RenderListContent(where);
        }

        // json 列表输出
        [CloverAuthorize(ModuleCode = "BankAccount", FuncCode = Consts.Browse, FuncName = "获取资金调拨附件信息")]
        public ActionResult GetFincFundTransfeList()
        {
            string where = "1=1 AND {?? TargetType = '$TargetType$'} {?? AND TargetID = '$TargetID$'}";
            return RenderListContent(where);
        }

        // json 列表输出
        [CloverAuthorize(ModuleCode = "Project", FuncCode = Consts.Browse, FuncName = "获取项目跟踪日志附件信息")]
        [HttpPost]
        public ActionResult GetProjectLogAttachmentList()
        {
            string where = "1=1 AND {?? TargetType = '$TargetType$'} {?? AND TargetID = '$TargetID$'}";
            return RenderListContent(where);
        }

        // json 列表输出 业务操作附件信息 还款
        [CloverAuthorize(ModuleCode = "LoanMgmtFp", FuncCode = Consts.Browse, FuncName = "获取业务操作附件信息")]
        [HttpPost]
        public ActionResult RepayList()
        {
            string where = "1=1 AND {?? TargetType = '$TargetType$'} {?? AND TargetID = '$TargetID$'}";
            return RenderListContent(where);
        }

        // json 列表输出 业务操作附件信息 收费
        [CloverAuthorize(ModuleCode = "LoanMgmtFp", FuncCode = Consts.Browse, FuncName = "获取业务操作附件信息")]
        [HttpPost]
        public ActionResult RepayRateMgList()
        {
            string where = "1=1 AND {?? TargetType = '$TargetType$'} {?? AND TargetID = '$TargetID$'}";
            return RenderListContent(where);
        }

        // json 列表输出 售结汇附件信息
        [CloverAuthorize(ModuleCode = "FundExchange", FuncCode = Consts.Browse, FuncName = "获取业售结汇附件信息")]
        [HttpPost]
        public ActionResult GetFundExchangeList()
        {
            string where = "1=1 AND {?? TargetType = '$TargetType$'} {?? AND TargetID = '$TargetID$'}";
            return RenderListContent(where);
        }

        #endregion

        #region 附件相关操作方法
        [HttpPost]
        private ContentResult RenderListContent(string where)
        {
            int rowscount = 0;
            var result = _attachmentdal.GetAllPaged(PageSize, PageIndex,
                  Clover.Data.BaseDAO.ParseSQLCommand(WebContext, where, Helper.GetDataPermissionRule), true, out rowscount);

            return Content(Helper.GetListJsonStr(result, rowscount));
        }

        //帮助文档Help  
        public ActionResult HelpAttachments()
        {
            return View("HelpAttachments");
        }

        //验证方法
        private bool ValidateAttachment(Attachment model)
        {

            if (String.IsNullOrEmpty(model.FilePath))
            {
                ModelState.AddModelError("FilePath", "请填写文件路径 ");
            }
            if (String.IsNullOrEmpty(model.TargetType))
            {
                ModelState.AddModelError("TargetType", "无效的附件类型 ");
            }
            if (String.IsNullOrEmpty(model.TargetID))
            {
                ModelState.AddModelError("TargetID", "无效的附件对象 ");
            }
            /*
            if (ModelState.IsValid &&  _attachmentdal.CheckExistsSameID(model.AttachmentID, model.DictID.ToString()))
            {
                ModelState.AddModelError("AttachmentID", "该代码已经存在，请输入另外一个");
            }
            */
            return ModelState.IsValid;
        }

        //初始化数据
        private void LoadInitDataForAttachment(Attachment model)
        {
            if (!IsEdit)
            {
                model.CreatorName = WebContext.CurrentUser.UserName;
                model.Creator = WebContext.CurrentUser.UniqueId;
                model.UpdateTime = DateTime.Now;
                model.TargetType = Utility.GetParm("TargetType", "");
                model.TargetID = Utility.GetParm("TargetID", "");
            }
        }

        //信息添加     
        //[CloverAuthorize(ModuleCode = "Attachment", FuncCode = Consts.Create, FuncName = "新增资料")]
        public ActionResult CreateAttachment()
        {
            var model = new Attachment();
            LoadInitDataForAttachment(model);
            return View("AttachmentEdit", model);
        }

        /// <summary>
        /// 多文件上传窗体
        /// </summary>
        /// <returns></returns>
        public ActionResult MutilFileUpload()
        {
            return View();
        }
        /// <summary>
        /// 多文件上传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        //[CloverAuthorize(ModuleCode = "Attachment", FuncCode = Consts.Create, FuncName = "新增资料")]
        public ActionResult QuickAttachmentUpload(int? chunk, int? chunks, string name, string TargetID, string TargetType)
        {
            var files = Request.Files;
            var id = Guid.NewGuid();
            var msg = string.Empty;
            chunk = chunk ?? 0;

            var output = GetTempUploadPath("~/upload/" + TargetType + "/" + TargetID, name);
            string uploadedFilePath = output[0];
            string outputurl = output[1];
            var fileUpload = Request.Files[0];
            FileInfo fino = new FileInfo(uploadedFilePath);
            if (!fino.Directory.Exists)
                fino.Directory.Create();
            using (var fs = new FileStream(uploadedFilePath, chunk == 0 ? FileMode.Create : FileMode.Append))
            {
                var buffer = new byte[fileUpload.InputStream.Length];
                fileUpload.InputStream.Read(buffer, 0, buffer.Length);
                fs.Write(buffer, 0, buffer.Length);
            }
            if (chunk < (chunks - 1))
            {
                //分块,未上传完
                return Content("Success", "text/plain");
            }
            else if ((chunks - 1) == chunk)
            {
                //全部块上传完成
                try
                {
                    FileInfo fileinfo = new FileInfo(uploadedFilePath);
                    string oldname = fileinfo.Name;
                    string newfilwname = fileinfo.Name.Replace(fileinfo.Extension, "_") + Guid.NewGuid().ToString() + fileinfo.Extension;
                    string newfilepath = uploadedFilePath.Replace(oldname, newfilwname);
                    fileinfo.MoveTo(newfilepath);
                    fileinfo = new FileInfo(newfilepath);
                    var model = new Attachment()
                    {
                        Title = name.Replace(fileinfo.Extension, ""),
                        FilePath = outputurl.Replace(oldname, newfilwname),
                        Bytes = fileinfo.Length,
                        FileName = fileinfo.Name,
                        TargetID = TargetID,
                        TargetType = TargetType,
                        Creator = WebContext.CurrentUser.UniqueId,
                        UpdateTime = DateTime.Now
                    };

                    //临时文件转换为真实文件
                    tempToReal(model);

                    _attachmentdal.Insert(model);

                    //日志记录
                    OPLogDAO.Log(WebContext.CurrentUser, Utility.GetViewerIP(), " 添加信息操作:", model);

                    // Return Success JSON-RPC response
                    return Content("{\"jsonrpc\" : \"2.0\",\"result\" : null, \"id\" : \"id\"}");
                }
                catch (Exception ex)
                {
                    LogCentral.Current.Error("创建失败", ex);
                    return Content("{\"jsonrpc\" : \"2.0\", \"error\" : {\"code\": 101, \"message\": \"" + ex.Message + ".\"}, \"id\" : \"id\"}");
                }
            }
            return new EmptyResult();
        }

        //信息添加(post)    
        [HttpPost]
        //[CloverAuthorize(ModuleCode = "Attachment", FuncCode = Consts.Create, FuncName = "新增资料")]
        public ActionResult CreateAttachment(Attachment model)
        {
            LoadInitDataForAttachment(model);

            if (ValidateAttachment(model))
            {
                try
                {
                    //临时文件转换为真实文件
                    tempToReal(model);

                    _attachmentdal.Insert(model);

                    //日志记录
                    OPLogDAO.Log(WebContext.CurrentUser, Utility.GetViewerIP(), " 添加信息操作:", model);

                    return RefreshParentAndCloseFrame();
                }
                catch (Exception ex)
                {
                    ShowError("创建失败," + ex.Message);
                    LogCentral.Current.Error("创建失败", ex);
                }
            }
            LoadInitDataForAttachment(model);
            return View("AttachmentEdit", model);
        }

        /// <summary>
        /// 将指定文件移动临时文件夹
        /// </summary>
        /// <param name="model"></param>
        private void tempToReal(Attachment model)
        {
            var tmpfilepath = model.FilePath;
            var fino = new System.IO.FileInfo(Server.MapPath(tmpfilepath));
            if (!fino.Exists)
                throw new FileNotFoundException("上传文件" + tmpfilepath + "无法获取，请重新上传");

            var oldfilename = fino.Name;
            var newfilename = fino.Name.Replace("temp_", "");
            var newfullfilepath = fino.FullName.Replace(fino.Name, newfilename);

            if (System.IO.File.Exists(newfullfilepath))
            {
                System.IO.File.Delete(newfullfilepath);
            }
            fino.MoveTo(fino.FullName.Replace(fino.Name, newfilename));
            model.Bytes = fino.Length;
            model.FileName = fino.Name;
            model.FilePath = model.FilePath.Replace(oldfilename, newfilename);
        }

        //信息修改 
        //[CloverAuthorize(ModuleCode = "Attachment", FuncCode = Consts.Edit, FuncName = "修改附件信息资料")]
        public ActionResult EditAttachment(string id)
        {
            Attachment model = null;
            IsEdit = false;
            if (!string.IsNullOrEmpty(id))
            {
                IsEdit = true;
                model = _attachmentdal.GetModel(int.Parse(id));
            }
            LoadInitDataForAttachment(model);
            return View("AttachmentEdit", model);
        }

        //信息修改(post)    
        [HttpPost]
        //[CloverAuthorize(ModuleCode = "Attachment", FuncCode = Consts.Edit, FuncName = "新增附件信息资料")]
        public ActionResult EditAttachment(string id, Attachment model)
        {
            if (ValidateAttachment(model))
            {
                try
                {
                    var m = _attachmentdal.GetModel(int.Parse(id));
                    m.Title = model.Title;
                    if (m.FilePath != model.FilePath)
                    {
                        tempToReal(model);

                        m.FilePath = model.FilePath;
                        m.FileName = model.FileName;
                        m.Bytes = model.Bytes;
                    }
                    m.Descn = model.Descn;
                    m.ViewOrder = model.ViewOrder;
                    m.Status = model.Status;
                    m.UpdateTime = DateTime.Now;

                    _attachmentdal.Update(m);

                    //日志记录
                    OPLogDAO.Log(WebContext.CurrentUser, Utility.GetViewerIP(), " 更新附件信息操作:", model);

                    return RefreshParentAndCloseFrame();
                }
                catch (Exception ex)
                {
                    ShowError("更新失败");
                    LogCentral.Current.Error("更新失败", ex);
                }
            }

            LoadInitDataForAttachment(model);
            return View("AttachmentEdit", model);

        }

        //信息删除(post)    
        [HttpPost]
        //[CloverAuthorize(ModuleCode = "Attachment", FuncCode = Consts.Delete, FuncName = "删除资料")]
        public ActionResult DeleteAttachment()
        {
            var delids = Utility.GetFormParm("delids", "");

            string[] ids = StringHelper.SplitString(delids, ",");

            if (ids != null && ids.Length > 0)
            {
                foreach (string id in ids)
                {
                    try
                    {
                        int iid = int.Parse(id);
                        var m = _attachmentdal.GetModel(iid);

                        //附件删除判定
                        //if (m.Creator.CompareTo(WebContext.CurrentUser.UniqueId) != 0 &&
                        //    WebContext.CurrentUser.UniqueId.CompareTo(SystemVar.AdminId) != 0) {
                        //        return Fail("该附件不是你所创建,无法删除，请联系管理员");
                        //}

                        _attachmentdal.Delete(iid);

                        if (m != null && !string.IsNullOrEmpty(m.FilePath))
                        {
                            var filepath = Server.MapPath(m.FilePath);
                            if (System.IO.File.Exists(filepath))
                                System.IO.File.Delete(filepath);

                            OPLogDAO.Log(WebContext.CurrentUser, Utility.GetViewerIP(),
                           "删除操作", m);
                        }

                    }
                    catch (Exception ex)
                    {
                        log.Current.Error("删除操作失败", ex);
                        return Fail("删除操作失败");
                    }
                }
            }


            return Content("");
        }

        [HttpPost]
        //[CloverAuthorize(ModuleCode = "Attachment", FuncCode = Consts.Delete, FuncName = "删除资料")]
        public ActionResult RefAttachment(string sourceIds, string targetId)
        {
            try
            {
                _attachmentdal.RefAttachment(WebContext.CurrentUser.UniqueId, sourceIds, targetId);
                return Success();
            }
            catch (Exception ex)
            {
                return Fail("引用附件失败：" + ex.Message);
            }
        }


        ///// <summary>
        ///// 引用附件到附件列表
        ///// </summary>
        ///// <param name="sourceIds">来源附件主键</param>
        ///// <param name="targetId">目前业务主键</param>
        ///// <returns></returns>
        //[HttpPost]
        ////[CloverAuthorize(ModuleCode = "Attachment", FuncCode = Consts.Delete, FuncName = "删除资料")]
        //public ActionResult AllAttachmentList(string sourceIds, string targetId)
        //{
        //    return RenderListContent("");
        //}

        public ActionResult DownloadAttachment(int id)
        {
            var m = _attachmentdal.GetModel(id);
            if (m == null)
                return Content("无效的文件请求");

            //Utility.DownloadFile(System.Web.HttpContext.Current.Request
            //    , System.Web.HttpContext.Current.Response, m.FilePath, m.FileName, 100);
            var finfo = new FileInfo(Server.MapPath(m.FilePath));
            if (finfo.Exists)
            {
                return File(new FileStream(finfo.FullName, FileMode.Open), Utility.ConvertFileType(finfo.Extension), m.Title + finfo.Extension);
            }
            else
            {
                return new EmptyResult();
            }
        }

        /// <summary>
        /// 预览附件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult PreviewAttachment(int id)
        {
            var m = _attachmentdal.GetModel(id);

            //Utility.DownloadFile(System.Web.HttpContext.Current.Request
            //    , System.Web.HttpContext.Current.Response, m.FilePath, m.FileName, 100);
            var finfo = new FileInfo(Server.MapPath(m.FilePath));
            if (finfo.Exists)
            {
                return File(new FileStream(finfo.FullName, FileMode.Open), Utility.ConvertFileType(finfo.Extension), m.Title + finfo.Extension);
            }
            else
            {
                return new EmptyResult();
            }
        }

        #region 文件上传
        public ActionResult UploadFile()
        {
            string guid = Request.Params["guid"];
            string type = Request.Params["type"];
            string cfgtype = !string.IsNullOrEmpty(Request.Params["cfgtype"]) ? Request["cfgtype"] : COMMONUPLOADTAG;
            string filename = Request.Params["file"];
            string target = Utility.GetParm("target", "");
            string msg = "";

            SaveFileToServer(guid, type, target, filename, cfgtype, out msg);

            return Content(msg);
        }

        private string makeNewFileName(FileInfo cfinfo)
        {
            return makeNewFileName(cfinfo, null);
        }

        private string makeNewFileName(FileInfo cfinfo, string filename)
        {
            if (!string.IsNullOrEmpty(filename))
                return filename.Replace(cfinfo.Extension, "_").Replace(" ", "_") + Guid.NewGuid().ToString() + cfinfo.Extension;
            else
                return cfinfo.Name.Replace(cfinfo.Extension, "_").Replace(" ", "_") + Guid.NewGuid().ToString() + cfinfo.Extension;
        }

        private OutputFileInfo SaveFileToServer(string guid, string type, string target, string filename, string cfgtype, out string msg)
        {
            msg = string.Empty;
            OutputFileInfo rst = null;

            if (Request.Files.Count == 0)
            {
                log.Current.Error("上传的文件爱无效");
                msg = "上传失败";
                return null;
            }

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
            else if (type == COMMONUPLOADTAG)
            {

                try
                {
                    FilesUploadInfo cfilecfg = FileUploadConfig.GetFUConfig(type);
                    //Uri uri = new Uri();
                    FileInfo cfinfo = new FileInfo(Request.Files[0].FileName);
                    string targetdir = Utility.ConvertPsyPath(target);
                    int uploadsize = Request.Files[0].ContentLength;
                    string newfilename = makeNewFileName(cfinfo);
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
                        string filepath = Path.Combine(targetdir, newfilename);
                        Request.Files[0].SaveAs(filepath);

                    }//end else

                    msg = "success";

                }
                catch (System.Exception ex)
                {
                    log.Current.Error("上传失败", ex);
                    msg = "上传失败";
                }
            }
            //模块上传文件
            else if (type == "ModuleFileUpload")
            {


                try
                {
                    FilesUploadInfo cfilecfg = FileUploadConfig.GetFUConfig(cfgtype);
                    //Uri uri = new Uri();
                    FileInfo cfinfo = new FileInfo(Request.Files[0].FileName);
                    string targetdir = Server.MapPath(target);
                    int uploadsize = Request.Files[0].ContentLength;

                    if (!Directory.Exists(targetdir))
                        Directory.CreateDirectory(targetdir);

                    if (uploadsize > cfilecfg.MaxFileSize * 1024) //大小判断
                    {
                        msg = "最大文件大小只允许:" + cfilecfg.MaxFileSize + "KB";
                    }
                    else if (!cfilecfg.ExtAllowedList.Contains(cfinfo.Extension.Replace(".", "").ToLower()))
                    {
                        msg = "只允许上传以下文件格式:" + cfilecfg.ExtAllowed;
                    }
                    else
                    {

                        //保存文件
                        FileInfo fino = new FileInfo(Request.Files[0].FileName);
                        filename = !string.IsNullOrEmpty(filename) ? filename : fino.Name;
                        string newMfilename = makeNewFileName(fino, filename);

                        var output = GetTempUploadPath(target, newMfilename);
                        string filepath = output[0];
                        string outputurl = output[1];
                        //log.Current.Info("保存文件到" + filepath + "(" + newMfilename + ")");
                        Request.Files[0].SaveAs(filepath);
                        cfinfo = new System.IO.FileInfo(filepath);
                        var outputmsg = GetOutputFileInfo(outputurl, filename, cfinfo);
                        msg = "success##" + Newtonsoft.Json.JsonConvert.SerializeObject(outputmsg);
                        rst = outputmsg;
                    }//end else
                }
                catch (System.Exception ex)
                {
                    log.Current.Error("删除临时图片失败", ex);
                    msg = "上传文件失败";
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
            return rst;
        }

        /// <summary>
        /// 获取上传文件位置数组
        /// </summary>
        /// <param name="target"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private string[] GetTempUploadPath(string target, string name)
        {
            string tempfilename = "temp_" + name;
            string oldfilename = name;
            string filepath = Path.Combine(Server.MapPath(target), tempfilename);
            string outputurl = target + "/" + tempfilename;

            return new[] { filepath, outputurl };
        }

        //获取上传文件的信息对象
        private OutputFileInfo GetOutputFileInfo(string url, string filename, FileInfo fino)
        {
            return new OutputFileInfo
            {
                url = url,
                filename = filename,
                ext = fino.Extension,
                size = fino.Length
            };
        }

        /// <summary>
        /// 输出的文件信息
        /// </summary>
        internal class OutputFileInfo
        {
            public string url;
            public string filename;
            public string ext;
            public long size;
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
                    FileInfo cfinfo = new FileInfo(filename);
                    string newfilename = makeNewFileName(cfinfo);

                    string filepath = Path.Combine(rootpath, newfilename);

                    log.Current.Info("保存文件到" + filepath + "(" + rootpath + "," + newfilename + ")");

                    Request.Files[0].SaveAs(filepath);

                    FileInfo fino = new FileInfo(filepath);
                    string imgurl = Utility.ConvertAbsoulteUrl(filecfg.TempPath + "/" + guid + "/" + newfilename);
                    msg = "success##" + Newtonsoft.Json.JsonConvert.SerializeObject(GetOutputFileInfo(imgurl, newfilename, fino));
                }
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

        #endregion


        #endregion
    }
}