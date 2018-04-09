using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

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
    /// 管理员账号相关控制
    /// </summary>
    public partial class PermissionController : AdminLoginController
    {
        private readonly ModuleBLL moddal = ObjectFactory.GetInstance<ModuleBLL>();

      
        [CloverAuthorize(ModuleCode="Module", FuncCode = Clover.Permission.Consts.Browse, FuncName = "浏览功能模块")]
        public ActionResult Module(string type)
        {
            var systembll = StructureMap.ObjectFactory.GetInstance<Clover.Permission.BLL.PMSystemBLL>();
            var sysmlist = systembll.SelectPMSystemList("");

            ViewData["PMSystemList"] = sysmlist;

            switch (type) {
                case "GetAllSystemModuleTree":
                    return GetAllSystemModuleTree();
                case "GetModuleTree":
                    return GetModuleTree();
                case "ModuleList":
                    return ModuleList();
                case "DeleteModule":
                    return DeleteModule();
                case "ModuleFunctionList":
                    return ModuleFunctionList();
                case "DeleteModuleFunction":
                    return DeleteModuleFunction();
                case "AddModuleFunction":
                    return AddModuleFunction();
                case "NotJoinFunctionByModule":
                    return ModuleNoFunctionRef();   
            }
                
            return View();
        }

        [CloverAuthorize(ModuleCode = "Module", FuncCode = Clover.Permission.Consts.Browse, FuncName = "浏览功能模块")]
        public ActionResult GetAllSystemModuleTree()
        {
            try
            {
                var syslist = ViewData["PMSystemList"] as List<PMSystem>;
                var sb = new System.Text.StringBuilder();
                if (syslist == null)
                    return Fail("无效系统列表");

                foreach (var m in syslist)
                {
                    sb.Append(Helper.RenderChildrenNodes(m.SystemID.ToString()));
                }
               
                return Content(sb.ToString());
            }
            catch (Exception ex)
            {
                log.Current.Error("读取模块信息失败", ex);
                return Content("");
            }
        }

        [CloverAuthorize(ModuleCode="Module", FuncCode = Clover.Permission.Consts.Browse, FuncName = "浏览功能模块")]
        public ActionResult GetModuleTree()
        {
            try
            {
                int SystemID = Utility.GetIntParm("SystemID", 0);

                int myModuleId = Utility.GetIntParm("mid", 0);

                var mlist = moddal.GetSystemModules(SystemID);

                //删除自身
                mlist.RemoveAll(x => x.ModuleID == myModuleId);

                var tree = new Clover.Core.Collection.Tree<Module>(mlist);

                string output = Helper.ToJsonTree<Module>(tree);
                return Content(output);
            }
            catch (Exception ex)
            {                
                log.Current.Error("读取模块信息失败", ex);
                return Content("");
            }
        }

        [CloverAuthorize(ModuleCode = "Module", FuncCode = Clover.Permission.Consts.Browse, FuncName = "浏览功能模块")]
        public ActionResult ModuleList()
        {
            string codeorname = Utility.GetFormParm("CodeOrName", "");
            string parentid = Utility.GetFormParm("ParentID", "");
            int? systemid = Utility.GetIntParm("SystemID", 0);
            
            int rowscount = 0;

            var result = moddal.SelectModuleList(PageSize, PageIndex, (systemid == 0 ? null : systemid), codeorname, parentid, out rowscount);

            return Content(Helper.GetListJsonStr<Module>(result, rowscount));
        }

        [CloverAuthorize(ModuleCode = "Module", FuncCode = Clover.Permission.Consts.Delete, FuncName = "删除模块")]
        public ActionResult DeleteModule()
        {
            int Moduleid = Utility.GetFormIntParm("moduleIds", 0);

            try
            {
                var m = moddal.Get(Moduleid);
                moddal.DeleteModule(Moduleid);

                OPLogDAO.Log(WebContext.CurrentUser, Utility.GetViewerIP(),
                    "删除模块操作", m);

            }
            catch (Exception ex)
            {
                log.Current.Error("删除模块操作失败", ex);
                return Content("");
            }
            return Content("");           
        }

        private bool ValidateEdit(Module model)
        {
            if (String.IsNullOrEmpty(model.ModuleCode))
            {
                ModelState.AddModelError("ModuleCode", "请填写模块代码");
            }

            if (String.IsNullOrEmpty(model.ModuleName))
            {
                ModelState.AddModelError("LoginName", "请填写模块名称");
            }

            if (ModelState.IsValid && moddal.CheckExistsSameID(model.ModuleCode, model.ModuleID.ToString()))
            {
                ModelState.AddModelError("ModuleCode", "模块代码已经存在，请输入另外一个");
            }

            return ModelState.IsValid;
        }
        [HttpGet]
        [CloverAuthorize(ModuleCode = "Module", FuncCode = Clover.Permission.Consts.Create, FuncName = "创建模块")]
        public ActionResult ModuleAdd()
        {
            IsEdit = false;
            var m = new Module();
            m.ParentID = Utility.GetIntParm("ParentID", -1);
            m.SystemID = Utility.GetIntParm("SystemID", 0);
            LoadPMSystem();
            return View("ModuleEdit", m);
        }

        [HttpPost]
        [CloverAuthorize(ModuleCode = "Module", FuncCode = Clover.Permission.Consts.Create, FuncName = "创建模块")]
        public ActionResult ModuleAdd(string ModuleName, string ModuleCode, string ModuleTag,int Status, int SystemID,int ViewOrd, string ParentID, string Descn)
        {
            LoadPMSystem();

            var model = new Module();
            model.ModuleName = ModuleName;
            model.ModuleCode = ModuleCode;
            model.ModuleTag = ModuleTag;
            model.SystemID = SystemID;
            model.ViewOrd = ViewOrd;
            model.Status = Status;
            if(ConvertHelper.IsInt32(ParentID))
                model.ParentID = int.Parse(ParentID);
            model.Descn = Descn;

            if (ValidateEdit(model))
            {
                try
                {                   
                    moddal.InsertModule(model);

                    return RefreshParentAndCloseFrame();
                  
                }
                catch (Exception ex)
                {
                    ShowError("添加失败");
                    log.Current.Error("模块信息失败", ex);
                }
            }
            return View("ModuleEdit", model);
        }

        [HttpGet]
        [CloverAuthorize(ModuleCode = "Module", FuncCode = Clover.Permission.Consts.Edit, FuncName = "编辑模块")]
        public ActionResult ModuleEdit(int ModuleID)
        {
            IsEdit = true;
            LoadPMSystem();
            return View(moddal.Get(ModuleID));
        }

        

        [HttpPost]
        [CloverAuthorize(ModuleCode = "Module", FuncCode = Clover.Permission.Consts.Edit, FuncName = "编辑模块")]
        public ActionResult ModuleEdit(string ModuleName, string ModuleCode, string ModuleTag, int Status, string ParentID, int SystemID, int ViewOrd, string Descn)
        {
            LoadPMSystem();

            TempData.Keep("SelectedParentModule");
            int ModuleID = Utility.GetIntParm("ModuleID",0);

            var model = new Module();
            model.ModuleName = ModuleName;
            model.ModuleCode = ModuleCode;
            model.ModuleTag = ModuleTag;
            model.SystemID = SystemID;
            model.ViewOrd = ViewOrd;
            if (ConvertHelper.IsInt32(ParentID))
                model.ParentID = int.Parse(ParentID);
            model.Status = Status;
            model.Descn = Descn;

            if (ModuleID != 0)
            {

                model.ModuleID = ModuleID;
                if (ValidateEdit(model))
                {
                    try
                    {
                        model = moddal.Get(model.ModuleID);
                        
                        model.ModuleName = ModuleName;
                        model.ModuleCode = ModuleCode;
                        model.ModuleTag = ModuleTag;
                        model.SystemID = SystemID;
                        model.ViewOrd = ViewOrd;
                        if (ConvertHelper.IsInt32(ParentID))
                            model.ParentID = int.Parse(ParentID);
                        else
                            model.ParentID = -1;
                        model.Descn = Descn;
                        model.Status = Status;

                        moddal.UpdateModule(model);

                        return RefreshParentAndCloseFrame();
                      
                    }
                    catch (Exception ex)
                    {
                        ShowError("修改失败");
                        log.Current.Error("模块信息失败", ex);
                    }
                }
            }
            return View(model);
        }

        [CloverAuthorize(ModuleCode = "Module", FuncCode = "JoinFunction", FuncName = "添加功能模块关联")]
        public ActionResult AddModuleFunction()
        {
            int Moduleid = Utility.GetFormIntParm("ModuleId", 0);
            string ids = Utility.GetFormParm("functionIds", "");
            var rudal = ObjectFactory.GetInstance<ModuleFunctionBLL>();

            try
            {
                rudal.InsertModuleFunctions(Moduleid, StringHelper.SplitString(ids, ",").ToArray());

                OPLogDAO.Log(WebContext.CurrentUser, Utility.GetViewerIP(),
                    "功能关联操作", string.Format("模块ID:{0}的以下功能关联:{1}", Moduleid, ids));

            }
            catch (Exception ex)
            {
                log.Current.Error("添加功能关联操作失败", ex);
                return Content("");
            }
            return Content("");
        }        

        [CloverAuthorize(ModuleCode = "Module", FuncCode = "JoinFunction", FuncName = "删除功能模块关联")]
        public ActionResult DeleteModuleFunction()
        {
            int Moduleid = Utility.GetFormIntParm("ModuleId", 0);
            string ids = Utility.GetFormParm("functionIds", "");
            var rudal = ObjectFactory.GetInstance<ModuleFunctionBLL>();

            try
            {
                rudal.DeleteModuleFunctions(Moduleid, StringHelper.SplitString(ids, ",").ToArray());

                OPLogDAO.Log(WebContext.CurrentUser, Utility.GetViewerIP(), 
                    "删除功能关联操作", string.Format("删除模块ID:{0}的以下功能关联:{1}",Moduleid,ids));

            }
            catch (Exception ex)
            {
                log.Current.Error("删除功能关联操作失败", ex);
                return Content("");
            }
            return Content("");
        }

        [HttpPost]
        [CloverAuthorize(ModuleCode = "Module", FuncCode = Clover.Permission.Consts.Browse, FuncName = "浏览功能模块管理信息")]
        public ActionResult ModuleFunctionList()
        {
            int Moduleid = Utility.GetFormIntParm("ModuleID", 0);
            var funcbll = ObjectFactory.GetInstance<FunctionBLL>();

            try
            {
                var result = funcbll.GetModuleFunctions(Moduleid);
                return Content(Helper.GetListJsonStr<Function>(result, result.Count));
            }
            catch (Exception ex)
            {
                log.Current.Error("读取模块功能操作失败", ex);
                return Content("");
            }         
        }

        [CloverAuthorize(ModuleCode = "Module", FuncCode = "JoinFunction", FuncName = "浏览功能模块")]
        public ActionResult ModuleNoFunctionRef()
        {        
            string codeorname = Utility.GetFormParm("CodeOrName", "");
            int moduleid = Utility.GetFormIntParm("ModuleID", 0);

            var funcbll = ObjectFactory.GetInstance<FunctionBLL>();

            string where = string.Empty;            
            if (!string.IsNullOrEmpty(codeorname))
            {
                where = string.Format(" (FunctionName LIKE '%{0}%' or FunctionCode LIKE '%{0}%') ", codeorname);
            }
         
            var result = funcbll.GetFunctionByNoModuleRef(moduleid , where);

            return Content(Helper.GetListJsonStr(result, result.Count));
        }

        private void LoadPMSystem() {
            var systembll = ObjectFactory.GetInstance<PMSystemBLL>();
            ViewData["PMSystemList"] = systembll.SelectPMSystemList("");
        }
        
    }
}
