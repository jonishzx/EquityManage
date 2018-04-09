using System;
using System.Data;
using System.Collections.Generic;
using System.Web.Mvc;

using Clover.Web.Core;
using StructureMap;

using Clover.Permission.BLL;
using Clover.Permission.Model;
using UkeyTech.OA.WebApp.Extenstion;

namespace UkeyTech.OA.WebApp.Areas.Admin.Controllers
{
    /// <summary>
    /// 控制
    /// </summary>
    public partial class PermissionController : AdminLoginController
    {
        FuncPermissionBLL funcpbll = ObjectFactory.GetInstance<FuncPermissionBLL>();

        [CloverAuthorize(ModuleCode = "FuncPermission", FuncCode = Clover.Permission.Consts.Browse, FuncName = "浏览功能授权信息")]
        public ActionResult FuncPermission(string type)
        {
            switch (type)
            {
                case "FuncPermissionList":
                    return GetFuncPermissionList();
            }

            return RolePermission();
            //return View();
        }


        [CloverAuthorize(ModuleCode = "FuncPermission", FuncCode = Clover.Permission.Consts.Browse, FuncName = "浏览功能授权信息")]
        public ActionResult RolePermission()
        {
            GetFunctionColumns("FunctionTag='Common'");
            return View("RolePermission");
        }

        [CloverAuthorize(ModuleCode = "Module", FuncCode = Clover.Permission.Consts.Browse, FuncName = "模块树结构")]
        public ActionResult GetModuleTreeContent(string OwnerCode, string OwnerValue, string ScopeCode, string ScopeValue)
        {
            if (string.IsNullOrEmpty(OwnerCode) || string.IsNullOrEmpty(OwnerValue))
                return Content(Helper.EmptyRows);

            string codeorname = Utility.GetFormParm("CodeOrName", "");
            int rowscount = 0;
            var owner = Clover.Permission.Common.TranPermissionOwner(OwnerCode);
            var result = moddal.SelectModuleListWithFunction(PageSize, PageIndex, null, codeorname, string.Empty, out rowscount);
            var ds = CreateFunctionMartix(OwnerCode, OwnerValue, ScopeCode, ScopeValue, owner);
            var dt = new DataTable();
            if (ds != null) {
                dt= ds.Tables[0].Clone(); 
            }
            dt.Columns.Add("_parentId");
            dt.Columns.Add("ModuleTag");
            dt.Columns.Add("SystemID");
            dt.Columns.Add("ModuleCode");
            foreach (var r in result)
            {
                if (!result.Exists(x => x.ModuleID == r.ParentID))
                    r.ParentID = null;

                DataRow newdr = dt.NewRow();
                newdr["ModuleID"] = r.ModuleID;
                newdr["ModuleCode"] = r.ModuleCode;
                newdr["ModuleName"] = r.ModuleName;
                newdr["ModuleTag"] = r.ModuleTag;
                newdr["SystemID"] = r.SystemID;
                newdr["_parentId"] = r._parentId;
                var rows = ds.Tables[0].Select("ModuleID='" + r.ModuleID + "'");
                if (rows.Length > 0)
                {
                    foreach (DataColumn dc in ds.Tables[0].Columns)
                    {
                        newdr[dc.ColumnName] = rows[0][dc.ColumnName].ToString();
                    }
                }
                dt.Rows.Add(newdr);
            }
            string output = Helper.GetDataTableJsonStr(dt, rowscount);
            return Content(output);
        }

        /// <summary>
        /// 获取功能列
        /// </summary>
        private void GetFunctionColumns(string where)
        {
            var cloumns = new System.Text.StringBuilder();
            var list = new FunctionBLL().SelectFunctionListWithOrder(where, "ViewOrd");
            foreach (Function model in list)
            {
                cloumns.Append(string.Concat("{ field: '", model.FunctionID, "', title: '", model.FunctionName, "',  align: 'center', width: ", model.FunctionName.Length * 20));
                cloumns.Append(",formatter: function (value, rec) {   return CreatCellInfo(value,'" + Clover.Config.CPM.PermissionConfig.Config.EnableDenyPermission.ToString() + "');}");
                cloumns.Append(" },");
            }
            cloumns.Remove(cloumns.Length - 1, 1);
            ViewData["Columns"] = cloumns.ToString();
        }

        [CloverAuthorize(ModuleCode = "FuncPermission", FuncCode = Clover.Permission.Consts.Browse, FuncName = "浏览功能授权信息")]
        public ActionResult FuncPermissionView()
        {

            GetFunctionColumns(string.Empty);
            return View();
        }

        [CloverAuthorize(ModuleCode = "FuncPermission", FuncCode = Clover.Permission.Consts.Browse, FuncName = "浏览功能授权信息")]
        public ActionResult FuncPermissionViewList(string OwnerCode, string OwnerValue, string ScopeCode, string ScopeValue)
        {

            var owner = Clover.Permission.Common.TranPermissionOwner(OwnerCode);

            var ds = CreateFunctionMartix(OwnerCode, OwnerValue, ScopeCode, ScopeValue, owner);

            return Content(Helper.GetDataTableJsonStr(ds.Tables[0], ds.Tables[0].Rows.Count));
        }

        private DataSet CreateFunctionMartix(string OwnerCode, string OwnerValue, string ScopeCode, string ScopeValue, PermissionOwner owner)
        {
            List<UserFuncPMResult> list = null;

            //超级管理员及其角色能访问所有权限
            if (owner == PermissionOwner.Role)
            {
                if (OwnerValue != "null")
                {
                    Role rm = dal.Get(int.Parse(OwnerValue));
                    switch (rm.RoleCode)
                    {
                        case SystemVar.AdminRoleCode:
                            list = funcpbll.GetAllFuncPermission();
                            break;
                        case SystemVar.PMAdminRoleCode:
                            list = funcpbll.GetPMFuncPermission(rm.RoleID);
                            break;
                        default:
                            list = funcpbll.GetFuncPermission(
                                Clover.Permission.Common.TranPermissionOwner(OwnerCode),
                                OwnerValue,
                                Clover.Permission.Common.TranFilterScope(ScopeCode),
                                ScopeValue);
                            break;
                    }
                }
            }
            else if (owner == PermissionOwner.User && OwnerValue == SystemVar.AdminRoleCode)
            {
                list = funcpbll.GetAllFuncPermission();
            }
            else
            {
                list = funcpbll.GetFuncPermission(
                Clover.Permission.Common.TranPermissionOwner(OwnerCode),
                OwnerValue,
                Clover.Permission.Common.TranFilterScope(ScopeCode),
                ScopeValue);
            }
            DataSet ds = null;
            if (list != null)
            {
                ds = funcpbll.FunctionAsColumnPermissionList(list, true);
            }
            return ds;
        }

        [CloverAuthorize(ModuleCode = "FuncPermission", FuncCode = Clover.Permission.Consts.Browse, FuncName = "浏览功能授权信息")]
        public ActionResult FuncPermissionList()
        {
            string codeorname = Utility.GetFormParm("CodeOrName", "");
            string parentid = Utility.GetFormParm("ParentID", "");
            int? functionid = Utility.GetIntParm("FunctionID", 0);

            int rowscount = 0;

            var result = funcbll.SelectFunctionList(PageSize, PageIndex, codeorname, out rowscount);

            return Content(Helper.GetListJsonStr<Function>(result, rowscount));
        }

        [HttpPost]
        [CloverAuthorize(ModuleCode = "FuncPermission", FuncCode = Clover.Permission.Consts.Edit, FuncName = "更新功能授权信息")]
        public ActionResult UpdateFuncDataPermission(int FuncPermissionID, int? DataPermissionId, string ownerTitle, string ownerValue)
        {
            var model = funcpbll.GetFuncPermission(FuncPermissionID);
            model.DataPermissionId = DataPermissionId;
            funcpbll.UpdateFuncPermission(model, ownerTitle, ownerValue);
            return Success(model.FuncPermissionID.ToString());
        }

        [CloverAuthorize(ModuleCode = "FuncPermission", FuncCode = Clover.Permission.Consts.Browse, FuncName = "浏览功能授权信息")]
        public ActionResult GetRelationFunctionList(string functionId)
        {
            var relationid = funcbll.Get(int.Parse(functionId)).RelationFunctionID;

            return string.IsNullOrEmpty(relationid) ? Content("") : Content(relationid.Trim(new char[] { '(' }).Trim(new char[] { ')' }));
        }


        /// <summary>
        /// 将来源的权限来源拷贝到目标的来源
        /// </summary>
        /// <param name="ownerTitle"></param>
        /// <param name="srcOwnerValue"></param>
        /// <param name="targetOwnerValue"></param>
        /// <returns></returns>
        [HttpPost]
        [CloverAuthorize(ModuleCode = "FuncPermission", FuncCode = "UserEnabled", FuncName = "权限克隆")]
        public ActionResult CloneFuncPermission(string ownerTitle, string targetOwnerValue, string CloneRoleValue)
        {
            try
            {
                if (CloneRoleValue.Equals(targetOwnerValue, StringComparison.CurrentCultureIgnoreCase))
                {
                    return Fail("克隆的角色与目标角色相同，无需克隆权限");
                }
                funcpbll.CloneFuncPermission(ownerTitle, CloneRoleValue, targetOwnerValue);
                RefreshParentAndCloseFrame();
                return Success();
            }
            catch (Exception ex)
            {
                ShowError("克隆失败,失败原因" + ex.Message);
                return Fail("克隆失败,失败原因" + ex.Message);
            }

        }
        [CloverAuthorize(ModuleCode = "FuncPermission", FuncCode = "UserEnabled", FuncName = "设置功能授权信息")]
        public ActionResult SetFuncPermission(string moduleId, string ownerValue, string ownerTitle, string funcs)
        {
            if (!string.IsNullOrEmpty(ownerValue))
            {
                int num = int.Parse(moduleId);
                PermissionOwner owner = Clover.Permission.Common.TranPermissionOwner(ownerTitle);
                var list = new List<FuncPermission>();
                if (!string.IsNullOrEmpty(funcs))
                {
                    //funcs 为 functionID;isallow;isdeny|functionID;isallow;isdeny
                    string[] strArray = funcs.TrimEnd(new char[] { '|' }).Split(new char[] { '|' });
                    foreach (string str in strArray)
                    {
                        string[] strArray2 = str.TrimEnd(new char[] { ';' }).Split(new char[] { ';' });
                        if (strArray2.Length == 3)
                        {
                            var item = new FuncPermission
                            {
                                ModuleID = num,
                                FunctionID = int.Parse(strArray2[0])
                            };
                            switch (owner)
                            {
                                case PermissionOwner.User:
                                    item.UserID = ownerValue;
                                    break;

                                case PermissionOwner.Role:
                                    item.RoleID = int.Parse(ownerValue);
                                    break;

                                case PermissionOwner.Group:
                                    item.GroupID = int.Parse(ownerValue);
                                    break;
                            }
                            item.CreateDate = DateTime.Now;
                            item.IsAllow = bool.Parse(strArray2[1]);
                            item.IsDeny = bool.Parse(strArray2[2]);
                            list.Add(item);
                        }
                    }
                }
                funcpbll.SetFuncPermission(num, owner, ownerValue, list);
            }

            return Content("");
        }


        [CloverAuthorize(ModuleCode = "FuncPermission", FuncCode = Clover.Permission.Consts.Browse, FuncName = "浏览功能授权信息")]
        public ActionResult GetFuncPermissionList()
        {
            int ModuleID = Utility.GetFormIntParm("ModuleID", 0);
            string OwnerTitle = Utility.GetFormParm("OwnerTitle", "");
            string OwnerValue = Utility.GetFormParm("OwnerValue", "");

            if (ModuleID != 0)
            {
                if (!(string.IsNullOrEmpty(OwnerTitle) && !string.IsNullOrEmpty(OwnerValue)))
                {
                    PermissionOwner owner = Clover.Permission.Common.TranPermissionOwner(OwnerTitle);
                    List<UserFuncPMResult> list = funcpbll.GetFuncPermission(owner, OwnerValue, Clover.Permission.BLL.FilterScope.Module, ModuleID.ToString());
                    return Content(Helper.GetListJsonStr<UserFuncPMResult>(list, list.Count));
                }
                else
                {
                    List<Function> functionByModule = new FunctionBLL().GetModuleFunctions(ModuleID);
                    return Content(Helper.GetListJsonStr<Function>(functionByModule, functionByModule.Count));
                }
            }

            return Content("");
        }
    }
}
