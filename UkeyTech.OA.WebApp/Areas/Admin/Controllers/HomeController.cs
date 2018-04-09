using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Clover.Config;
using Clover.Core.Common;
using Clover.Permission.BLL;
using Clover.Permission.Model;
using Clover.Web.Core;
using StructureMap;
using UkeyTech.WebFW.DAO;
using UkeyTech.WebFW.Model;
using UkeyTech.OA.WebApp.Extenstion;
using Clover.Core.Collection;

namespace UkeyTech.OA.WebApp.Areas.Admin.Controllers
{

    /// <summary>
    /// 主页
    /// </summary>
    public class HomeController : AdminLoginController
    {
        #region Home
        private readonly UserConfigDAO configdal = ObjectFactory.GetInstance<UserConfigDAO>();
        private readonly UserConfigDAO usrcfgDao = ObjectFactory.GetInstance<UserConfigDAO>();

        //
        // GET: /Admin/Home/
        [AdminErrorHandle]
        public ActionResult Index()
        {
            InitUserMessage();
 
            return View();
        }

        private void InitUserMessage()
        {
            //初始化默认部门及岗位
            InitGroupPositionRole();

            ViewData["PWebContext"] = WebContext;
            ViewData["PMenusItems"] = GetMenuData();
            ViewData["PMemoItems"] = GetMemoData();

            //WidgetController c = new WidgetController();
            //string panels = c.GetUserSelectdWidgetListContent(Url.Action("LoadWidget", "Widget"));
            //string layouts = c.GetWidgetLayout();
            //ViewData["Panels"] = panels;
            //ViewData["Layouts"] = layouts;
        }

        /// <summary>
        ///  初始化默认部门及岗位
        /// </summary>
        private void InitGroupPositionRole()
        {
            var adminId = WebContext.CurrentUser.UniqueId;
            var defgroup = usrcfgDao.GetUserConfigs(adminId, AdminDAO.DefaultGroupTag);
            var defpos = usrcfgDao.GetUserConfigs(adminId, AdminDAO.DefaultGroupPositionTag);
            var defrole = usrcfgDao.GetUserConfigs(adminId, AdminDAO.DefaultRoleTag);

            if ((defgroup.Count == 0 || string.IsNullOrEmpty(defgroup[0].ConfigValue)) ||
                (defpos.Count == 0 || string.IsNullOrEmpty(defgroup[0].ConfigValue)))
            {
                var dal = ObjectFactory.GetInstance<AdminDAO>();
                var result = dal.GetUserGroupPosition(adminId);
                if (result.Rows.Count > 0)
                {
                    //有组织-岗位(角色)对应信息
                    SetGroupPosition(result, adminId);
                }
                else
                {
                    var list = ObjectFactory.GetInstance<GroupBLL>().GetGroupByUser(adminId);
                    //只获取组织信息
                    SetGroup(list, adminId);
                }
            }
        }

        /// <summary>
        /// 设置默认的组织及岗位
        /// </summary>
        /// <param name="result"></param>
        /// <param name="adminId"></param>
        private void SetGroupPosition(DataTable result, string adminId)
        {
            var dr = result.Rows[0];
            if (dr["GroupId"] != DBNull.Value && dr["GroupName"] != DBNull.Value)
            {
                var groupId = dr["GroupId"].ToString();
                var groupName = dr["GroupName"].ToString();
                usrcfgDao.UpdateUserConfigs(adminId, AdminDAO.DefaultGroupTag, groupId);
                WebContext.CurrentUser.CurrGroupId = groupId;
                WebContext.CurrentUser.CurrGroupName = groupName;
            }
            if (dr["PositionId"] != DBNull.Value && dr["PositionName"] != DBNull.Value)
            {
                var positionId = dr["PositionId"].ToString();
                var positionName = dr["PositionName"].ToString();
                usrcfgDao.UpdateUserConfigs(adminId, AdminDAO.DefaultGroupPositionTag, positionId);
                WebContext.CurrentUser.CurrPositionId = positionId;
                WebContext.CurrentUser.CurrPositionName = positionName;
            }
            if (dr["RoleId"] != DBNull.Value && dr["RoleName"] != DBNull.Value)
            {
                var roleId = dr["RoleId"].ToString();
                var roleName = dr["RoleName"].ToString();
                usrcfgDao.UpdateUserConfigs(adminId, AdminDAO.DefaultRoleTag, roleId);
                WebContext.CurrentUser.CurrRoleId = roleId;
                WebContext.CurrentUser.CurrRoleName = roleName;
            }
        }

        /// <summary>
        /// 设置默认的组织
        /// </summary>
        /// <param name="result"></param>
        /// <param name="adminId"></param>
        private void SetGroup(List<Group> result, string adminId)
        {
            if (result.Count > 0)
            {
                var group = result[0];
                var groupId = group.GroupID.ToString();
                var groupName = group.GroupName;
                usrcfgDao.UpdateUserConfigs(adminId, AdminDAO.DefaultGroupTag, groupId);
                WebContext.CurrentUser.CurrGroupId = groupId;
                WebContext.CurrentUser.CurrGroupName = groupName;
            }
        }

        public List<Module> GetMenuData()
        {
            if (WebContext.CurrentUser == null || string.IsNullOrEmpty(WebContext.CurrentUser.UniqueId))
            {
                ModelState.AddModelError("Error", "会话过期");
                return null;
            }

            //获取每个子权限的父权限  
            var mbll = ObjectFactory.GetInstance<ModuleBLL>();
            var funcbll = ObjectFactory.GetInstance<FuncPermissionBLL>();
            List<UserFuncPMResult> modulelist = null;
            if (string.IsNullOrEmpty(WebContext.CurrentUser.CurrRoleId))
            {
                modulelist = funcbll.GetSystemFuncPermission(WebContext.CurrentUser.UniqueId,
                       WebSiteConfig.Config.SystemNo);
            }
            else {
                //如果有当前角色，则使用当前角色获取权限信息
                modulelist = funcbll.GetSystemFuncPermission(WebContext.CurrentUser.UniqueId, WebContext.CurrentUser.CurrRoleId,
                       WebSiteConfig.Config.SystemNo);
            }

            Helper.CurrUserPermission = modulelist;

            var modulecodes = new List<int>();
            if (modulelist.Count == 0) {
                ModelState.AddModelError("Error", "无任何可访问的模块,请检查website.config的SystemNo是否存在于数据库");
                return null;
            }

            foreach (var module in modulelist.Where(module => !modulecodes.Contains(module.ModuleID)))
            {
                modulecodes.Add(module.ModuleID);
            }

            //临时存储
            ViewData["ModuleItems"] = modulecodes;

            //删除无用节点
            List<Module> roots = mbll.GetEnabledModuleTree().GetRoot();

            if (WebContext.CurrentUser.UniqueId != string.Empty && SystemVar.AdminId
                != WebContext.CurrentUser.UniqueId)
            {               
                //移除非根节点的节点信息
                roots.RemoveAll(it => modulecodes.Contains(it.ModuleID) && it.ParentId != "1");
            }

            roots.Sort();

            return roots;
        }

        /// <summary>
        /// 获取用户设置的快捷方式
        /// </summary>
        /// <returns></returns>
        private List<UserConfig> GetMemoData()
        {
            var dao = ObjectFactory.GetInstance<UserConfigDAO>();

            return dao.GetUserConfigs(WebContext.CurrentUser.UniqueId, "Memo");
        }
        
        public JsonResult GetFirstTree() {
            var lst = GetMenuData();
            return Json(lst);
        }

        /// <summary>
        /// 检查是否显示在菜单的功能代码
        /// </summary>
        static readonly string[] CheckModuleMenuFuncCodes = new string[3] { "Browse", "Menu", "List" };
        public JsonResult GetTreeByEasyui(string id)
        {
            try
            {
                ModuleBLL mbll = ObjectFactory.GetInstance<ModuleBLL>();
                var modules = mbll.GetEnabledModules();
                
                var userId = WebContext.CurrentUser.UniqueId;
                if (userId != string.Empty && SystemVar.AdminId!= userId)
                {
                    var nodelst = GetChildrendNodes(modules,id,true);
                    return Json(nodelst);
                }
                else
                {
                    var nodelst = GetChildrendNodes(modules, id, false);
                    return Json(nodelst);
                }
            }
            catch (Exception ex)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }
        }

        private List<SysModuleNavModel> GetChildrendNodes(List<Module> modules,string parentId,bool checkprivilege)
        {
            var lst = modules.Where(o => o.ParentId == parentId);

            var nodelist = new List<SysModuleNavModel>();
            foreach (var it in lst)
            {
                //验证是否去具有Broswer或是Menu权限才显示内容
                var children = modules.FindAll(o => o.ParentId == it.Id);
                if (checkprivilege==false || children.Count > 0 || (checkprivilege && children.Count == 0 &&
                    Helper.CurrUserPermission.Exists(x => x.ModuleCode == it.ModuleCode
                                        && CheckModuleMenuFuncCodes.Contains(x.FunctionCode))))

                {
                    SysModuleNavModel model = new SysModuleNavModel();
                    model.id = it.Id;
                    model.text = it.ModuleName;
                    model.attributes = it.Target.Replace("~","");
                    model.iconCls = it.ImageUrl;
                    if (children.Count > 0)
                    {
                        model.state = "closed";
                    }
                    else
                    {
                        model.state = "open";
                    }
                    nodelist.Add(model);
                }
            }
            return nodelist;
        }
        #endregion

        #region memo view

        [AdminErrorHandle]
        public ActionResult MemoSetting()
        {
            InitUserMessage();

            return View();
        }

        [AdminErrorHandle]
        public ActionResult MemoSettingList()
        {
            var dao = ObjectFactory.GetInstance<UserConfigDAO>();
            var list = dao.GetUserConfigs(WebContext.CurrentUser.UniqueId, "Memo");
            return Content(Helper.GetListJsonStr(list, list.Count));
        }

        [AdminErrorHandle]
        public ActionResult SaveMomoSetting(string titles, string targets)
        {
            InitUserMessage();

            //分解
            string[] mTitles = StringHelper.SplitString(titles.TrimEnd(new[] {','}), ",");
            string[] mTargets = StringHelper.SplitString(targets.TrimEnd(new[] {','}), ",");

            if (mTitles.Length == mTargets.Length)
            {
                var dao = ObjectFactory.GetInstance<UserConfigDAO>();
                dao.SaveUserConfigs(WebContext.CurrentUser.UniqueId, "Memo", mTitles, mTargets);
            }
            return View("MemoSetting");
        }

        #endregion
    }
}