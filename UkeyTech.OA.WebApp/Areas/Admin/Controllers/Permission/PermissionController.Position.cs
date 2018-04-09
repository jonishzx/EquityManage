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
        private readonly PositionBLL pdal = ObjectFactory.GetInstance<PositionBLL>();

        [CloverAuthorize(ModuleCode = "Position", FuncCode = Clover.Permission.Consts.Browse, FuncName = "浏览岗位信息")]
        public ActionResult Position(string type)
        {
            switch (type)
            {             
                case "PositionList":
                    return PositionList();
                case "PositionTreeList":
                    return PositionTreeList();
                case "DeletePositions":
                    return DeletePositions();
                case "PositionUserList":
                    return PositionUserList();
                case "DeletePositionUser":
                    return DeletePositionUser();
                case "AddPositionUser":
                    return AddPositionUser();
                case "PositionNoUserRef":
                    return PositionNoUserRef();
            }

            return View();
        }

        public ActionResult GetAllPositionTree()
        {
            return GetPositionTreeContent();
        }

        private ActionResult GetPositionTreeContent()
        {
            try
            {
                int page = 1;
                int rows = 9999999;
                string codeorname = string.Empty;
                string parentid = string.Empty;
                string groupid = Utility.GetParm("GroupID", "");
                string exceptid = Utility.GetParm("exceptPosID", "");
                int rowscount = 0;
                var result = pdal.SelectPositionList(rows, page, codeorname, groupid, parentid, exceptid, out rowscount);
                var tree = new Clover.Core.Collection.Tree<Position>(result);

                string output = Helper.ToJsonTree<Position>(tree);
                return Content(output);
            }
            catch (Exception ex)
            {
                log.Current.Error("读取组信息失败", ex);
                return Content("");
            }
        }

        [CloverAuthorize(ModuleCode = "Group", FuncCode = Clover.Permission.Consts.Browse, FuncName = "浏览用户组")]
        public ActionResult GetPositionTree()
        {
            return GetPositionTreeContent();
        }

        [CloverAuthorize(ModuleCode = "Position", FuncCode = Clover.Permission.Consts.Browse, FuncName = "浏览岗位信息")]
        public ActionResult PositionTreeList()
        {          
            string codeorname = Utility.GetFormParm("CodeOrName", "");
            string parentid = Utility.GetFormParm("id", "");
            string groupid = Utility.GetFormParm("ParentID", "");
            string exceptid = Utility.GetFormParm("exceptPosID", "");

            int rowscount = 0;

            var result = pdal.SelectPositionList(PageSize, PageIndex, codeorname, groupid, parentid, exceptid, out rowscount);

            return Content(Helper.GetTreeListJsonStr<Position>(result, rowscount));
        }

        private string GetChildren(string positonid, string exceptid)
        {
            int rowscount = 0;
            var result = pdal.SelectPositionList(int.MaxValue, 1, null, string.Empty, positonid, exceptid, out rowscount);
            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }

        [CloverAuthorize(ModuleCode = "Position", FuncCode = Clover.Permission.Consts.Browse, FuncName = "浏览岗位信息")]
        public ActionResult PositionList()
        {        
            string codeorname = Utility.GetFormParm("CodeOrName", "");
            string parentid = Utility.GetFormParm("id", "");
            string groupid = Utility.GetFormParm("ParentID", "");

            int rowscount = 0;

            var result = pdal.SelectPositionList(PageSize, PageIndex, codeorname, groupid, parentid, "", out rowscount);

            foreach (var pos in result) {
                if ( pos.ParentID.HasValue && !result.Exists(x => x.Id == pos.ParentID.Value.ToString())) {
                    pos.ParentID = null;
                }
            }

            return Content(Helper.GetListJsonStr<Position>(result, rowscount));
        }

        /// <summary>
        /// 岗位选择视图
        /// </summary>
        /// <returns></returns>
        public ActionResult WFPositionList()
        {
            string codeorname = Utility.GetFormParm("CodeOrName", "");
            string parentid = Utility.GetFormParm("id", "");
            string groupid = Utility.GetFormParm("ParentID", "");

            int rowscount = 0;

            List<Position> result = null;
            if(Clover.Config.CPM.PermissionConfig.Config.PositionBaseOnUserGroup)
            {
                result = pdal.GetGroupPositionAllPaged(PageSize, PageIndex, codeorname, groupid, parentid, out rowscount);
            }else
            {
                result = pdal.SelectPositionList(PageSize, PageIndex, codeorname, groupid, parentid, "", out rowscount);
            }

            foreach (var pos in result)
            {
                if (pos.ParentID.HasValue && !result.Exists(x => x.Id == pos.ParentID.Value.ToString()))
                {
                    pos.ParentID = null;
                }
            }

            return Content(Helper.GetListJsonStr<Position>(result, rowscount));
        }

        [CloverAuthorize(ModuleCode = "Position", FuncCode = Clover.Permission.Consts.Delete, FuncName = "删除岗位")]
        public ActionResult DeletePositions()
        {
            int Positionid = Utility.GetFormIntParm("PositionIds", 0);

            try
            {
                var m = pdal.Get(Positionid);
                var rudal = ObjectFactory.GetInstance<PositionUserBLL>();
                var users = rudal.GetPositionUsers(m.PositionID.ToString());
                if (users.Count > 0)
                {
                    var sb = new StringBuilder();
                    users.ForEach(x => sb.Append(x.UserName + ","));
                    return Fail("用户[" + sb.ToString().Trim(',') + "]已经与该岗位关联,请先解除用户与岗位的关系再删除岗位");
                }
                else
                {
                    pdal.DeletePosition(Positionid);
                    OPLogDAO.Log(WebContext.CurrentUser, Utility.GetViewerIP(),
                        "删除岗位操作", m);
                    //return Success();
                    return Content("");
                }
            }
            catch (Exception ex)
            {
                log.Current.Error("删除岗位操作失败", ex);
                return Content("");
            }

        }

        private bool ValidateEdit(Position model)
        {
            if (String.IsNullOrEmpty(model.PositionCode))
            {
                ModelState.AddModelError("PositionCode", "请填写岗位代码");
            }

            if (String.IsNullOrEmpty(model.PositionName))
            {
                ModelState.AddModelError("PositionName", "请填写岗位名称");
            }

            if (ModelState.IsValid && pdal.CheckExistsSameID(model.PositionCode, model.PositionID.ToString()))
            {
                ModelState.AddModelError("PositionCode", "岗位代码已经存在，请输入另外一个");
            }

            return ModelState.IsValid;
        }

        [HttpGet]
        [CloverAuthorize(ModuleCode = "Position", FuncCode = Clover.Permission.Consts.Create, FuncName = "创建岗位")]
        public ActionResult PositionAdd(string GroupId)
        {
            LoadPositionList(0);
            IsEdit = false;
            var m = new Position();
            if (!string.IsNullOrEmpty(GroupId) && ConvertHelper.IsInt32(GroupId))
            {
                m.GroupId = int.Parse(GroupId);
            }
            return View("PositionEdit", m);
        }

        [HttpPost]
        [CloverAuthorize(ModuleCode = "Position", FuncCode = Clover.Permission.Consts.Create, FuncName = "创建岗位")]
        public ActionResult PositionAdd(string PositionName, string PositionCode, string GroupId, string ParentID, string Descn, int PositionLevel)
        {
            LoadPositionList(0);
            TempData.Keep("SelectedParentPosition");
            var model = new Position() { PositionName = PositionName, PositionCode = PositionCode };

            if (ConvertHelper.IsInt32(GroupId))
                model.GroupId = int.Parse(GroupId);

            if (ConvertHelper.IsInt32(ParentID))
                model.ParentID = int.Parse(ParentID);

            model.Descn = Descn;
            model.PositionLevel = PositionLevel;

            if (ValidateEdit(model))
            {
                try
                {
                    pdal.InsertPosition(model);
                   
                    return RefreshParentAndCloseFrame();

                }
                catch (Exception ex)
                {
                    ShowError("添加失败");
                    log.Current.Error("岗位信息失败", ex);
                }
            }
            return View("PositionEdit", model);
        }

        [HttpGet]
        [CloverAuthorize(ModuleCode = "Position", FuncCode = Clover.Permission.Consts.Edit, FuncName = "编辑岗位")]
        public ActionResult PositionEdit(int PositionID)
        {
            IsEdit = true;
            LoadPositionList(PositionID);
            return View(pdal.Get(PositionID));
        }

        [HttpPost]
        [CloverAuthorize(ModuleCode = "Position", FuncCode = Clover.Permission.Consts.Edit, FuncName = "编辑岗位")]
        public ActionResult PositionEdit(int PositionID, string PositionName, string PositionCode, string GroupId, string ParentID, string Descn, int PositionLevel)
        {
            LoadPositionList(PositionID);

            TempData.Keep("SelectedParentPosition");

            var model = new Position() { PositionName = PositionName, PositionCode = PositionCode };
            if (ConvertHelper.IsInt32(ParentID))
                model.ParentID = int.Parse(ParentID);
            if (ConvertHelper.IsInt32(GroupId))
                model.GroupId = int.Parse(GroupId);
            model.Descn = Descn;

            if (PositionID != 0)
            {

                model.PositionID = PositionID;
                if (ValidateEdit(model))
                {
                    try
                    {
                        model = pdal.Get(model.PositionID);

                        model.PositionName = PositionName;
                        model.PositionCode = PositionCode;
                        model.Modifitor = WebContext.CurrentUser.UniqueId;

                        if (ConvertHelper.IsInt32(ParentID))
                            model.ParentID = int.Parse(ParentID);

                        model.UpdateTime = DateTime.Now;
                        if (ConvertHelper.IsInt32(GroupId))
                            model.GroupId = int.Parse(GroupId);
                        else
                            model.GroupId = -1;
                        model.Descn = Descn;
                        model.PositionLevel = PositionLevel;

                        pdal.UpdatePosition(model);

                        return RefreshParentAndCloseFrame();

                    }
                    catch (Exception ex)
                    {
                        ShowError("修改失败");
                        log.Current.Error("岗位信息失败", ex);
                    }
                }
            }
            return View(model);
        }

        [CloverAuthorize(ModuleCode = "Position", FuncCode = "JoinUser", FuncName = "添加用户岗位关联")]
        public ActionResult AddPositionUser()
        {
            int Positionid = Utility.GetFormIntParm("PositionId", 0);
            int? GroupID = null;
            if (Utility.GetFormIntParm("GroupID", 0) > 0)
                GroupID = Utility.GetFormIntParm("GroupID", 0);

            string ids = Utility.GetFormParm("userIds", "");
            var rudal = ObjectFactory.GetInstance<PositionUserBLL>();

            try
            {
                rudal.InsertPositionUsers(Positionid, GroupID, StringHelper.SplitString(ids, ",").ToArray());

                OPLogDAO.Log(WebContext.CurrentUser, Utility.GetViewerIP(),
                    "用户关联操作", string.Format("岗位ID:{0}的以下用户关联:{1}", Positionid, ids));

            }
            catch (Exception ex)
            {
                log.Current.Error("添加用户关联操作失败", ex);
                return Content("");
            }
            return Content("");
        }

        [CloverAuthorize(ModuleCode = "Position", FuncCode = "JoinUser", FuncName = "删除用户岗位关联")]
        public ActionResult DeletePositionUser()
        {
            int Positionid = Utility.GetFormIntParm("PositionId", 0);
            int? GroupID = null;
            if (Utility.GetFormIntParm("GroupID", 0) > 0)
                GroupID = Utility.GetFormIntParm("GroupID", 0);
            string ids = Utility.GetFormParm("userIds", "");
            var rudal = ObjectFactory.GetInstance<PositionUserBLL>();

            try
            {
                rudal.DeletePositionUsers(Positionid, GroupID, StringHelper.SplitString(ids, ",").ToArray());

                OPLogDAO.Log(WebContext.CurrentUser, Utility.GetViewerIP(),
                    "删除用户关联操作", string.Format("删除岗位ID:{0}的以下用户关联:{1}", Positionid, ids));

            }
            catch (Exception ex)
            {
                log.Current.Error("删除用户关联操作失败", ex);
                return Content("");
            }
            return Content("");
        }

        [CloverAuthorize(ModuleCode = "Position", FuncCode = Clover.Permission.Consts.Browse, FuncName = "浏览用户岗位管理信息")]
        public ActionResult PositionUserList()
        {        
            int Positionid = Utility.GetFormIntParm("PositionID", 0);
            var admindal = ObjectFactory.GetInstance<AdminDAO>();

            try
            {
                int rowscount = 0;
                var result = admindal.GetAdminByPosition(PageSize, PageIndex, Positionid, out rowscount);
                return Content(Helper.GetListJsonStr(result, rowscount));
            }
            catch (Exception ex)
            {
                log.Current.Error("读取岗位用户操作失败", ex);
                return Content("");
            }
        }

        [CloverAuthorize(ModuleCode = "Position", FuncCode = "JoinUser", FuncName = "浏览用户岗位")]
        public ActionResult PositionNoUserRef()
        {        
            string codeorname = Utility.GetFormParm("CodeOrName", "");
            int Positionid = Utility.GetFormIntParm("PositionID", 0);

            var admindao = ObjectFactory.GetInstance<AdminDAO>();


            string where = string.Empty;
            if (!string.IsNullOrEmpty(codeorname))
            {
                where = string.Format(" (LoginName LIKE '%{0}%' OR AdminName LIKE '%{0}%') ", codeorname);
            }
            int rowscount = 0;
            var result = admindao.GetAdminNoPositionRef(PageSize, PageIndex, Positionid, where, out rowscount);

            return Content(Helper.GetListJsonStr(result, rowscount));
        }

        private void LoadPositionList(int parentid)
        {
            ViewData["Parentlist"] = pdal.SelectPositionList("PositionID<>" + parentid);
        }
    }
}
