using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Clover.Core.Common;
using Clover.Core.Logging;
using Clover.Permission;
using Clover.Web.Core;
using StructureMap;
using UkeyTech.WebFW.DAO;
using UkeyTech.WebFW.Model;
using UkeyTech.OA.WebApp.Extenstion;
using System.Web.Routing;
namespace UkeyTech.OA.WebApp.Areas.Admin.Controllers
{
    /// <summary>
    /// 小部件
    /// </summary>
    public class WidgetController : AdminLoginController
    {
        private readonly UserConfigDAO configdal = ObjectFactory.GetInstance<UserConfigDAO>();
        private static readonly WidgetDAO dal = ObjectFactory.GetInstance<WidgetDAO>();

        #region 初始化

        private static volatile List<Widget> _cachedlist;
        private static readonly object Lockobj = new object();

        /// <summary>
        /// 初始化缓存列表
        /// </summary>
        public WidgetController()
        {
            InitCacheList();
        }

        private void InitCacheList() {
            InitCacheList(false);
        }

      
        /// <summary>
        /// 初始化部件列表
        /// </summary>
        public static void InitCacheList(bool refresh) {
            if (_cachedlist != null && !refresh) return;
            lock (Lockobj)
            {
                if (_cachedlist == null || refresh)
                {
                    _cachedlist = dal.GetAll("");
                }
            }
        }

        private static Widget GetWidget(int widgetid)
        {
            return _cachedlist != null ? _cachedlist.Find(x => x.WidgetID == widgetid) : null;
        }

        #endregion

        #region 各种部件参数设置

        #endregion

        #region 用户部件读取及设置

        /// <summary>
        /// 获取布局的部件信息
        /// </summary>
        /// <returns></returns>
        public string GetWidgetLayout()
        {
            var list = configdal.GetUserConfigs(WebContext.CurrentUser.UniqueId, "WidgetLayout");

            //如果用户的部件未初始化,则使用默认的部件加载
            if (list.Count == 0 || string.IsNullOrEmpty(list[0].ConfigValue))
            {
                configdal.UpdateUserConfigs(WebContext.CurrentUser.UniqueId, "WidgetLayout", ConfigurationManager.AppSettings["DefaultUserWidgetLayout"] ?? "::");
                list = configdal.GetUserConfigs(WebContext.CurrentUser.UniqueId, "WidgetLayout");
            }

            if (list.Count > 0)
                return list[0].ConfigValue;
            else
                return "::";
        }

        /// <summary>
        /// 查找用户布局中具有的部件
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetUserWidgetLayout()
        {
            return Content(GetWidgetLayout());
        }

        /// <summary>
        /// 查找用户布局中具有的部件
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetUserSelectdWidgetList()
        {
            var result = GetUserSelectdWidgetListContent(Url.Action("LoadWidget"));
            return Content(result);
        }

        /// <summary>
        /// 查找用户布局中具有的部件内容
        /// </summary>
        /// <returns></returns>
        public string GetUserSelectdWidgetListContent(string baseurl)
        {
            string[] ids = GetWidgetLayout().Split(new[] { ',', ':' }, StringSplitOptions.RemoveEmptyEntries);

            //查找用户布局中具有的项目
            var selList = new List<Widget>();
            foreach (string id in ids)
            {
                var it = _cachedlist.Find(x => id == ("p" + x.WidgetID.ToString()));
                if(it != null)
                    selList.Add(it);
            }

            // { id: 'p1', title: '天气', height: 200, collapsible: true, maximizable: true, href: 'Widget/Weather' },

            var result = new StringBuilder();
            result.Append("[");
            try
            {
                foreach (Widget m in selList)
                {
                    result.Append("{");
                    System.Web.Routing.RouteValueDictionary rvd = new System.Web.Routing.RouteValueDictionary();
                    result.AppendFormat("\"id\":\"p{0}\", \"title\":\"{1}\",{2} \"href\":\"{3}\"",
                                        m.WidgetID,
                                        m.WidgetName,
                                        (!string.IsNullOrEmpty(m.UIParamters) ? m.UIParamters + "," : ""),
                                        baseurl + "?widgetid=" + m.WidgetID + "&t=" + DateTime.Now.ToString("yyyyMMddHHmmss")
                        );
                    result.Append("},");
                }
            }
            catch (Exception ex) {
                LogCentral.Current.Error("布局有异常",ex);
            }
            //trim end ,
            if (result.Length > 1)
                result.Remove(result.Length - 1, 1);

            result.Append("]");
            return result.ToString();
        }



        //获取用户选中的部件列表
        [HttpGet]
        public ActionResult GetUserWidgetList()
        {
            return Content(Helper.GetListJsonStr(
                _cachedlist, _cachedlist.Count));
        }

        /// <summary>
        /// 获取特定ID的部件
        /// </summary>
        /// <param name="widgetid"></param>
        /// <returns></returns>
        [HttpGet]
        [AdminErrorHandle]
        public ActionResult LoadWidget(int widgetid)
        {
            Widget model = GetWidget(widgetid);


            if (!string.IsNullOrEmpty(model.Target))
            {
                try
                {
                    model.Target = Utility.ConvertAbsoulteUrl(model.Target);
                }
                catch { }
            }
            return PartialView(model.WidgetTag, model);
        }

        /// <summary>
        /// 获取特定ID的部件
        /// </summary>
        /// <param name="layout"></param>
        /// <returns></returns>
        [HttpPost]
        [AdminErrorHandle]
        public ActionResult SaveUserWidget(string layout)
        {
            try
            {
                configdal.UpdateUserConfigs(WebContext.CurrentUser.UniqueId, "WidgetLayout",
                                            layout);
            }
            catch (Exception ex)
            {
                LogCentral.Current.Error("用户部件保存失败", ex);
                return Fail("用户部件保存失败");
            }

            return Success();
        }

        /// <summary>
        /// 用户
        /// </summary>
        /// <returns></returns>
        [AdminErrorHandle]
        public ActionResult WidgetSetting()
        {
            _cachedlist = dal.GetAll("");

            //按照列加入字典
            string[] columns = GetWidgetLayout().Split(new[] {':'});
            var dict = new Dictionary<int, List<Widget>>(3);
            for (int i = 0; i < columns.Length; i++)
            {
                string[] ids = columns[i].Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
                List<Widget> selList = _cachedlist.FindAll(x => ids.Contains("p" + x.WidgetID.ToString()));
                dict.Add(i, selList);
            }
            ViewData["UserWidgetLayout"] = dict;

            return View("WidgetSetting");
        }

        /// <summary>
        /// 获取用户未选中的组件
        /// </summary>
        /// <returns></returns>
        public ActionResult GetNoSelectedWidgetList()
        {
            string[] ids = GetWidgetLayout().Split(new[] {',', ':'}, StringSplitOptions.RemoveEmptyEntries);

            var selList = _cachedlist.FindAll(x => !ids.Contains("p" + x.WidgetID.ToString()));

            return Content(Helper.GetListJsonStr(selList, selList.Count));
        }

        #endregion

        #region 部件信息管理

        /// <summary>
        /// 从目录获取小部件的view列表
        /// </summary>
        private void InitWidgetViews()
        {
            var list = new Dictionary<string, string>(3);
            if (ConfigurationManager.AppSettings["WebWidgetPath"] != null)
            {
                string path =
                    HttpContext.Server.MapPath(ConfigurationManager.AppSettings["WebWidgetPath"]);
                string findExt = ConfigurationManager.AppSettings["WebWidgetExt"];
                string[] files = Directory.GetFiles(path, findExt);

                foreach (string file in files)
                {
                    var finfo = new FileInfo(file);
                    string view = finfo.Name.Replace(finfo.Extension, "");
                    list.Add(view, view);
                }
            }

            ViewData["WidgetViews"] = list;
        }

        [CloverAuthorize(FuncCode = Consts.Browse, FuncName = "浏览小部件资料")]
        public ActionResult GetWidgetList(string CodeOrName)
        {
            int rowscount = 0;

            string where = string.Format("Status = 1 and (WidgetName like '%{0}%' or WidgetCode like '%{0}%')", CodeOrName);

            var result = dal.GetAllPaged(PageSize, PageIndex, where, true, out rowscount);

            result.ForEach(delegate(Widget x)
                               {
                                   if (!string.IsNullOrEmpty(x.Target))
                                   {
                                       try
                                       {
                                           x.Target = Utility.ConvertAbsoulteUrl(x.Target);
                                       }
                                       catch { }
                                   }
                               });
            return Content(Helper.GetListJsonStr(result, rowscount));
        }

        [CloverAuthorize(FuncCode = Consts.Browse, FuncName = "浏览小部件资料")]
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Widget/Account/Create
        [CloverAuthorize(FuncName = "创建小部件", FuncCode = Consts.Create)]
        public ActionResult Create()
        {
            var model = new Widget();
            InitWidgetViews();

            return View("EDIT", model);
        }

        //
        // POST: /Widget/Account/Create
        [HttpPost]
        [ValidateInput(false)]
        [CloverAuthorize(FuncName = "创建小部件", FuncCode = Consts.Create)]
        public ActionResult Create(Widget model)
        {
            InitWidgetViews();
            if (ValidateEdit(model))
            {
                try
                {
                    dal.Insert(model);
                    
                    InitCacheList(true);
                    
                    //日志记录
                    OPLogDAO.Log(WebContext.CurrentUser, Utility.GetViewerIP(), " 添加小部件操作:", model);
                }
                catch (Exception ex)
                {
                    ShowError("创建失败");
                    LogCentral.Current.Error("小部件创建失败", ex);
                    return View("EDIT", model);
                }

                return RefreshParentAndCloseFrame();
            }

            return View("EDIT", model);
        }


        private bool ValidateEdit(Widget model)
        {
            if (String.IsNullOrEmpty(model.WidgetCode))
            {
                ModelState.AddModelError("WidgetCode", "请填写代码");
            }

            if (String.IsNullOrEmpty(model.WidgetName))
            {
                ModelState.AddModelError("WidgetName", "请填写名称");
            }

            if (String.IsNullOrEmpty(model.WidgetTag))
            {
                ModelState.AddModelError("WidgetTag", "请选择部件所属类别");
            }

            if (String.IsNullOrEmpty(model.Target))
            {
                ModelState.AddModelError("Target", "请填写URL");
            }

            if (ModelState.IsValid && dal.CheckExistsSameID(model.WidgetName, model.WidgetID.ToString()))
            {
                ModelState.AddModelError("WidgetCode", "该代码已经存在，请输入另外一个");
            }

            return ModelState.IsValid;
        }

        //
        // GET: /Widget/Account/Edit/5
        [CloverAuthorize(FuncCode = Consts.Edit, FuncName = "编辑小部件资料")]
        public ActionResult Edit(int id)
        {
            InitWidgetViews();
            Widget model = null;
            IsEdit = false;

            if (id != 0)
            {
                IsEdit = true;
                model = dal.GetModel(id);
            }

            return View(model);
        }

        //
        // POST: /Widget/Account/Edit/5
        [HttpPost]
        [ValidateInput(false)]
        [CloverAuthorize(FuncCode = Consts.Edit, FuncName = "编辑小部件资料")]
        public ActionResult Edit(int id, Widget model)
        {
            InitWidgetViews();
            model.WidgetID = id;

            IsEdit = true;

            if (ValidateEdit(model))
            {
                try
                {
                    var m = dal.GetModel(id);
                    m.WidgetCode = model.WidgetCode;
                    m.WidgetName = model.WidgetName;
                    m.WidgetTag = model.WidgetTag;
                    m.Parameters = model.Parameters;
                    m.UIParamters = model.UIParamters;
                    m.Target = model.Target;
                    m.Descn = model.Descn;

                    dal.Update(m);

                    InitCacheList(true);

                    //日志记录
                    OPLogDAO.Log(WebContext.CurrentUser, Utility.GetViewerIP(), "修改小部件操作:", model);
                }
                catch (Exception ex)
                {
                    ShowError("修改失败");
                    LogCentral.Current.Error("小部件修改失败", ex);
                    return View(model);
                }

                return RefreshParentAndCloseFrame();
            }

            return View(model);
        }

        //
        // POST: /Widget/Account/Delete/5
        [HttpPost]
        [CloverAuthorize(FuncCode = Consts.Delete, FuncName = "删除小部件资料")]
        public ActionResult Delete(string delids)
        {
            try
            {
                string[] ids = StringHelper.SplitString(delids, ",");

                if (ids != null && ids.Length > 0)
                {
                    foreach (string sid in ids)
                    {
                        dal.Delete(int.Parse(sid));

                        //日志记录
                        OPLogDAO.Log(WebContext.CurrentUser, Utility.GetViewerIP(), "删除小部件操作:", sid);
                    }
                }
                return Success();
            }
            catch (Exception ex)
            {
                LogCentral.Current.Error("小部件修改失败", ex);
                return Fail("小部件修改失败");
            }
        }

        #endregion
    }
}