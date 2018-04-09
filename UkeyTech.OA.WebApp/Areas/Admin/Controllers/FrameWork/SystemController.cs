using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Mvc;
using Clover.Config;
using Clover.Config.Sys;
using Clover.Config.WebSiteSetting;
using Clover.Core.Common;
using Clover.Core.Alias;
using Clover.Core.Extension;
using Clover.Core.Logging;
using Clover.Permission;
using Clover.Web.Core;
using StructureMap;
using UkeyTech.WebFW.DAO;
using UkeyTech.WebFW.Model;
using UkeyTech.OA.WebApp.Extenstion;
using Clover.Core.Collection;

namespace UkeyTech.OA.WebApp.Areas.Admin.Controllers
{
    /// <summary>
    /// 系统管理controller
    /// </summary>
    public class SystemController : AdminLoginController
    {
        #region 数据库备份

        private readonly DBBackupDAO dbdal = ObjectFactory.GetInstance<DBBackupDAO>();

        [CloverAuthorize(ModuleCode = "DataBaseBackup", FuncCode = Consts.Browse, FuncName = "浏览备份信息")]
        public ActionResult GetBackupList()
        {
            int rowscount = 0;

            //放弃使用数据库记录方式，因为还原数据库后可能会导致数据库的记录丢失，因此需要改变为以文件的方式

            //获取配置中的路径
            ConnectionString conn = SysConfig.GetConnecting("main");

            if (string.IsNullOrEmpty(conn.DBBackupPath))
            {
                ShowError("请设置好main标识的备份文件的路径");
                return View("DataBaseBackup");
            }

            DataTable filedt = dbdal.CreateList(conn.DBBackupPath);


            DataTable pagedt = DataTableHelper.GetPagedTable(filedt, PageIndex, PageSize, "", "UpdateTime desc", out rowscount);

            return Content(Helper.GetDataTableJsonStr(pagedt, rowscount));
        }

        [CloverAuthorize(ModuleCode = "DataBaseBackup", FuncCode = Consts.Browse, FuncName = "浏览备份信息")]
        public ActionResult DataBaseBackup()
        {
            return View();
        }

        [HttpPost]
        [AdminErrorHandle]
        [CloverAuthorize(ModuleCode = "DataBaseBackup", FuncCode = Consts.Edit, FuncName = "备份数据库")]
        public ActionResult DataBaseBackup(DBBackup model)
        {
            model.FileName = !string.IsNullOrEmpty(model.FileName) ? model.FileName.Trim() : DateTime.Now.ToString("yyyy-MM-dd-HH_mm_ss");

            ConnectionString conn = SysConfig.GetConnecting("main");

            model.ServerPath = conn.DBBackupPath + "\\";

            string dbname = conn.GetDBName();

            try
            {
                dbdal.Backup(model, dbname);

                //日志记录
                OPLogDAO.Log(WebContext.CurrentUser, Utility.GetViewerIP(), "数据库备份操作:", dbname);

                ShowError("创建备份成功");
            }
            catch (Exception ex)
            {
                ShowError("创建备份失败");
                LogCentral.Current.Error("管理员创建失败", ex);
                return View(model);
            }

            return View(model);
        }

        [HttpPost]
        [AdminErrorHandle]
        [CloverAuthorize(ModuleCode = "DataBaseBackup", FuncCode = Consts.Edit, FuncName = "备份数据库")]
        public ActionResult DeleteDBBackup(string file)
        {
            try
            {
                System.IO.File.Delete(HttpUtility.UrlDecode(file));

                //日志记录
                OPLogDAO.Log(WebContext.CurrentUser, Utility.GetViewerIP(), "数据库备份删除操作:", file);

                return Success();
            }
            catch (Exception ex)
            {
                LogCentral.Current.Error("数据库备份删除成功失败", ex);
                return Fail("数据库备份删除失败");
            }
        }

        [HttpPost]
        [AdminErrorHandle]
        [CloverAuthorize(ModuleCode = "DataBaseBackup", FuncCode = Consts.Edit, FuncName = "还原备份数据库")]
        public ActionResult ResotreDBBackup(string file)
        {
            //还原数据库
            var masterconn = SysConfig.GetConnecting("master");
            var mainconn = SysConfig.GetConnecting("main");
            try
            {
                dbdal.Restore("master", HttpUtility.UrlDecode(file), mainconn.GetDBName());

                //日志记录
                OPLogDAO.Log(WebContext.CurrentUser, Utility.GetViewerIP(), "数据库还原操作", file);

                return Success();
            }
            catch (Exception ex)
            {
                LogCentral.Current.Error("数据库还原操作失败", ex);
                return Fail("数据库还原操作失败,请刷新重试.");
            }
        }

        #endregion

        #region 日志查看

        private readonly OPLogDAO logdal = ObjectFactory.GetInstance<OPLogDAO>();

        [CloverAuthorize(ModuleCode = "OPLog", FuncCode = Consts.Browse, FuncName = "浏览日志信息")]
        public ActionResult GetLogList(string searchtext, string startdate, string enddate)
        {
            startdate = StringHelper.IsEmpty(startdate) ? DateTime.Now.AddDays(-15).ToString("yyyy-MM-dd") : startdate;
            enddate = StringHelper.IsEmpty(enddate) ? DateTime.Now.ToString("yyyy-MM-dd") : enddate;

            TempData["StartDate"] = startdate;
            TempData["EndDate"] = enddate;

            TempData.Keep("StartDate");
            TempData.Keep("EndDate");

            int rowscount = 0;

            var result = logdal.Query(PageSize, PageIndex, startdate, enddate, searchtext, out rowscount);

            return Content(Helper.GetListJsonStr(result, rowscount));
        }

        [CloverAuthorize(ModuleCode = "OPLog", FuncCode = Consts.Browse, FuncName = "浏览日志信息")]
        public ActionResult OPLog()
        {
            TempData["StartDate"] = DateTime.Now.AddDays(-15).ToString("yyyy-MM-dd");
            TempData["EndDate"] = DateTime.Now.ToString("yyyy-MM-dd");

            TempData.Keep("StartDate");
            TempData.Keep("EndDate");


            return View();
        }

        [CloverAuthorize(ModuleCode = "OPLog", FuncCode = Consts.Browse, FuncName = "浏览日志信息")]
        public ActionResult LogDetail(int? id)
        {
            if (id.HasValue)
            {
                OPLog model = logdal.GetModel(id.Value);
                return PartialView("VCLogDetail", model);
            }
            return PartialView("VCLogDetail");
        }

        [CloverAuthorize(ModuleCode = "OPLog", FuncCode = Consts.Delete, FuncName = "删除日志信息")]
        public ActionResult DelLog(int? id)
        {
            try
            {
                if (id.HasValue)
                {
                    logdal.Delete(id.Value);

                    //日志删除记录吗？
                    OPLogDAO.Log(WebContext.CurrentUser, Utility.GetViewerIP(), "日志信息删除操作:", id);

                    return Success();
                }

                return Fail("参数有误");
            }
            catch (Exception ex)
            {
                LogCentral.Current.Error("数据库备份删除成功失败", ex);
                return Fail("日志删除失败");
            }
        }

        #endregion

        #region 网站信息设置

        [CloverAuthorize(ModuleCode = "WebSiteMgmt", FuncCode = Consts.Browse, FuncName = "网站信息设置")]
        public ActionResult WebSiteMgmt()
        {
            return View(WebSiteConfig.Config);
        }

        [HttpPost]
        [CloverAuthorize(ModuleCode = "WebSiteMgmt", FuncCode = Consts.Edit, FuncName = "网站信息设置")]
        public ActionResult WebSiteMgmt(WebSiteConfigInfo model)
        {
            if (ValidateSaveWebMgmt(model))
            {
                WebSiteConfig.Config.WebAppName = model.WebAppName;
                WebSiteConfig.Config.Weburl = model.Weburl;
                WebSiteConfig.Config.Company = model.Company;
                WebSiteConfig.Config.Supplier = model.Supplier;
                WebSiteConfig.Config.Copyright = model.Copyright;

                //密码策略
                WebSiteConfig.Config.UsePassWordStrategy = model.UsePassWordStrategy;
                WebSiteConfig.Config.PasswordRegex = model.PasswordRegex;
                WebSiteConfig.Config.PasswordNotMatchMessage = model.PasswordNotMatchMessage;
                WebSiteConfig.Config.ChangePasswordPeriod = model.ChangePasswordPeriod;

                OPLogDAO.Log(WebContext.CurrentUser, Utility.GetViewerIP(), "保存网站基本信息操作:", model);

                WebSiteConfig.SaveConfig();

                ShowMessage("操作成功");
            }
            return View(model);
        }

        private bool ValidateSaveWebMgmt(WebSiteConfigInfo model)
        {
            if (String.IsNullOrEmpty(model.WebAppName))
            {
                ModelState.AddModelError("WebAppName", "网站名称");
            }

            if (String.IsNullOrEmpty(model.Weburl))
            {
                ModelState.AddModelError("Weburl", "对外地址");
            }

            if (String.IsNullOrEmpty(model.Company))
            {
                ModelState.AddModelError("Company", "所属公司");
            }

            if (String.IsNullOrEmpty(model.Supplier))
            {
                ModelState.AddModelError("Supplier", "软件供应商");
            }


            if (String.IsNullOrEmpty(model.Copyright))
            {
                ModelState.AddModelError("Copyright", "版权声明");
            }

            return ModelState.IsValid;
        }

        #endregion

        #region 网站访问限制

        [CloverAuthorize(ModuleCode = "WebAccessMgmt", FuncCode = Consts.Browse, FuncName = "系统访问设置")]
        public ActionResult WebAccessMgmt()
        {
            LogCentral.Current.Error("日期变更时，更新网站在线信息时发生系统错误", null);
            return View(WebSiteConfig.Config);
        }

        [HttpPost]
        [CloverAuthorize(ModuleCode = "WebAccessMgmt", FuncCode = Consts.Edit, FuncName = "系统访问设置")]
        public ActionResult WebAccessMgmt(WebSiteConfigInfo model)
        {
            WebSiteConfig.Config.Ipdenyaccess = model.Ipdenyaccess;
            WebSiteConfig.Config.IpdenyaccessTip = model.IpdenyaccessTip;
            WebSiteConfig.Config.IpdenyaccessType = model.IpdenyaccessType;

            WebSiteConfig.Config.Resipaccess = model.Resipaccess;
            WebSiteConfig.Config.ResipaccessTip = model.ResipaccessTip;

            WebSiteConfig.Config.Adminipaccess = model.Adminipaccess;
            WebSiteConfig.Config.AdminipaccessTip = model.AdminipaccessTip;

            WebSiteConfig.Config.Visitbanperiods = model.Visitbanperiods;
            WebSiteConfig.Config.VisitbanperiodsTip = model.VisitbanperiodsTip;


            WebSiteConfig.Config.Closed = model.Closed;
            WebSiteConfig.Config.Closedreason = model.Closedreason;


            OPLogDAO.Log(WebContext.CurrentUser, Utility.GetViewerIP(), "保存网站访问设置信息操作:", model);

            WebSiteConfig.SaveConfig();

            ShowMessage("操作成功");

            return View(model);
        }

        #endregion

        #region 数据字典管理
        #region 字典
        DictionaryDAO Dictionarydal = ObjectFactory.GetInstance<DictionaryDAO>();

        private void LoadParentList(string code)
        {
            //注意不允许选择自己及其子节点作为父节点
            ViewData["Parentlist"] = Dictionarydal.GetList(null, int.MaxValue, string.Format("DictID<>'{0}'", code),"ViewOrder", true);
        }

        //字典信息管理       
       // [CloverAuthorize(ModuleCode = "Dictionary", FuncCode = Consts.Browse, FuncName = "获取字典资料")]
        public ActionResult GetDictionaryTreeWithItems(string parentid)
        {
            Tree<Dictionary> result = Dictionarydal.GetDictTreeWithItems();
            
            if (!string.IsNullOrEmpty(parentid))
            {
                var treenode = result.FindById(parentid);
                return Content(Helper.ToJsonTree(treenode));
            }
            else
                return Content(Helper.ToJsonTree(result));
        }

        //字典信息管理       
        //[CloverAuthorize(ModuleCode = "Dictionary", FuncCode = Consts.Browse, FuncName = "获取字典资料")]
        public ActionResult GetDictionaryTree(string parentid, string NameOrCode)
        {
            Tree<Dictionary> result = Dictionarydal.GetDictTree(string.Format("(Name like '%{0}%' or Code like '%{0}%')" ,NameOrCode));

            if (!string.IsNullOrEmpty(parentid))
            {
                var treenode = result.FindById(parentid);
                return Content(Helper.ToJsonTree(treenode));
            }
            else
                return Content(Helper.ToJsonTree(result));
        }

        private bool ValidateDict(Dictionary model)
        {
            if (String.IsNullOrEmpty(model.DictID))
            {
                ModelState.AddModelError("DictID", "请填写代码");
            }
            if (String.IsNullOrEmpty(model.Name))
            {
                ModelState.AddModelError("Name", "请填写名称");
            }
            if (ModelState.IsValid && Dictionarydal.CheckExistsSameID(model.DictID, model.DictID.ToString()))
            {
                ModelState.AddModelError("DictID", "该代码已经存在，请输入另外一个");
            }
            
            return ModelState.IsValid;
        }

         //字典信息管理添加     
        [CloverAuthorize(ModuleCode = "Dictionary", FuncCode = Consts.Browse, FuncName = "获取字典资料")]
        public ActionResult CreateDict()
        {
            LoadParentList("");
            return View("DictionaryEdit", new Dictionary());
        }

        [HttpPost]
        //字典信息管理添加     
        [CloverAuthorize(ModuleCode = "Dictionary", FuncCode = Consts.Browse, FuncName = "获取字典资料")]
        [ValidateInput(false)]
        public ActionResult CreateDict(Dictionary model)
        {
            LoadParentList("");
          
            if (ValidateDict(model))
            {
                try
                {
                    model.Code = model.DictID;
                    model.UpdateTime = DateTime.Now;
                    Dictionarydal.Insert(model);

                    //日志记录
                    OPLogDAO.Log(WebContext.CurrentUser, Utility.GetViewerIP(), " 添加字典信息操作:", model);

                    return RefreshParentAndCloseFrame();
                }
                catch (Exception ex)
                {
                    ShowError("创建失败");
                    LogCentral.Current.Error("表单创建失败", ex);
                }
            }
           
            return View("DictionaryEdit", model);
        }

        //字典信息管理修改   
        [CloverAuthorize(ModuleCode = "Dictionary", FuncCode = Consts.Browse, FuncName = "获取字典资料")]
        public ActionResult EditDict(string id)
        {
            Dictionary model = null;
            IsEdit = false;
            if (!string.IsNullOrEmpty(id))
            {
                LoadParentList(id);
                IsEdit = true;
                model = Dictionarydal.GetModel(id);            
            }

            return View("DictionaryEdit", model);
        }

        [HttpPost]
        //字典信息管理修改       
        [CloverAuthorize(ModuleCode = "Dictionary", FuncCode = Consts.Browse, FuncName = "获取字典资料")]
        [ValidateInput(false)]
        public ActionResult EditDict(string id, Dictionary model)
        {
            if (ValidateDict(model))
            {
                try
                {
                    var oldmodel = Dictionarydal.GetModel(id);
                    oldmodel.Code = model.Code;
                    oldmodel.Tag = model.Tag;
                    oldmodel.Name = model.Name;
                    oldmodel.ParentId = model.ParentId;
                    oldmodel.SqlCmd = model.SqlCmd;
                    oldmodel.ViewOrder = model.ViewOrder;
                    oldmodel.UpdateTime = DateTime.Now;
                    oldmodel.ExtAttr = model.ExtAttr;
                    Dictionarydal.Update(oldmodel);

                    //日志记录
                    OPLogDAO.Log(WebContext.CurrentUser, Utility.GetViewerIP(), "更新字典信息操作:", model);

                    return RefreshParentAndCloseFrame();
                }
                catch (Exception ex)
                {
                    ShowError("更新失败");
                    LogCentral.Current.Error("字典更新失败", ex);
                }
            }
            LoadParentList(id);
            return View("DictionaryEdit", model);
        }


        [HttpPost]
        //字典信息管理添加     
        [CloverAuthorize(ModuleCode = "Dictionary", FuncCode = Consts.Browse, FuncName = "获取字典资料")]
        public ActionResult DeleteDict()
        {
            string id = Utility.GetFormParm("ID", "");

            try
            {
                var m = Dictionarydal.GetModel(id);
                //Dictionarydal.DeleteDictionary(id);

                OPLogDAO.Log(WebContext.CurrentUser, Utility.GetViewerIP(),
                    "删除字典操作", m);

            }
            catch (Exception ex)
            {
                log.Current.Error("删除字典操作失败", ex);
                return Fail("删除字典操作失败");
            }
            return Success();
        }

        [CloverAuthorize(ModuleCode = "Dictionary", FuncCode = Consts.Browse, FuncName = "员工档案管理")]
        public ActionResult Dictionary()
        {
            return View();
        }
        #endregion

        #region 字典项目
        DictItemDAO dictitemDAO = ObjectFactory.GetInstance<DictItemDAO>();

        private bool ValidateDictItem(DictItem model)
        {
            if (String.IsNullOrEmpty(model.Code))
            {
                ModelState.AddModelError("Code", "请填写代码");
            }
            if (String.IsNullOrEmpty(model.Name))
            {
                ModelState.AddModelError("Name", "请填写代码");
            }
            if (ModelState.IsValid && !IsEdit && dictitemDAO.CheckExistsSameCode(model.DictID, model.Code))
            {
                ModelState.AddModelError("Code", "该代码已经存在，请输入另外一个");
            }
            if (ModelState.IsValid && dictitemDAO.CheckExistsSameName(model.Name, model.DictID, model.Code))
            {
                ModelState.AddModelError("Name", "该名称已经存在，请输入另外一个");
            }

            return ModelState.IsValid;
        }
        
        [CloverAuthorize(ModuleCode = "Dictionary", FuncCode = Consts.Browse, FuncName = "获取字典资料")]
        public ActionResult RefreshDict(string CodeOrName, string ParentID)
        {
            dictitemDAO.ClearCacheItems();
            dictitemDAO.InitDictItems();
            return Content("成功刷新,目前字典项目数为:" + dictitemDAO.GetDictItemsCacheCount().ToString());          
        }
        //字典项目信息管理
        [CloverAuthorize(ModuleCode = "Dictionary", FuncCode = Consts.Browse, FuncName = "获取字典资料")]
        public ActionResult GetDictItemList(string CodeOrName, string ParentID)
        {
            int rowscount = 0;
            
            string[] dictids = sh.SplitString(ParentID, ",");
            string dictidsStr = string.Join("','", dictids);
            string where = sh.IsEmpty(ParentID) ? "" : string.Format("DictID in ('{0}')", dictidsStr);

            if (where.Length > 0 && !sh.IsEmpty(CodeOrName))
            {
                where += string.Format(" AND (Name like '%{0}%' OR Code like '%{0}%')", CodeOrName);
            }
            var result = dictitemDAO.GetAllPaged(PageSize, PageIndex, where, true, out rowscount);

            return Content(Helper.GetListJsonStr(result, rowscount));          
        }

            //字典项目信息管理
        public ActionResult GetALLDictItemList(string CodeOrName, string ParentID)
        {
            string[] dictids = sh.SplitString(ParentID, ",");

            var result = dictitemDAO.GetListByDictIDs(dictids);

            return Content(Helper.GetListJsonStr(result, result.Count));          
        }

        public ActionResult GetALLEmpItemList(string CodeOrName, string ParentID)
        {


            var result = dictitemDAO.GetListByEmpIDs();

            return Content(Helper.GetDataTableJsonStr(result, result.Rows.Count));


        }

        //字典信息管理添加     
        [CloverAuthorize(ModuleCode = "Dictionary", FuncCode = Consts.Browse, FuncName = "获取字典资料")]
        public ActionResult CreateDictItem()
        {
            return View("DictionaryItemEdit", new DictItem());
        }

        [HttpPost]
        //字典信息管理添加     
        [CloverAuthorize(ModuleCode = "Dictionary", FuncCode = Consts.Browse, FuncName = "获取字典资料")]
        public ActionResult CreateDictItem(string dictid, DictItem model)
        {
            model.DictID = dictid;
            if (ValidateDictItem(model))
            {
                try
                {
                   
                    model.UpdateTime = DateTime.Now;
                    dictitemDAO.Insert(model);

                    //日志记录
                    OPLogDAO.Log(WebContext.CurrentUser, Utility.GetViewerIP(), " 添加字典信息操作:", model);

                    return RefreshParentAndCloseFrame();
                }
                catch (Exception ex)
                {
                    ShowError("创建失败");
                    LogCentral.Current.Error("表单创建失败", ex);
                }
            }
            return View("DictionaryItemEdit", model);
        }

        //字典信息管理添加     
        [CloverAuthorize(ModuleCode = "Dictionary", FuncCode = Consts.Browse, FuncName = "获取字典资料")]
        public ActionResult EditDictItem(string dictid, string code)
        {
            DictItem model = null;
            IsEdit = false;
            if (!sh.IsEmpty(dictid) && !sh.IsEmpty(code))
            {
                IsEdit = true;
                model = dictitemDAO.GetModel(dictid, code);
            }

            return View("DictionaryItemEdit", model);
        }

        [HttpPost]
        //字典信息管理添加     
        [CloverAuthorize(ModuleCode = "Dictionary", FuncCode = Consts.Browse, FuncName = "获取字典资料")]
        public ActionResult EditDictItem(string dictid, DictItem model)
        {
            IsEdit = true;
            model.DictID = dictid;
            if (ValidateDictItem(model))
            {
                try
                {
                    var oldmodel = dictitemDAO.GetModel(dictid, Utility.GetParm("code"));
                    oldmodel.Descn = model.Descn;
                    oldmodel.Name = model.Name;
                    oldmodel.Value = model.Value;
                    oldmodel.Status = model.Status;
                    oldmodel.UpdateTime = DateTime.Now;
                    dictitemDAO.Update(oldmodel);

                    //日志记录
                    OPLogDAO.Log(WebContext.CurrentUser, Utility.GetViewerIP(), "更新字典信息操作:", model);

                    return RefreshParentAndCloseFrame();
                }
                catch (Exception ex)
                {
                    ShowError("更新失败");
                    LogCentral.Current.Error("字典更新失败", ex);
                }
            }
            return View("DictionaryItemEdit", model);
        }

        [HttpPost]
        //字典信息管理添加     
        [CloverAuthorize(ModuleCode = "Dictionary", FuncCode = Consts.Browse, FuncName = "获取字典资料")]
        public ActionResult DeleteDictItem(string DictID, string Code)
        {
         
            try
            {
                var m = dictitemDAO.GetModel(DictID, Code);
                dictitemDAO.Delete(DictID, Code);

                OPLogDAO.Log(WebContext.CurrentUser, Utility.GetViewerIP(),
                    "删除字典项目操作", m);

            }
            catch (Exception ex)
            {
                log.Current.Error("删除字典操作失败", ex);
                return Content("");
            }
            return Content("");
        }
        #endregion

        #region 自定义方法
        public ActionResult GetDictSQLDataList(string dictid)
        {
            int rowscount = 0;

            DataTable dt = Dictionarydal.GetSQLDictData(WebContext, dictid, PageSize, PageIndex, out rowscount);

            if (dt == null)
                return Content(Helper.EmptyRows);
            else
                return Content(Helper.GetDataTableJsonStr(dt, rowscount));
        }
        #endregion
        #endregion
    }
}