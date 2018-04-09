using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Principal;
using System.Web.Security;

using Clover.Core.Alias;
using Clover.Core.Common;
using Clover.Web.Core;

using StructureMap;

using Clover.Permission.BLL;
using Clover.Permission.Model;
using UkeyTech.WebFW.DAO;
using UkeyTech.OA.WebApp.Extenstion;
namespace UkeyTech.OA.WebApp.Areas.Admin.Controllers
{
    /// <summary>
    /// 功能操作
    /// </summary>
    public partial class PermissionController : AdminLoginController
    {
        private readonly FunctionBLL funcbll = ObjectFactory.GetInstance<FunctionBLL>();


        [CloverAuthorize(ModuleCode = "Function", FuncCode = Clover.Permission.Consts.Browse, FuncName = "浏览功能功能信息")]
        public ActionResult Function(string type)
        {
            switch (type)
            {
                case "FunctionList":
                    return FunctionList();
                case "DeleteFunctions":
                    return DeleteFunctions();
            }

            return View();
        }


        [CloverAuthorize(ModuleCode = "Function", FuncCode = Clover.Permission.Consts.Browse, FuncName = "浏览功能功能信息")]
        public ActionResult FunctionList()
        {
            string codeorname = Utility.GetFormParm("CodeOrName", "");
            string parentid = Utility.GetFormParm("ParentID", "");
            int? Functionid = Utility.GetIntParm("FunctionID", 0);

            int rowscount = 0;

            List<Function> result = funcbll.SelectFunctionList(PageSize, PageIndex, codeorname, out rowscount);

            return Content(Helper.GetListJsonStr<Function>(result, rowscount));
        }

        /// <summary>
        /// javascript 获取模块的访问权限
        /// </summary>
        /// <returns></returns>
        public ActionResult GetModuleFunctionJson(string ModuleCode, bool withdatarule)
        {
            var list = funcbll.GetModuleFunctions(ModuleCode);

            return Content(Helper.GetPermissionJson(ModuleCode, withdatarule, list.Select(m => m.FunctionCode).ToArray()));
        }


        [CloverAuthorize(ModuleCode = "Function", FuncCode = Clover.Permission.Consts.Delete, FuncName = "删除功能信息")]
        public ActionResult DeleteFunctions()
        {
            try
            {
                int[] functionIds = Array.ConvertAll<string, int>(
                    StringHelper.SplitString( Utility.GetFormParm("functionIds",""), ","),
                    int.Parse);

                funcbll.DeleteFunctions(functionIds);

                OPLogDAO.Log(WebContext.CurrentUser, Utility.GetViewerIP(),
                    "删除功能信息操作", Utility.GetFormParm("functionIds", ""));

            }
            catch (Exception ex)
            {
                log.Current.Error("删除功能信息操作失败", ex);
                return Content("");
            }
            return Content("");
        }


        private bool ValidateEdit(Function model)
        {
            if (String.IsNullOrEmpty(model.FunctionCode))
            {
                ModelState.AddModelError("FunctionCode", "请填写功能代码");
            }

            if (String.IsNullOrEmpty(model.FunctionName))
            {
                ModelState.AddModelError("LoginName", "请填写功能名称");
            }

            if (ModelState.IsValid && funcbll.CheckExistsSameID(model.FunctionCode, model.FunctionID.ToString()))
            {
                ModelState.AddModelError("FunctionCode", "功能代码已经存在，请输入另外一个");
            }

            return ModelState.IsValid;
        }
        [HttpGet]
        [CloverAuthorize(ModuleCode = "Function", FuncCode = Clover.Permission.Consts.Create, FuncName = "创建功能")]
        public ActionResult FunctionAdd()
        {
            IsEdit = false;
            var m = new Function();
            LoadPMSystem();
            return View("FunctionEdit", m);
        }

        [HttpPost]
        [UkeyTech.OA.WebApp.Extenstion.CloverAuthorize(ModuleCode = "Function", FuncCode = Clover.Permission.Consts.Create, FuncName = "创建功能")]
        public ActionResult FunctionAdd(string FunctionName, string FunctionCode, string Descn, string FunctionTag, string ModuleID, int ViewOrd, string SystemID, string RelationFunctionID)
        {
            LoadPMSystem();
            var model = new Function();
            model.FunctionName = FunctionName;
            model.FunctionCode = FunctionCode;
            model.FunctionTag = FunctionTag;
            model.ViewOrd = ViewOrd;
            model.Descn = Descn;
            model.RelationFunctionID = RelationFunctionID;
            ViewData["ModuleID"] = ModuleID;
            ViewData["SystemID"] = SystemID;
            TempData.Keep("ModuleID");
            TempData.Keep("SystemID");
            if (ValidateEdit(model))
            {
                try
                {
                    if (FunctionTag == "Special")
                    {
                        funcbll.InsertFunction(model, int.Parse(ModuleID));
                    }
                    else {
                        funcbll.InsertFunction(model);
                    }
                    return RefreshParentAndCloseFrame();

                }
                catch (Exception  ex)
                {
                    ShowError("添加失败");
                    log.Current.Error("功能信息失败", ex);
                }
            }
            return View("FunctionEdit", model);
        }

        [HttpGet]
        [CloverAuthorize(ModuleCode = "Function", FuncCode = Clover.Permission.Consts.Edit, FuncName = "编辑功能")]
        public ActionResult FunctionEdit(int FunctionID)
        {
            LoadPMSystem();
            IsEdit = true;
            Function m = funcbll.Get(FunctionID);

            if (m != null)
            {
                if (m.FunctionTag == "Special")
                {
                    //读取现有的模块关联信息
                    Module spmodule = funcbll.GetFunctionSpModule(FunctionID);
                    if (spmodule != null)
                    {
                        ViewData["ModuleID"] = spmodule.ModuleID;
                        ViewData["SystemID"] = spmodule.SystemID;
                        TempData.Keep("ModuleID");
                        TempData.Keep("SystemID");
                    }
                }
            }
            else {
                m = new Function();
            }

            return View(m);
        }

        [HttpPost]
        [CloverAuthorize(ModuleCode = "Function", FuncCode = Clover.Permission.Consts.Edit, FuncName = "编辑功能")]
        public ActionResult FunctionEdit(string FunctionName, string FunctionCode, string FunctionTag, string Descn, string ModuleID, string SystemID, int ViewOrd, string RelationFunctionID)
        {
            LoadPMSystem();
            int FunctionID = Utility.GetIntParm("FunctionID", 0);

            var model = new Function();
            model.FunctionName = FunctionName;
            model.FunctionCode = FunctionCode;
            model.FunctionTag = FunctionTag;
            model.FunctionID = FunctionID;
            model.ViewOrd = ViewOrd;
            model.Descn = Descn;
            model.RelationFunctionID= RelationFunctionID;
            ViewData["ModuleID"] = ModuleID;
            ViewData["SystemID"] = SystemID;
            TempData.Keep("ModuleID");
            TempData.Keep("SystemID");

            if (FunctionID != 0)
            {

                model.FunctionID = FunctionID;
                if (ValidateEdit(model))
                {
                    try
                    {
                        model = funcbll.Get(model.FunctionID);

                        model.FunctionName = FunctionName;
                        model.FunctionCode = FunctionCode;
                        model.FunctionTag = FunctionTag;
                        model.ViewOrd = ViewOrd;
                        model.Descn = Descn;
                        model.RelationFunctionID = RelationFunctionID;
                        if (model.FunctionTag == "Special")
                            funcbll.UpdateFunction(model, int.Parse(ModuleID));
                        else
                            funcbll.UpdateFunctionWithOutSPModel(model);

                        return RefreshParentAndCloseFrame();

                    }
                    catch (Exception ex)
                    {
                        ShowError("修改失败");
                        log.Current.Error("修改功能信息失败", ex);
                        return Content("");
                    }
                }
            }
            return View(model);
        }
    }
}
