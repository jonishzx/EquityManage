using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Clover.Web.Core;

using StructureMap;

using Clover.Core.Alias;
using Clover.Permission.BLL;
using Clover.Permission.Model;
using UkeyTech.WebFW.DAO;
using UkeyTech.OA.WebApp.Extenstion;
namespace UkeyTech.OA.WebApp.Areas.Admin.Controllers
{
    /// <summary>
    /// 系统信息管理
    /// </summary>
    public partial class PermissionController : AdminLoginController
    {
        private readonly PMSystemBLL pmsbll = ObjectFactory.GetInstance<PMSystemBLL>();


        [CloverAuthorize(ModuleCode = "PMSystem", FuncCode = Clover.Permission.Consts.Browse, FuncName = "浏览功能系统信息")]
        public ActionResult PMSystem(string type)
        {
            switch (type)
            {
                case "GetModuleTree":
                    return GetModuleTree();
                case "PMSystemList":
                    return PMSystemList();
                case "DeletePMSystem":
                    return DeletePMSystems();
            }

            return View();
        }


        [CloverAuthorize(ModuleCode = "PMSystem", FuncCode = Clover.Permission.Consts.Browse, FuncName = "浏览功能系统信息")]
        public ActionResult PMSystemList()
        {          
            string codeorname = Utility.GetFormParm("CodeOrName", "");
            string parentid = Utility.GetFormParm("ParentID", "");
            int? systemid = Utility.GetIntParm("SystemID", 0);

            int rowscount = 0;

            var result = pmsbll.SelectPMSystemList(PageSize, PageIndex, codeorname, out rowscount);

            return Content(Helper.GetListJsonStr<PMSystem>(result, rowscount));
        }


        [CloverAuthorize(ModuleCode = "PMSystem", FuncCode = Clover.Permission.Consts.Delete, FuncName = "删除系统信息")]
        public ActionResult DeletePMSystems()
        {
            int SystemID = Utility.GetFormIntParm("SystemID", 0);

            try
            {
                var m = pmsbll.Get(SystemID);
                pmsbll.DeletePMSystem(SystemID);

                OPLogDAO.Log(WebContext.CurrentUser, Utility.GetViewerIP(),
                    "删除系统信息操作", m);

            }
            catch (Exception ex)
            {
                log.Current.Error("删除系统信息操作失败", ex);
                return Content("");
            }
            return Content("");           
        }


        private bool ValidateEdit(PMSystem model)
        {
            if (String.IsNullOrEmpty(model.SystemCode))
            {
                ModelState.AddModelError("SystemCode", "请填写系统代码");
            }

            if (String.IsNullOrEmpty(model.SystemName))
            {
                ModelState.AddModelError("LoginName", "请填写系统名称");
            }

            if (ModelState.IsValid && pmsbll.CheckExistsSameID(model.SystemCode, model.SystemID.ToString()))
            {
                ModelState.AddModelError("SystemCode", "系统代码已经存在，请输入另外一个");
            }

            return ModelState.IsValid;
        }
        [HttpGet]
        [CloverAuthorize(ModuleCode = "PMSystem", FuncCode = Clover.Permission.Consts.Create, FuncName = "创建系统")]
        public ActionResult PMSystemAdd()
        {
            IsEdit = false;
            var m = new PMSystem();
            return View("PMSystemEdit", m);
        }

        [HttpPost]
        [CloverAuthorize(ModuleCode = "PMSystem", FuncCode = Clover.Permission.Consts.Create, FuncName = "创建系统")]
        public ActionResult PMSystemAdd(string SystemName, string SystemCode, string Descn)
        {
            var model = new PMSystem();
            model.SystemName = SystemName;
            model.SystemCode = SystemCode;
            model.Descn = Descn;

            if (ValidateEdit(model))
            {
                try
                {
                    pmsbll.InsertPMSystem(model);

                    return RefreshParentAndCloseFrame();

                }
                catch (Exception ex)
                {
                    ShowError("添加失败");
                    log.Current.Error("系统信息失败", ex);
                }
            }
            return View("PMSystemEdit", model);
        }

        [HttpGet]
        [CloverAuthorize(ModuleCode = "PMSystem", FuncCode = Clover.Permission.Consts.Edit, FuncName = "编辑系统")]
        public ActionResult PMSystemEdit(int SystemID)
        {
            IsEdit = true;

            return View(pmsbll.Get(SystemID));
        }

        [HttpPost]
        [CloverAuthorize(ModuleCode = "PMSystem", FuncCode = Clover.Permission.Consts.Edit, FuncName = "编辑系统")]
        public ActionResult PMSystemEdit(string SystemName, string SystemCode, string Descn)
        {
            int SystemID = Utility.GetIntParm("SystemID", 0);

            var model = new PMSystem();
            model.SystemName = SystemName;
            model.SystemCode = SystemCode;
        
            model.SystemID = SystemID;      
            model.Descn = Descn;

            if (SystemID != 0)
            {

                model.SystemID = SystemID;
                if (ValidateEdit(model))
                {
                    try
                    {
                        model = pmsbll.Get(model.SystemID);

                        model.SystemName = SystemName;
                        model.SystemCode = SystemCode;
                     
                        model.Descn = Descn;

                        pmsbll.UpdatePMSystem(model);

                        return RefreshParentAndCloseFrame();

                    }
                    catch (Exception ex)
                    {
                        ShowError("修改失败");
                        log.Current.Error("修改系统信息失败", ex);
                        return Content("");
                    }
                }
            }
            return View(model);
        }

    }
}
