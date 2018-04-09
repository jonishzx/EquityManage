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
using System.Text;

namespace UkeyTech.OA.WebApp.Areas.Admin.Controllers
{
    /// <summary>
    /// 管理员账号相关控制
    /// </summary>
    public partial class PermissionController : AdminLoginController
    {
        private readonly RoleBLL dal = ObjectFactory.GetInstance<RoleBLL>();
        private readonly RoleUserBLL roleuserdal = ObjectFactory.GetInstance<RoleUserBLL>();
      
        [CloverAuthorize(ModuleCode="Role", FuncCode = Clover.Permission.Consts.Browse, FuncName = "浏览用户角色")]
        public ActionResult Role(string type)
        {
            switch (type) { 
                case "GetRoleTree":
                    return GetRoleTree();
                case "RoleList":
                    return RoleList();
                case "DeleteRoles":
                    return DeleteRoles();
                case "RoleUserList":
                    return RoleUserList();
                case "DeleteRoleUser":
                    return DeleteRoleUser();
                case "AddRoleUser":
                    return AddRoleUser();
                case "RoleNoUserRef":
                    return RoleNoUserRef();   
            }
                
            return View();
        }

        [CloverAuthorize(ModuleCode="Role", FuncCode = Clover.Permission.Consts.Browse, FuncName = "浏览用户角色")]
        public ActionResult GetRoleTree()
        {
            try
            {
                string output = Helper.ToJsonTree<Role>(dal.GetRoleTree());
                return Content(output);
            }
            catch (Exception ex)
            {                
                log.Current.Error("读取角色信息失败", ex);
                return Content("");
            }
        }

        [CloverAuthorize(ModuleCode = "Role", FuncCode = Clover.Permission.Consts.Browse, FuncName = "浏览用户角色")]
        public ActionResult RoleList()
        {         
            string code = Utility.GetFormParm("Code", "");
            string name = Utility.GetFormParm("Name", "");

            string parentid = Utility.GetFormParm("ParentID", "");

            //int rowscount = 0;

            List<Role> result = dal.SelectRoleList(code, name, parentid);
            if (result == null) throw new ArgumentNullException("result");

            return Content(Helper.GetListJsonStr<Role>(result, result.Count));
        }

        [CloverAuthorize(ModuleCode = "Role", FuncCode = Clover.Permission.Consts.Delete, FuncName = "删除角色")]
        public ActionResult DeleteRoles()
        {           
            int roleid = Utility.GetFormIntParm("roleId", 0);

            try
            {
                var m = dal.Get(roleid);
                var users = roleuserdal.GetRoleUsers(m.RoleCode);
                if (users.Count > 0)
                {
                    var sb = new StringBuilder();
                    users.ForEach(x=>sb.Append(x.UserName + ","));
                    return Fail("用户[" + sb.ToString().Trim(',') + "]已经与该角色关联,请先解除用户与角色的关系再删除角色");
                }
                else
                {
                    dal.DeleteRole(roleid);
                    OPLogDAO.Log(WebContext.CurrentUser, Utility.GetViewerIP(),
                        "删除角色操作", m);
                    return Success();
                }
            }
            catch (Exception ex)
            {
                log.Current.Error("删除角色操作失败", ex);
                return Fail("删除失败,请联系系统管理员");
            }
        }

        private bool ValidateEdit(Role model)
        {
            if (String.IsNullOrEmpty(model.RoleCode))
            {
                ModelState.AddModelError("RoleCode", "请填写角色代码");
            }

            if (String.IsNullOrEmpty(model.RoleName))
            {
                ModelState.AddModelError("RoleName", "请填写角色名称");
            }

            if (ModelState.IsValid && dal.CheckExistsSameID(model.RoleCode, model.RoleID.ToString()))
            {
                ModelState.AddModelError("RoleCode", "角色代码已经存在，请输入另外一个");
            }

            return ModelState.IsValid;
        }
        [HttpGet]
        [CloverAuthorize(ModuleCode = "Role", FuncCode = Clover.Permission.Consts.Create, FuncName = "创建角色")]
        public ActionResult RoleAdd(string ParentID)
        {
            LoadRoleList(0);
            IsEdit = false;
            var m = new Role();
            if (!string.IsNullOrEmpty(ParentID) && ConvertHelper.IsInt32(ParentID))
            {
                m.ParentID = int.Parse(ParentID);
            }
            return View("RoleEdit", m);
        }

        [HttpPost]
        [CloverAuthorize(ModuleCode = "Role", FuncCode = Clover.Permission.Consts.Create, FuncName = "创建角色")]
        public ActionResult RoleAdd(string RoleName, string RoleCode, string RoleTag, string ParentID, string Descn)
        {
            LoadRoleList(0);
            TempData.Keep("SelectedParentRole");
            var model = new Role();
            model.RoleName = RoleName;
            model.RoleCode = RoleCode;
            model.RoleTag = RoleTag;
            if(ConvertHelper.IsInt32(ParentID))
                model.ParentID = int.Parse(ParentID);
            model.Descn = Descn;

            if (ValidateEdit(model))
            {
                try
                {                   
                    dal.InsertRole(model);

                    return RefreshParentAndCloseFrame();
                  
                }
                catch (Exception ex)
                {
                    ShowError("添加失败");
                    log.Current.Error("角色信息失败", ex);
                }
            }
            return View("RoleEdit", model);
        }

        [HttpGet]
        [CloverAuthorize(ModuleCode = "Role", FuncCode = Clover.Permission.Consts.Edit, FuncName = "编辑角色")]
        public ActionResult RoleEdit(int RoleID)
        {
            LoadRoleList(RoleID);
            IsEdit = true;

            return View(dal.Get(RoleID));
        }

        [HttpPost]
        [CloverAuthorize(ModuleCode = "Role", FuncCode = Clover.Permission.Consts.Edit, FuncName = "编辑角色")]
        public ActionResult RoleEdit(int RoleID, string RoleName, string RoleCode, string RoleTag, string ParentID, string Descn)
        {
            LoadRoleList(RoleID);
            TempData.Keep("SelectedParentRole");

            var model = new Role();
            model.RoleName = RoleName;
            model.RoleCode = RoleCode;
            model.RoleTag = RoleTag;
            if (ConvertHelper.IsInt32(ParentID))
                model.ParentID = int.Parse(ParentID);
            model.Descn = Descn;

            if (RoleID != 0)
            {

                model.RoleID = RoleID;
                if (ValidateEdit(model))
                {
                    try
                    {
                        model = dal.Get(model.RoleID);
                        
                        model.RoleName = RoleName;
                        model.RoleCode = RoleCode;
                        model.RoleTag = RoleTag;
                        if (ConvertHelper.IsInt32(ParentID))
                            model.ParentID = int.Parse(ParentID);
                        else
                            model.ParentID = -1;
                        model.Descn = Descn;

                        dal.UpdateRole(model);

                        return RefreshParentAndCloseFrame();
                      
                    }
                    catch (Exception ex)
                    {
                        ShowError("修改失败");
                        log.Current.Error("角色信息失败", ex);
                    }
                }
            }
            return View(model);
        }

        [CloverAuthorize(ModuleCode = "Role", FuncCode = "JoinUser", FuncName = "添加用户角色关联")]
        public ActionResult AddRoleUser()
        {
            int roleid = Utility.GetFormIntParm("roleId", 0);
            string ids = Utility.GetFormParm("userIds", "");
            var rudal = ObjectFactory.GetInstance<RoleUserBLL>();

            try
            {
                rudal.InsertRoleUsers(roleid, StringHelper.SplitString(ids, ",").ToArray());

                OPLogDAO.Log(WebContext.CurrentUser, Utility.GetViewerIP(),
                    "用户关联操作", string.Format("角色ID:{0}的以下用户关联:{1}", roleid, ids));

            }
            catch (Exception ex)
            {
                log.Current.Error("添加用户关联操作失败", ex);
                return Content("");
            }
            return Content("");
        }        

        [CloverAuthorize(ModuleCode = "Role", FuncCode = "JoinUser", FuncName = "删除用户角色关联")]
        public ActionResult DeleteRoleUser()
        {
            int roleid = Utility.GetFormIntParm("roleId", 0);
            string ids = Utility.GetFormParm("userIds", "");
            var rudal = ObjectFactory.GetInstance<RoleUserBLL>();

            try
            {
                rudal.DeleteRoleUsers(roleid, StringHelper.SplitString(ids, ",").ToArray());

                OPLogDAO.Log(WebContext.CurrentUser, Utility.GetViewerIP(), 
                    "删除用户关联操作", string.Format("删除角色ID:{0}的以下用户关联:{1}",roleid,ids));

            }
            catch (Exception ex)
            {
                log.Current.Error("删除用户关联操作失败", ex);
                return Content("");
            }
            return Content("");
        }

        [CloverAuthorize(ModuleCode = "Role", FuncCode = Clover.Permission.Consts.Browse, FuncName = "浏览用户角色管理信息")]
        public ActionResult RoleUserList()
        {        
            int roleid = Utility.GetFormIntParm("RoleID", 0);
            var admindal = ObjectFactory.GetInstance<AdminDAO>();

            try
            {
                int rowscount = 0;
                var result = admindal.GetAdminByRole(PageSize, PageIndex, roleid, out rowscount);
                return Content(Helper.GetListJsonStr(result, rowscount));
            }
            catch (Exception ex)
            {
                log.Current.Error("读取角色用户操作失败", ex);
                return Content("");
            }         
        }

        [CloverAuthorize(ModuleCode = "Role", FuncCode = "JoinUser", FuncName = "浏览用户角色")]
        public ActionResult RoleNoUserRef()
        {        
            string codeorname = Utility.GetFormParm("CodeOrName", "");
            int roleid = Utility.GetFormIntParm("RoleID", 0);

            var admindao = ObjectFactory.GetInstance<AdminDAO>();

            
            string where = string.Empty;
            if (!string.IsNullOrEmpty(codeorname))
            {
                where = string.Format(" (LoginName LIKE '%{0}%' OR AdminName LIKE '%{0}%') ", codeorname);
            }
            int rowscount = 0;
            var result = admindao.GetNotJoinUserByRole(PageSize, PageIndex, roleid, where, out rowscount);

            return Content(Helper.GetListJsonStr(result, rowscount));
        }

        private void LoadRoleList(int RoleID)
        {
            var rolebll = StructureMap.ObjectFactory.GetInstance<RoleBLL>();

            ViewData["Parentlist"] = rolebll.SelectRoleList("RoleID<>" + RoleID);
        }
    }
}
