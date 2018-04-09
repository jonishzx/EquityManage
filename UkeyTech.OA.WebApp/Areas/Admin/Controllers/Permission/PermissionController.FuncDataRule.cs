using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Principal;
using System.Web.Security;

using Clover.Core.Logging;
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
    /// 数据权限控制器
    /// </summary>
    public partial class PermissionController : AdminLoginController
    {
        FunctionDataRuleBLL funcrulepbll = ObjectFactory.GetInstance<FunctionDataRuleBLL>();

        [CloverAuthorize(ModuleCode = "FunctionDataRule", FuncCode = Clover.Permission.Consts.Browse, FuncName = "浏览功能授权信息")]
        public ActionResult FunctionDataRule()
        {
            return View();
        }

        [CloverAuthorize(ModuleCode = "FunctionDataRule", FuncCode = Clover.Permission.Consts.Browse, FuncName = "浏览功能功能信息")]
        public ActionResult GetAllFuncDataRuleList()
        {
            string codeorname = Utility.GetFormParm("CodeOrName", "");
         
            int rowscount = 0;

            List<FunctionDataRule> result = funcrulepbll.GetAllFunctionDataRule(PageSize, PageIndex, codeorname, out rowscount);

            return Content(Helper.GetListJsonStr<FunctionDataRule>(result, rowscount));
        }


        [CloverAuthorize(ModuleCode = "FunctionDataRule", FuncCode = Clover.Permission.Consts.Browse, FuncName = "获取可用的数据功能信息")]
        public ActionResult GetFuncDataRuleList(int FuncPermissionID)
        {
            List<FunctionDataRule> result = funcrulepbll.GetFunctionDataRule(FuncPermissionID);

            return Content(Helper.GetListJsonStr<FunctionDataRule>(result, result.Count));
        }

        [HttpGet]
        [CloverAuthorize(ModuleCode = "FunctionDataRule", FuncCode = Clover.Permission.Consts.Create, FuncName = "创建功能")]
        public ActionResult FuncDataRuleAdd()
        {
            IsEdit = false;
            var m = new FunctionDataRule();
        
            return View("FunctionDataRuleEdit", m);
        }

        [HttpPost]
        [UkeyTech.OA.WebApp.Extenstion.CloverAuthorize(ModuleCode = "FunctionDataRule", FuncCode = Clover.Permission.Consts.Create, FuncName = "创建数据权限功能")]
        public ActionResult FuncDataRuleAdd(FunctionDataRule model)
        {
            if (ValidateFunctionDataRuleEdit(model))
            {
                try
                {
                    model.Status = 1;
                    funcrulepbll.Insert(model);

                    return RefreshParentAndCloseFrame();                    
                }
                catch (Exception ex)
                {
                    ShowError("添加失败");
                    log.Current.Error("功能信息失败", ex);
                }
            }
            return View("FunctionDataRuleEdit", model);
        }

        private bool ValidateFunctionDataRuleEdit(FunctionDataRule model) {
            if (String.IsNullOrEmpty(model.Code))
            {
                ModelState.AddModelError("Code", "请填写数据权限代码");
            }

            if (String.IsNullOrEmpty(model.Name))
            {
                ModelState.AddModelError("Name", "请填写名称");
            }

            if (ModelState.IsValid && funcbll.CheckExistsSameID(model.Code, model.DataPermissionId.ToString()))
            {
                ModelState.AddModelError("FunctionCode", "数据权限代码已经存在，请输入另外一个");
            }

            return ModelState.IsValid;
        }

        [HttpGet]
        [CloverAuthorize(ModuleCode = "FunctionDataRule", FuncCode = Clover.Permission.Consts.Create, FuncName = "创建功能")]
        public ActionResult FuncDataRuleEdit(string id)
        {
            LoadPMSystem();
            IsEdit = true;
            var m = funcrulepbll.Get(int.Parse(id));
            IsEdit = true;
            if (m == null)
            {
                m = new FunctionDataRule();
            }

            return View("FunctionDataRuleEdit", m);
        }

        [HttpPost]
        [UkeyTech.OA.WebApp.Extenstion.CloverAuthorize(ModuleCode = "FunctionDataRule", FuncCode = Clover.Permission.Consts.Create, FuncName = "修改数据权限功能")]
        public ActionResult FuncDataRuleEdit(string id, FunctionDataRule model)
        {
            if (ValidateFunctionDataRuleEdit(model))
            {
                try
                {
                    var m = funcrulepbll.Get(int.Parse(id));

                    m.Code = model.Code;
                    m.AllowAction = model.AllowAction;
                    m.DenyAction = model.DenyAction;
                    m.DataRule = model.DataRule;
                    m.Name = model.Name;
                    m.Priority = model.Priority;
                    m.Status = 1;
                    funcrulepbll.Update(m);
                    //日志记录
                    OPLogDAO.Log(WebContext.CurrentUser, Utility.GetViewerIP(), " 数据权限修改操作:", model);

                    return RefreshParentAndCloseFrame();
                }
                catch (Exception ex)
                {
                    ShowError("创建失败");
                    LogCentral.Current.Error("数据权限修改失败", ex);
                }
            }
            return View("FunctionDataRuleEdit", model);
        }

        [CloverAuthorize(ModuleCode = "FunctionDataRule", FuncCode = Clover.Permission.Consts.Delete, FuncName = "删除数据权限信息")]
        public ActionResult DeleteFuncDataRule(string id)
        {
            try
            {
                funcrulepbll.Delete(int.Parse(id));

                OPLogDAO.Log(WebContext.CurrentUser, Utility.GetViewerIP(),
                    "删除数据权限操作", Utility.GetFormParm("id", ""));

            }
            catch (Exception ex)
            {
                log.Current.Error("删除功能信息操作失败", ex);
                return Content("");
            }
            return Content("");
        }
    }
}
