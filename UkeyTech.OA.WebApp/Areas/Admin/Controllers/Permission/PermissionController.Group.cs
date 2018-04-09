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
        private readonly GroupBLL gdal = ObjectFactory.GetInstance<GroupBLL>();

        [CloverAuthorize(ModuleCode = "Group", FuncCode = Clover.Permission.Consts.Browse, FuncName = "浏览组信息")]
        public ActionResult Group(string type)
        {
            switch (type)
            {

                case "GetGroupTree":
                    return GetGroupTree();
                case "GroupList":
                    return GroupList();
                case "GroupTreeList":
                    return GroupTreeList();
                case "DeleteGroups":
                    return DeleteGroups();
                case "GroupUserList":
                    return GroupUserList();
                case "DeleteGroupUser":
                    return DeleteGroupUser();
                case "AddGroupUser":
                    return AddGroupUser();
                case "GroupNoUserRef":
                    return GroupNoUserRef();
            }

            return View();
        }

        public ActionResult GetAllGroupTree()
        {
            return GetGroupTreeContent();
        }

        private ActionResult GetGroupTreeContent()
        {
            string status = Utility.GetParm("Status", "");
            string where = "";

            if (!string.IsNullOrEmpty(status))
            {
                where = string.Format(" (Status = {0} or ParentID = -1) ", status);
            }

            try
            {
                string output = Helper.ToJsonTree<Group>(gdal.GetGroupTreeInstance(where));
                return Content(output);
            }
            catch (Exception ex)
            {
                log.Current.Error("读取组信息失败", ex);
                return Content("");
            }
        }

        [CloverAuthorize(ModuleCode = "Group", FuncCode = Clover.Permission.Consts.Browse, FuncName = "浏览用户组")]
        public ActionResult GetGroupTree()
        {
            return GetGroupTreeContent();
        }

        [CloverAuthorize(ModuleCode = "Group", FuncCode = Clover.Permission.Consts.Browse, FuncName = "浏览组信息")]
        public ActionResult GroupList()
        {
            string codeorname = Utility.GetFormParm("CodeOrName", "");
            string parentid = Utility.GetFormParm("ParentID", "");
            string status = Utility.GetFormParm("Status", Utility.GetParm("Status", ""));

            int rowscount = 0;

            var result = gdal.SelectGroupList(PageSize, PageIndex, codeorname,status, parentid, out rowscount);

            return Content(Helper.GetListJsonStr<Group>(result, rowscount));
        }

        [CloverAuthorize(ModuleCode = "Group", FuncCode = Clover.Permission.Consts.Browse, FuncName = "浏览组信息")]
        public ActionResult GroupTreeList()
        {
            string codeorname = Utility.GetFormParm("CodeOrName", "");
            string parentid = Utility.GetFormParm("ParentID", "");
            string status = Utility.GetFormParm("Status", Utility.GetParm("Status",""));

            int rowscount = 0;

            var result = gdal.SelectGroupList(PageSize, PageIndex, codeorname,status, parentid, out rowscount);

            if (!string.IsNullOrEmpty(parentid))
            {
                var o = result.Find(x => x.GroupID.ToString() == parentid);
                if (o != null)
                {
                    o.ParentID = null;
                }
            }
            var findits = result.FindAll(x => !result.Exists(y => y.GroupID.ToString() == x.ParentId));

            foreach(var it in findits)
            {
                result.Find(x => x.Id == it.Id).ParentID = null;
            }
            return Content(Helper.GetTreeListJsonStr<Group>(result, rowscount));
        }

        [CloverAuthorize(ModuleCode = "Group", FuncCode = Clover.Permission.Consts.Delete, FuncName = "删除组")]
        public ActionResult DeleteGroups()
        {
            int Groupid = Utility.GetFormIntParm("groupIds", 0);

            try
            {
                var m = gdal.Get(Groupid);
                gdal.DeleteGroup(Groupid);

                OPLogDAO.Log(WebContext.CurrentUser, Utility.GetViewerIP(),
                    "删除组操作", m);

            }
            catch (Exception ex)
            {
                log.Current.Error("删除组操作失败", ex);
                return Content("");
            }
            return Content("");
        }

        private bool ValidateEdit(Group model)
        {
            if (String.IsNullOrEmpty(model.GroupCode))
            {
                ModelState.AddModelError("GroupCode", "请填写组代码");
            }

            if (String.IsNullOrEmpty(model.GroupName))
            {
                ModelState.AddModelError("GroupName", "请填写组名称");
            }

            if (ModelState.IsValid && gdal.CheckExistsSameID(model.GroupCode, model.GroupID.ToString()))
            {
                ModelState.AddModelError("GroupCode", "组代码已经存在，请输入另外一个");
            }

            return ModelState.IsValid;
        }

        [HttpGet]
        [CloverAuthorize(ModuleCode = "Group", FuncCode = Clover.Permission.Consts.Create, FuncName = "创建组")]
        public ActionResult GroupAdd(string ParentID)
        {
            LoadGroupList(0);
            IsEdit = false;
            Group m = new Group();
            if (!string.IsNullOrEmpty(ParentID) && ConvertHelper.IsInt32(ParentID))
            {
                m.ParentID = int.Parse(ParentID);
            }
            return View("GroupEdit", m);
        }

        [HttpPost]
        [CloverAuthorize(ModuleCode = "Group", FuncCode = Clover.Permission.Consts.Create, FuncName = "创建组")]
        public ActionResult GroupAdd(string GroupName, string FullName, string GroupCode, string GroupTag, string ParentID, string Descn, string Modifitor)
        {
            LoadGroupList(0);
            TempData.Keep("SelectedParentGroup");
            Group model = new Group();
            model.GroupName = GroupName;
            model.GroupCode = GroupCode;
            model.FullName = FullName;
            if (ConvertHelper.IsInt32(ParentID))
                model.ParentID = int.Parse(ParentID);
            model.Descn = Descn;
            model.Modifitor = Modifitor;

            if (ValidateEdit(model))
            {
                try
                {
                    gdal.InsertGroup(model);

                    return RefreshParentAndCloseFrame();

                }
                catch (Exception ex)
                {
                    ShowError("添加失败");
                    log.Current.Error("组信息失败", ex);
                }
            }
            return View("GroupEdit", model);
        }

        [HttpGet]
        [CloverAuthorize(ModuleCode = "Group", FuncCode = Clover.Permission.Consts.Edit, FuncName = "编辑组")]
        public ActionResult GroupEdit(int GroupID)
        {
            IsEdit = true;
            LoadGroupList(GroupID);
            return View(gdal.Get(GroupID));
        }

        [HttpPost]
        [CloverAuthorize(ModuleCode = "Group", FuncCode = Clover.Permission.Consts.Edit, FuncName = "编辑组")]
        public ActionResult GroupEdit(int GroupID, string GroupName, string FullName, string GroupCode, string GroupTag, string ParentID, string Descn, string Modifitor, int Status)
        {
            LoadGroupList(GroupID);

            TempData.Keep("SelectedParentGroup");

            var model = new Group();
            model.GroupName = GroupName;
            model.GroupCode = GroupCode;
            if (ConvertHelper.IsInt32(ParentID))
                model.ParentID = int.Parse(ParentID);
            model.Descn = Descn;
            model.Modifitor = Modifitor;
            model.Status = Status;

            if (GroupID != 0)
            {

                model.GroupID = GroupID;
                if (ValidateEdit(model))
                {
                    try
                    {
                        model = gdal.Get(model.GroupID);

                        model.GroupName = GroupName;
                        model.GroupCode = GroupCode;
                   
                        if (ConvertHelper.IsInt32(ParentID))
                            model.ParentID = int.Parse(ParentID);
                        else
                            model.ParentID = null;
                        model.Descn = Descn;
                        model.Modifitor = Modifitor;
                        model.FullName = FullName;
                        model.Status = Status;
                        gdal.UpdateGroup(model);

                        //TO-DO 如果将一个父部门变更为禁用,其自部门应当一并变更为禁用 add by lzj 2016-12-19

                        return RefreshParentAndCloseFrame();

                    }
                    catch (Exception ex)
                    {
                        ShowError("修改失败");
                        log.Current.Error("组信息失败", ex);
                    }
                }
            }
            return View(model);
        }

        [CloverAuthorize(ModuleCode = "Group", FuncCode = "JoinUser", FuncName = "添加用户组关联")]
        public ActionResult AddGroupUser()
        {
            int Groupid = Utility.GetFormIntParm("GroupId", 0);
            string ids = Utility.GetFormParm("userIds", "");
            var rudal = ObjectFactory.GetInstance<GroupUserBLL>();

            try
            {
                rudal.InsertGroupUsers(Groupid, StringHelper.SplitString(ids, ",").ToArray());

                OPLogDAO.Log(WebContext.CurrentUser, Utility.GetViewerIP(),
                    "用户关联操作", string.Format("组ID:{0}的以下用户关联:{1}", Groupid, ids));

            }
            catch (Exception ex)
            {
                log.Current.Error("添加用户关联操作失败", ex);
                return Content("");
            }
            return Content("");
        }

        [CloverAuthorize(ModuleCode = "Group", FuncCode = "JoinUser", FuncName = "删除用户组关联")]
        public ActionResult DeleteGroupUser()
        {
            int Groupid = Utility.GetFormIntParm("GroupId", 0);
            string ids = Utility.GetFormParm("userIds", "");
            var rudal = ObjectFactory.GetInstance<GroupUserBLL>();

            try
            {
                rudal.DeleteGroupUsers(Groupid, StringHelper.SplitString(ids, ",").ToArray());

                OPLogDAO.Log(WebContext.CurrentUser, Utility.GetViewerIP(),
                    "删除用户关联操作", string.Format("删除组ID:{0}的以下用户关联:{1}", Groupid, ids));

            }
            catch (Exception ex)
            {
                log.Current.Error("删除用户关联操作失败", ex);
                return Content("");
            }
            return Content("");
        }

        [CloverAuthorize(ModuleCode = "Group", FuncCode = Clover.Permission.Consts.Browse, FuncName = "浏览用户组管理信息")]
        public ActionResult GroupUserList()
        {
            int Groupid = Utility.GetFormIntParm("GroupID", 0);
            var admindal = ObjectFactory.GetInstance<AdminDAO>();

            try
            {
                int rowscount = 0;
                var result = admindal.GetAdminByGroup(PageSize, PageIndex, Groupid, out rowscount);
                return Content(Helper.GetListJsonStr(result, rowscount));
            }
            catch (Exception ex)
            {
                log.Current.Error("读取组用户操作失败", ex);
                return Content("");
            }
        }

        [CloverAuthorize(ModuleCode = "Group", FuncCode = "JoinUser", FuncName = "浏览用户组")]
        public ActionResult GroupNoUserRef()
        {
            string codeorname = Utility.GetFormParm("CodeOrName", "");
            int Groupid = Utility.GetFormIntParm("GroupID", 0);

            var admindao = ObjectFactory.GetInstance<AdminDAO>();


            string where = string.Empty;
            if (!string.IsNullOrEmpty(codeorname))
            {
                where = string.Format(" (LoginName LIKE '%{0}%' OR AdminName LIKE '%{0}%') ", codeorname);
            }
            int rowscount = 0;
            var result = admindao.GetAdminNoGroupRef(PageSize, PageIndex, Groupid, where, out rowscount);

            return Content(Helper.GetListJsonStr(result, rowscount));
        }

        private void LoadGroupList(int groupid)
        {
            var groupbll = StructureMap.ObjectFactory.GetInstance<GroupBLL>();
            //注意不允许选择自己及其子节点作为父节点
            ViewData["Parentlist"] = groupbll.SelectGroupList(string.Format("GroupID<>{0} AND '\\' + ParentPath + '\\' not like '%\\{0}\\%'", groupid));
        }

    }
}
