using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using Clover.Config;
using Clover.Config.FileUpload;
using Clover.Core.Common;
using Clover.Core.Logging;
using Clover.Core.XCrypt;
using Clover.Permission;
using Clover.Web.Core;
using StructureMap;
using UkeyTech.WebFW.DAO;
using UkeyTech.OA.WebApp.Extenstion;
using Clover.Permission.BLL;
using System.Web;
using Clover.Net.Mail;
using System.Configuration;
namespace UkeyTech.OA.WebApp.Areas.Admin.Controllers
{
    /// <summary>
    /// 管理员账号相关控制
    /// </summary>
    public class AccountController : AdminBaseController
    {
        #region 登录 & 登出

        private readonly AdminDAO dal = ObjectFactory.GetInstance<AdminDAO>();

        /// <summary>
        /// 是否记住用户名的cookie标记
        /// </summary>
        private string remberAdminIdFlag
        {
            get
            {
                return WebSiteConfig.Config.SystemNo + "_Admin";
            }
        }


        /// <summary>
        /// 是否全屏的cookie标记
        /// </summary>
        private string remberFullScreenFlag
        {
            get
            {
                return WebSiteConfig.Config.SystemNo + "_Admin_fullscreen";
            }
        }

        /// <summary>
        /// 检查是否需要更改密码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private bool NeedChangePassword(UkeyTech.WebFW.Model.Admin model)
        {
            return Clover.Config.WebSiteConfig.Config.UsePassWordStrategy && (model.PasswordLastUpdateTime == null || DateTime.Now.Subtract(model.PasswordLastUpdateTime.Value).Days
                          > Clover.Config.WebSiteConfig.Config.ChangePasswordPeriod);
        }
        [HttpGet]
        public ActionResult Login()
        {
            string message;

            if (!WebSiteConfig.CheckAdminIpAccess(out message))
            {
                Response.Write(message);
                Response.End();
            }

            string userid = CookieHelper.GetCookieValue(remberAdminIdFlag);
            ViewData["rememberMe"] = userid != string.Empty;
            ViewData["PUserId"] = userid;
            ViewData["returnUrl"] = Utility.GetParm("returnUrl", "");

            string openwithfullscreen = CookieHelper.GetCookieValue(remberFullScreenFlag);
            ViewData["IsFullScreen"] = openwithfullscreen != string.Empty;

            return View();
        }

        [HttpPost]
        [AdminErrorHandle(ErrorMessage = "服务器在处理登录时发生错误", ErrorName = "登录失败")]
        public ActionResult ChangeUser(string UserId, string Password, string returnUrl)
        {
            if (!WebContext.LoggedIn)
                return Content("你未登录,无法切换用户");

            //验证通过

            var admin = dal.GetFullInfoByLoginName(Utility.formatParam(UserId));

            if (admin == null)
            {
                return Content("该账号不存在或者账号无效");
            }
            if (String.Compare(admin.Password,
                               XCryptEngine.Current().Encrypt(Password),
                               StringComparison.Ordinal) != 0)
            {
                return Content("账号密码无效");
            }
            SystemBase.OnlineStaffList.Remove(WebContext.CurrentUser.UniqueId);

            var webcontext = ObjectFactory.GetInstance<IWebContext>();

            webcontext.CurrentUser = admin;

            //在线列表加入
            SystemBase.OnlineStaffList.Add(webcontext.CurrentUser.UniqueId);

            OPLogDAO.Log(webcontext.CurrentUser, Utility.GetViewerIP(), "后台登录操作",
                         webcontext.CurrentUser.UniqueId + ";" + webcontext.CurrentUser.UserName);

            //设置验证Cookie
            FormsAuthentication.SetAuthCookie(UserId, false);

            return Content("");
        }

        [HttpPost]
        [AdminErrorHandle(ErrorMessage = "服务器在处理登录时发生错误", ErrorName = "登录失败")]
        public ActionResult Login(string UserId, string Password, bool? rememberMe, string VCode, bool? IsFullScreen, string returnUrl)
        {
            ViewData["rememberMe"] = rememberMe;
            ViewData["IsFullScreen"] = IsFullScreen;
            ViewData["PUserId"] = UserId;
            UkeyTech.WebFW.Model.Admin model = null;
            if (!ValidateLogin(UserId, Password, VCode, out model))
            {
                return View();
            }

            //验证通过
            //设置验证Cookie
            FormsAuthentication.SetAuthCookie(UserId, false);

            var adminidflag = remberAdminIdFlag;
            //记住用户名
            if (rememberMe.HasValue && rememberMe.Value)
            {
                CookieHelper.CreateCookieValue(adminidflag, UserId, DateTime.Now.AddDays(30));
            }
            else
            {
                CookieHelper.RemoveCookieValue(adminidflag); //不记住密码则删除cookie
            }

            var fullscreenflag = remberFullScreenFlag;
            //是否全屏
            if (IsFullScreen.HasValue && IsFullScreen.Value)
            {
                CookieHelper.CreateCookieValue(fullscreenflag, UserId, DateTime.Now.AddDays(30));
            }
            else
            {
                CookieHelper.RemoveCookieValue(fullscreenflag); //不记住则删除cookie
            }

            DelTempFiles(); //del temp files

            //是否需要更改密码
            var neechangePwd = NeedChangePassword(model);

            if (!IsFullScreen.HasValue || !IsFullScreen.Value)
            {
                //需要更改密码
                if (neechangePwd)
                {
                    return RedirectToAction("ChangePassword", "Account", new { welcome = "1" });
                }

                if (!String.IsNullOrEmpty(returnUrl))
                {
                    return RedirectToAction("Index", "Home", new { url = returnUrl });
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                //需要更改密码
                if (neechangePwd)
                {
                    ViewData["Reidirection"] = "fullscreen('" + Url.Action("ChangePassword", "Account") + "'?welcome=1);";
                }
                else
                {
                    ViewData["Reidirection"] = "fullscreen('" + Url.Action("Index", "Home") + "');";
                }
                return View();
            }
        }

        private bool ValidateLogin(string UserId, string Password, string VCode, out UkeyTech.WebFW.Model.Admin admin)
        {
            admin = null;
            if (String.IsNullOrEmpty(UserId))
            {
                ModelState.AddModelError("userid", "你必须输入用户名");
            }
            if (String.IsNullOrEmpty(Password))
            {
                ModelState.AddModelError("password", "你必须输入密码");
            }
            else
            {
                //数据库验证

                admin = dal.GetSimpleByLoginName(Utility.formatParam(UserId));

                if (admin == null)
                {
                    ModelState.AddModelError("userid", "该账号不存在或者账号无效");
                }
                else if (admin.Status != 1)
                {
                    ModelState.AddModelError("userid", "该账号已无效");
                }
                else if (String.Compare(admin.Password,
                                        XCryptEngine.Current().Encrypt(Password),
                                        StringComparison.Ordinal) != 0)
                {
                    ModelState.AddModelError("password", "账号密码无效");
                }
                else
                {
                    var webcontext = ObjectFactory.GetInstance<IWebContext>();

                    //获取完整的用户信息
                    admin = dal.GetFullInfoByLoginName(Utility.formatParam(UserId));

                    webcontext.CurrentUser = admin;
                    //在线列表加入
                    SystemBase.OnlineStaffList.Add(webcontext.CurrentUser.UniqueId);

                    OPLogDAO.Log(webcontext.CurrentUser, Utility.GetViewerIP(), "后台登录操作",
                                 webcontext.CurrentUser.UniqueId + ";" + webcontext.CurrentUser.UserName);




                }
            }

            return ModelState.IsValid;
        }

        /// <summary>
        /// 删除已经经过2前的临时文件夹
        /// </summary>
        private void DelTempFiles()
        {
            var templist = new List<string>();

            var finfos = FileUploadConfig.Config;

            foreach (FilesUploadInfo finfo in finfos.FilesUploadInfo)
            {
                string tempdir = Utility.ConvertPsyPath(finfo.TempPath);
                if (!templist.Contains(tempdir))
                    templist.Add(tempdir);
            }


            foreach (string dir in templist)
            {
                //可能不存在该目录
                if (!Directory.Exists(dir))
                    continue;

                string[] tempdirs = Directory.GetDirectories(dir);

                foreach (string cdir in tempdirs)
                {
                    var dirinfo = new DirectoryInfo(cdir);
                    if (dirinfo.LastWriteTime >= DateTime.Now.AddDays(-2)) continue;
                    try
                    {
                        Directory.Delete(cdir, true);
                    }
                    catch (Exception ex)
                    {
                        LogCentral.Current.Error("删除临时文件夹[" + dir + "]发生系统错误", ex);
                    }
                }
            }
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        [AdminErrorHandle]
        public ActionResult Logout()
        {
            //session清空
            //当前用户列表移除
            if (WebContext.CurrentUser != null)
            {
                Helper.CleanUserPermission();
                OPLogDAO.Log(WebContext.CurrentUser, Utility.GetViewerIP(), "登出操作", null);
                SystemBase.OnlineStaffList.Remove(WebContext.CurrentUser.UniqueId);
            }

            WebContext.CurrentUser = null;

            if (HttpContext.Session != null)
                HttpContext.Session.Abandon();

            FormsAuthentication.SignOut();
            //跳转会登录界面
            return RedirectToAction("Login", "Account");
        }

        #endregion

        #region 密码修改

        private bool ValidatePassword(string OldPwd, string NewPwd, string ConfirmPwd, bool showoldpassword, UkeyTech.WebFW.Model.Admin model)
        {

            if (String.IsNullOrEmpty(OldPwd) && showoldpassword)
            {
                ModelState.AddModelError("OldPwd", "请填写旧密码");
            }
            if (String.IsNullOrEmpty(NewPwd))
            {
                ModelState.AddModelError("NewPwd", "请填写新密码");
            }

            if (!string.IsNullOrEmpty(NewPwd) && !NewPwd.Equals(ConfirmPwd))
            {
                ModelState.AddModelError("ConfirmPwd", "新密码与确认密码不一致");
            }

            string validPwdmsg = Clover.Config.WebSiteConfig.Config.PasswordNotMatchMessage;
            if (Clover.Config.WebSiteConfig.Config.UsePassWordStrategy && !Helper.CheckPasswordComplexity(NewPwd))
            {
                ModelState.AddModelError("NewPwd", "密码不符合密码策略，必须符合：" + validPwdmsg);
            }

            if (Clover.Config.WebSiteConfig.Config.UsePassWordStrategy && NewPwd.Equals(Clover.Core.XCrypt.XCryptEngine.Current().Decrypt(model.Password)))
            {
                ModelState.AddModelError("NewPwd", "密码策略不允许新密码与旧密码一致");
            }

            return ModelState.IsValid;
        }

        [HttpPost]
        [AdminErrorHandle]
        public ActionResult ChangePassword(string id, string OldPwd, string NewPwd, string ConfirmPwd)
        {
            if (WebContext.CurrentUser == null)
            {
                return Helper.ForbiddenView("密码修改",
                                            Request.Url != null ? Request.Url.PathAndQuery : "");
            }
            TempData.Keep("NavToHome");
            TempData.Keep("ShowOldPassword");
            var showoldpassword = (bool)TempData["ShowOldPassword"];
            var navtohome = (bool)TempData["NavToHome"];

            string changePWDUserId = WebContext.CurrentUser.UniqueId;
            WebFW.Model.Admin model = null;

            if (!string.IsNullOrEmpty(id))
            {
                model = dal.GetModel(id);

                if (model != null && showoldpassword &&
                    WebContext.CurrentUser.UniqueId != model.AdminId && changePWDUserId != SystemVar.AdminId)
                {
                    //修改密码的用户不是当前用户，且用户不是超级管理员
                    ModelState.AddModelError("NewPwd", "你不是超级管理员,你无权更改别人的密码");
                    return View();
                }

                changePWDUserId = id;
            }
            else
            {
                //更改自己的密码
                model = dal.GetModel(changePWDUserId);
            }

            //初始化密码修改的界面
            InitPasswordUpdate(model.AdminId, showoldpassword);

            if (ValidatePassword(OldPwd, NewPwd, ConfirmPwd, showoldpassword, model))
            {
                //修改密码
                if (showoldpassword && Clover.Core.XCrypt.XCryptEngine.Current().Encrypt(OldPwd) != model.Password)
                {
                    ModelState.AddModelError("OldPwd", "旧密码不正确，请重新输入");

                    return View();
                }

                if (model != null)
                {
                    try
                    {
                        dal.UpdatePassword(changePWDUserId, XCryptEngine.Current().Encrypt(NewPwd));

                        OPLogDAO.Log(WebContext.CurrentUser, Utility.GetViewerIP(), "用户密码修改操作:", model.AdminName);

                        if (!string.IsNullOrEmpty(id))
                        {
                            return RefreshParentAndCloseFrame();
                        }
                        else
                        {
                            if (navtohome)
                                ShowMessage("提示", "密码修改成功", Url.Action("Index", "Home"));
                            else
                                ShowMessageAndClose("提示", "密码修改成功", 2);
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowError("修改密码失败");

                        LogCentral.Current.Error("密码修改失败发生系统错误", ex);
                        return View();
                    }
                }
            }
            return View();
        }

        private void InitPasswordUpdate(string id, bool alterMyPassword)
        {
            TempData["ShowOldPassword"] = true;
            TempData.Keep("ShowOldPassword");
            WebFW.Model.Admin model = null;
            if (!alterMyPassword && !string.IsNullOrEmpty(id))
            {
                model = dal.GetModel(id);
                if (model != null)
                    TempData["StateText"] = "你当前修改的是[" + model.AdminName + "]的密码";

                TempData["ShowOldPassword"] = false;
            }
            else
            {
                model = dal.GetModel(WebContext.CurrentUser.UniqueId);
                TempData["StateText"] = "你当前修改的是自己的密码";
            }

            var neechangePwd = NeedChangePassword(model);
            if (neechangePwd)
            {
                TempData["ShowOldPassword"] = true;
                TempData["StateText"] = (TempData["StateText"] ?? "") + ",<br/>";
                if (model.PasswordLastUpdateTime == null)
                {
                    TempData["StateText"] += "第一次登陆，请更改你的初始密码。";

                }
                else
                {
                    TempData["StateText"] += string.Format(@"距离上次密码的修改时间已经过{0}天，大于系统要求的密码更改周期{1}天，请更改你的密码。",
                   (model.PasswordLastUpdateTime != null ? DateTime.Now.Subtract(model.PasswordLastUpdateTime.Value).Days.ToString() : "很多"),
                   Clover.Config.WebSiteConfig.Config.ChangePasswordPeriod);
                }
            }
        }

        [HttpGet]
        public ActionResult ChangePassword(string id)
        {
            if (WebContext.CurrentUser == null)
            {
                return Helper.ForbiddenView("密码修改",
                                            Request.Url != null ? Request.Url.PathAndQuery : "");
            }

            TempData["NavToHome"] = Request["welcome"] == "1";
            TempData.Keep("NavToHome");
            InitPasswordUpdate(id, string.IsNullOrEmpty(id) || id.Equals(WebContext.CurrentUser.UniqueId));

            return View();
        }

        #endregion

        /// <summary>
        /// 更新密码
        /// </summary>
        /// <param name="UserPwd"></param>
        /// <param name="NewPwd"></param>
        /// <param name="ConfirmPwd"></param>
        /// <returns></returns>
        public ActionResult UpdatePwd(string UserPwd, string NewPwd, string ConfirmPwd)
        {
            try
            {
                string result = "";
                string changePWDUserId = WebContext.CurrentUser.UniqueId;

                //更改自己的密码
                var model = dal.GetModel(changePWDUserId);
                if (XCryptEngine.Current().Encrypt(UserPwd) == model.Password)
                {
                    dal.UpdatePassword(changePWDUserId, XCryptEngine.Current().Encrypt(NewPwd));
                    result = "{\"msg\":\"修改成功，请重新登录！\",\"success\":true}";
                }
                else
                {
                    result = "{\"msg\":\"原密码不正确！\",\"success\":false}";
                }
                return Content(result);
            }
            catch (Exception ex)
            {
                return Content("{\"msg\":\"修改失败," + ex.Message + "\",\"success\":false}");
            }
        }

        #region 管理员信息管理

        [CloverAuthorize(FuncCode = Consts.Browse, FuncName = "浏览用户资料")]
        public ActionResult GetNoCurrentAccountList(string CodeOrName)
        {
            int rowscount = 0;

            string where = string.Format("(LoginName like '%{0}%' or AdminName like '%{0}%') AND Status=1 AND AdminId <> '{1}'",
                              CodeOrName, WebContext.CurrentUser.UniqueId);

            var result = dal.GetAllPaged(PageSize, PageIndex, where, true, out rowscount);

            return Content(Helper.GetListJsonStr(result, rowscount));
        }

        [HttpPost]
        [CloverAuthorize(FuncCode = Consts.Browse, FuncName = "浏览用户资料")]
        public ActionResult GetAccountList(string loginid, string adminname, string GroupId)
        {
            int rowscount = 0;
            string where = @"1=1 AND (AdminName like '%" + (adminname ?? "") + "%' or LoginName like '%" + adminname + @"%')
                    {? AND LoginName like '%$loginid$%'} ";
            if (!string.IsNullOrEmpty(GroupId))
                where += " AND exists (SELECT 1 FROM dbo.CPM_Group_User WHERE GroupID = " + GroupId + " AND CPM_Group_User.UserID = v_sys_admin.AdminId)";
            var result = dal.GetAllPaged(PageSize, PageIndex,
                Clover.Data.BaseDAO.ParseSQLCommand(WebContext, where), true, out rowscount);

            return Content(Helper.GetListJsonStr(result, rowscount));
        }

        [CloverAuthorize(FuncCode = Consts.Browse, FuncName = "浏览用户资料")]
        public ActionResult Index()
        {
            return View();
        }

        private bool ValidateCreate(WebFW.Model.Admin model, string ConfirmPwd)
        {
            bool validateedit = ValidateEdit(model);

            if (String.IsNullOrEmpty(model.Password))
            {
                ModelState.AddModelError("Password", "请填写密码");
            }

            if (!String.IsNullOrEmpty(model.Password) && !model.Password.Equals(ConfirmPwd))
            {
                ModelState.AddModelError("ConfirmPwd", "密码与确认密码不一致");
            }


            if (ModelState.IsValid && !string.IsNullOrEmpty(model.Email) && !string.IsNullOrEmpty(model.EmailPwd))
            {
                string smtpserver = ConfigurationManager.AppSettings["SMTPSvr"];
                if (!string.IsNullOrEmpty(smtpserver))
                {
                    string user = model.Email;
                    string pwd = Clover.Core.XCrypt.XCryptEngine.Current().Decrypt(model.EmailPwd);
                    MailSmtpClient client = new MailSmtpClient(smtpserver, user, pwd);
                    var authrst = false;
                    try
                    {
                        authrst = client.Authenticate();
                    }
                    catch { }
                    if (!authrst)
                        ModelState.AddModelError("Email", "邮箱地址与密码不符");
                }
            }

            if (ModelState.IsValid && dal.CheckExistsSameID(model.LoginName, model.UniqueId))
            {
                ModelState.AddModelError("LoginName", "登录名已经存在，请输入另外一个");
            }

            //映射账号检查
            UkeyTech.WebFW.Model.Admin rst = null;
            if (ModelState.IsValid && dal.CheckIsExistsSameMappingAccount(model.MappingAccount, model.UniqueId, out rst))
            {
                ModelState.AddModelError("MappingAccount", "该映射账号已被用户：" + model.AdminName + "[" + model.LoginName + "]" + "使用，请输入另外一个");
            }

            //密码验证
            string validPwdmsg = Clover.Config.WebSiteConfig.Config.PasswordNotMatchMessage;
            if (Clover.Config.WebSiteConfig.Config.UsePassWordStrategy && !Helper.CheckPasswordComplexity(model.Password))
            {
                ModelState.AddModelError("Password", "密码不符合密码策略，必须符合：" + validPwdmsg);
            }

            return ModelState.IsValid;
        }

        //
        // GET: /Admin/Account/Create
        [CloverAuthorize(FuncName = "创建用户", FuncCode = Consts.Create)]
        public ActionResult Create(string GroupId)
        {
            var model = new WebFW.Model.Admin();
            ViewData["AdminGroupIds"] = GroupId;
            return View("EDIT", model);
        }

        //
        // POST: /Admin/Account/Create
        [HttpPost]
        [CloverAuthorize(FuncName = "创建用户", FuncCode = Consts.Create)]
        public ActionResult Create(WebFW.Model.Admin model, string ConfirmPwd)
        {
            if (ValidateCreate(model, ConfirmPwd))
            {
                try
                {
                    model.Password = XCryptEngine.Current().Encrypt(model.Password);
                    model.AdminId = Guid.NewGuid().ToString();
                    model.IP = Utility.GetViewerIP();
                    model.EmailPwd = XCryptEngine.Current().Encrypt(model.EmailPwd);

                    dal.Insert(model);
                    //更新相关的角色、组织、岗位
                    string role = Request["AdminRoleIds"];
                    string group = Request["AdminGroupIds"];
                    string pos = Request["AdminPosIds"];

                    var roledao = ObjectFactory.GetInstance<RoleUserBLL>();
                    var groupdao = ObjectFactory.GetInstance<GroupUserBLL>();
                    var posdao = ObjectFactory.GetInstance<PositionUserBLL>();

                    var groups = group.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    var groupIds = Array.ConvertAll<string, int>(groups, new Converter<string, int>(StrToInt));
                    groupdao.InsertUserGroups(model.AdminId, groupIds);

                    var roles = role.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    var roleIds = Array.ConvertAll<string, int>(roles, new Converter<string, int>(StrToInt));
                    roledao.InsertUserRoles(model.AdminId, roleIds);


                    //var positions = pos.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    //var positionIds = Array.ConvertAll<string, int>(positions, new Converter<string, int>(StrToInt));
                    //posdao.InsertUserPosition(m.AdminId, positionIds);




                    //日志记录
                    OPLogDAO.Log(WebContext.CurrentUser, Utility.GetViewerIP(), " 添加操作员操作:", model);

                    return RefreshParentAndCloseFrame();
                }
                catch (Exception ex)
                {
                    ShowError("创建失败");
                    LogCentral.Current.Error("管理员创建失败", ex);
                    return View("Edit", model);
                }
            }

            return View("EDIT", model);
        }


        private bool ValidateEdit(WebFW.Model.Admin model)
        {
            if (String.IsNullOrEmpty(model.AdminName))
            {
                ModelState.AddModelError("AdminName", "请填写管理员姓名");
            }

            if (String.IsNullOrEmpty(model.LoginName))
            {
                ModelState.AddModelError("LoginName", "请填写登录名");
            }

            if (ModelState.IsValid && dal.CheckExistsSameID(model.LoginName, model.UniqueId))
            {
                ModelState.AddModelError("LoginName", "该登录名已经存在，请输入另外一个");
            }

            if (ModelState.IsValid && !string.IsNullOrEmpty(model.Email) && !string.IsNullOrEmpty(model.EmailPwd))
            {
                string smtpserver = ConfigurationManager.AppSettings["SMTPSvr"];

                if (!string.IsNullOrEmpty(smtpserver))
                {
                    string user = model.Email;
                    string pwd = model.EmailPwd;
                    MailSmtpClient client = new MailSmtpClient(smtpserver, user, pwd);
                    var authrst = false;
                    try
                    {
                        authrst = client.Authenticate();
                    }
                    catch { }
                    if (!authrst)
                        ModelState.AddModelError("Email", "邮箱地址与密码不符");
                }
            }

            //映射账号检查
            UkeyTech.WebFW.Model.Admin rst = null;
            if (ModelState.IsValid && dal.CheckIsExistsSameMappingAccount(model.MappingAccount, model.UniqueId, out rst))
            {
                ModelState.AddModelError("MappingAccount", "该映射账号已被用户：" + model.AdminName + "[" + model.LoginName + "]" + "使用，请输入另外一个");
            }

            return ModelState.IsValid;
        }

        //
        // GET: /Admin/Account/Edit/5
        [CloverAuthorize(FuncCode = Consts.Edit, FuncName = "编辑用户资料")]
        public ActionResult Edit(string id)
        {
            WebFW.Model.Admin model = null;
            IsEdit = true;

            if (!string.IsNullOrEmpty(id))
            {
                model = dal.GetModel(id);
            }
            else
            {
                model = dal.GetModel(WebContext.CurrentUser.UniqueId);
            }
            model.EmailPwd = Clover.Core.XCrypt.XCryptEngine.Current().Decrypt(model.EmailPwd);
            InitAdminAttributes(model.AdminId);
            return View(model);
        }

        private void InitAdminAttributes(string adminid)
        {
            var roledao = ObjectFactory.GetInstance<RoleBLL>();
            var groupdao = ObjectFactory.GetInstance<GroupBLL>();
            var posdao = ObjectFactory.GetInstance<PositionBLL>();

            var roles = roledao.GetRoleByUser(adminid);
            var groups = groupdao.GetGroupByUser(adminid);
            var poss = posdao.GetPositionByUser(adminid);

            var rolesSB = new StringBuilder();
            var groupsSB = new StringBuilder();
            var posSB = new StringBuilder();

            foreach (var role in roles)
            {
                rolesSB.Append(role.Id);
                rolesSB.Append(",");
            }

            foreach (var group in groups)
            {
                groupsSB.Append(group.Id);
                groupsSB.Append(",");
            }
            /*
           foreach (var pos in poss)
           {
               posSB.Append(pos.Id);
               posSB.Append(",");
           }*/

            ViewData["AdminRoleIds"] = rolesSB.ToString().Trim(',');
            ViewData["AdminGroupIds"] = groupsSB.ToString().Trim(',');
            //ViewData["AdminPosIds"] = posSB.ToString().Trim(',');
        }

        //
        // POST: /Admin/Account/Edit/5
        [HttpPost]
        [CloverAuthorize(FuncCode = Consts.Edit, FuncName = "编辑用户资料")]
        public ActionResult Edit(string id, WebFW.Model.Admin model)
        {

            ViewData["AdminRoleIds"] = Request["AdminRoleIds"];
            ViewData["AdminGroupIds"] = Request["AdminGroupIds"];

            if (string.IsNullOrEmpty(id))
            {
                model.AdminId = WebContext.CurrentUser.UniqueId;
            }
            else
            {
                model.AdminId = id;
            }

            IsEdit = true;

            if (ValidateEdit(model))
            {
                try
                {
                    WebFW.Model.Admin m;

                    if (!string.IsNullOrEmpty(id))
                    {
                        m = dal.GetModel(id);
                    }
                    else
                    {
                        m = dal.GetModel(WebContext.CurrentUser.UniqueId);
                    }


                    m.AdminName = model.AdminName;
                    m.LoginName = model.LoginName;
                    m.Descn = model.Descn;
                    m.Email = model.Email;
                    m.Status = model.Status;
                    m.MobilePhone = model.MobilePhone;
                    m.UsedDeptCode = model.UsedDeptCode;
                    m.BudgetDeptCode = model.BudgetDeptCode;
                    m.EmpType = model.EmpType;
                    m.Nation = model.Nation;
                    if (!string.IsNullOrEmpty(model.EmailPwd))
                        m.EmailPwd = XCryptEngine.Current().Encrypt(model.EmailPwd); ;
                    m.MappingAccount = model.MappingAccount;
                    dal.Update(m);

                    //更新相关的角色、组织、岗位

                    string role = Request["AdminRoleIds"];
                    string group = Request["AdminGroupIds"];
                    string pos = Request["AdminPosIds"];

                    var roledao = ObjectFactory.GetInstance<RoleUserBLL>();
                    var groupdao = ObjectFactory.GetInstance<GroupUserBLL>();
                    var posdao = ObjectFactory.GetInstance<PositionUserBLL>();

                    var roles = role.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    var roleIds = Array.ConvertAll<string, int>(roles, new Converter<string, int>(StrToInt));
                    roledao.DeleteUserRoles(m.AdminId);
                    roledao.InsertUserRoles(m.AdminId, roleIds);

                    var groups = group.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    var groupIds = Array.ConvertAll<string, int>(groups, new Converter<string, int>(StrToInt));
                    groupdao.DeleteUserGroups(m.AdminId);
                    groupdao.InsertUserGroups(m.AdminId, groupIds);

                    /*
                    var positions = pos.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    var positionIds = Array.ConvertAll<string, int>(positions, new Converter<string, int>(StrToInt));
                    posdao.InsertUserPosition(m.AdminId, positionIds);
                    */

                    //日志记录
                    OPLogDAO.Log(WebContext.CurrentUser, Utility.GetViewerIP(), "修改操作员操作:", model);


                    return RefreshParentAndCloseFrame();
                }
                catch (Exception ex)
                {
                    ShowError("修改失败");
                    LogCentral.Current.Error("管理员修改失败", ex);
                    return View(model);
                }
            }
            return View(model);
        }

        public static int StrToInt(string i)
        {
            return int.Parse(i);
        }

        //
        // POST: /Admin/Account/Delete/5
        [HttpPost]
        [CloverAuthorize(FuncCode = Consts.Delete, FuncName = "删除用户资料")]
        public ActionResult Delete(string delids)
        {
            try
            {
                string[] ids = StringHelper.SplitString(delids, ",");

                if (ids != null && ids.Length > 0)
                {
                    foreach (string sid in ids)
                    {
                        dal.Delete(sid);

                        //日志记录
                        OPLogDAO.Log(WebContext.CurrentUser, Utility.GetViewerIP(), "删除操作员操作:", sid);
                    }
                }
            }
            catch (Exception ex)
            {
                LogCentral.Current.Error("管理员删除失败", ex);
                return Fail("管理员删除失败");
            }
            return Success();
        }

        #endregion

        #region 用户先行编辑个人信息
        //
        // GET: /Admin/Account/Edit/5
        //[CloverAuthorize(FuncCode = Consts.Edit, FuncName = "修改用户资料")]
        public ActionResult MyInfoEdit(string id)
        {
            WebFW.Model.Admin model = dal.GetModel(WebContext.CurrentUser.UniqueId);
            model.EmailPwd = Clover.Core.XCrypt.XCryptEngine.Current().Decrypt(model.EmailPwd);
            IsEdit = true;

            return View(model);
        }

        //
        // POST: /Admin/Account/Edit/5
        [HttpPost]
        //[CloverAuthorize(FuncCode = Consts.Edit, FuncName = "修改用户资料")]
        public ActionResult MyInfoEdit(WebFW.Model.Admin model)
        {
            model.AdminId = WebContext.CurrentUser.UniqueId;
            IsEdit = true;

            if (ValidateEdit(model))
            {
                try
                {
                    WebFW.Model.Admin m = m = dal.GetModel(model.AdminId); ;

                    m.AdminName = model.AdminName;
                    m.Descn = model.Descn;
                    m.Email = model.Email;
                    m.MobilePhone = model.MobilePhone;
                    m.EmailPwd = XCryptEngine.Current().Encrypt(model.EmailPwd); ;
                    dal.Update(m);

                    //日志记录
                    OPLogDAO.Log(WebContext.CurrentUser, Utility.GetViewerIP(), "用户修改自身信息操作:", model);


                    RegisterScriptBlock("alert('修改成功');var p = GetRealParent();p.RunBackFunc();p.CloseWin();");
                }
                catch (Exception ex)
                {
                    ShowError("修改失败");
                    LogCentral.Current.Error("用户修改自身信息操作失败", ex);
                    return View(model);
                }
            }
            return View(model);
        }
        #endregion

        #region 辅助方法

        /// <summary>
        /// 获取指定ID的用户名
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetAdminName(string id)
        {
            return Content(AdminDAO.AdminIdNameList.ContainsKey(id) ? AdminDAO.AdminIdNameList[id] : "");
        }

        /// <summary>
        /// 获取指定ID的用户名（多个）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ActionResult GetAdminNames(string ids)
        {
            return Content(AdminDAO.getAdminNames(ids));
        }


        private readonly UserConfigDAO _configdal = ObjectFactory.GetInstance<UserConfigDAO>();

        /// <summary>
        /// 获取用户某个设置信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AdminErrorHandle]
        public string GetUserSetting(string code)
        {
            var list = _configdal.GetUserConfigs(WebContext.CurrentUser.UniqueId, code);

            if (list.Count > 0)
                return list[0].ConfigValue;
            else
                return "";
        }

        /// <summary>
        /// 保存用户配置信息设置
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AdminErrorHandle]
        public ActionResult SaveUserSetting(string code, string val)
        {
            try
            {
                _configdal.UpdateUserConfigs(WebContext.CurrentUser.UniqueId, code,
                                            val);
                return Success();
            }
            catch (Exception ex)
            {
                LogCentral.Current.Error("用户设置保存失败", ex);
                return Fail("用户设置保存失败");
            }
        }
        #endregion

        #region 角色岗位设置
        public ActionResult ChangeDefaultGroupPosition()
        {
            return View(WebContext.CurrentUser);
        }

        /// <summary>
        /// 获取用户的兼职信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UserGroupPositionList()
        {

            return UserGroupPositionListWithAdminId(WebContext.CurrentUser.UniqueId);
        }

        /// <summary>
        /// 获取用户的兼职信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UserGroupPositionListWithAdminId(string adminid)
        {

            if (WebContext.CurrentUser == null)
                return Content(Helper.EmptyRows);

            var result = dal.GetUserGroupPosition(adminid);

            return Content(Helper.GetDataTableJsonStr(result, result.Rows.Count));
        }
        /// <summary>
        /// 设置默认岗位
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetDefaultUserGroupPosition(string groupId, string positionId,
            string groupName, string positionName, string roleid, string rolename, bool changedefault)
        {
            if (WebContext.CurrentUser == null)
                return Content(Helper.EmptyRows);

            var usrcfgDao = ObjectFactory.GetInstance<UserConfigDAO>();
            try
            {
                if (changedefault) //变更用户的默认岗位及账号
                {
                    usrcfgDao.UpdateUserConfigs(WebContext.CurrentUser.UniqueId, AdminDAO.DefaultGroupTag, groupId);
                    usrcfgDao.UpdateUserConfigs(WebContext.CurrentUser.UniqueId, AdminDAO.DefaultGroupPositionTag, positionId);
                    usrcfgDao.UpdateUserConfigs(WebContext.CurrentUser.UniqueId, AdminDAO.DefaultRoleTag, roleid);
                }
                WebContext.CurrentUser.CurrGroupId = groupId;
                WebContext.CurrentUser.CurrGroupName = groupName;
                WebContext.CurrentUser.CurrPositionId = positionId;
                WebContext.CurrentUser.CurrPositionName = positionName;
                //默认角色
                WebContext.CurrentUser.CurrRoleId = roleid;
                WebContext.CurrentUser.CurrRoleName = rolename;

                return Success();
            }
            catch (Exception ex)
            {
                return Fail(ex.Message, "保存失败");
            }
        }

        [HttpPost]
        [CloverAuthorize(FuncCode = Consts.Edit, FuncName = "")]
        public ActionResult SetDefaultUserGroupPositionWithAdminId(string adminId, string groupId, string positionId,
            string groupName, string positionName, string roleId, string roleName, bool changedefault)
        {
            var usrcfgDao = ObjectFactory.GetInstance<UserConfigDAO>();
            try
            {
                if (changedefault) //变更用户的默认岗位及账号
                {
                    usrcfgDao.UpdateUserConfigs(adminId, AdminDAO.DefaultGroupTag, groupId);
                    usrcfgDao.UpdateUserConfigs(adminId, AdminDAO.DefaultGroupPositionTag, positionId);
                    usrcfgDao.UpdateUserConfigs(adminId, AdminDAO.DefaultRoleTag, roleId);

                }

                if (adminId.Equals(WebContext.CurrentUser.UniqueId))
                {
                    WebContext.CurrentUser.CurrGroupId = groupId;
                    WebContext.CurrentUser.CurrGroupName = groupName;
                    WebContext.CurrentUser.CurrPositionId = positionId;
                    WebContext.CurrentUser.CurrPositionName = positionName;

                    WebContext.CurrentUser.CurrRoleId = roleId;
                    WebContext.CurrentUser.CurrRoleName = roleName;

                }
                return Success();
            }
            catch (Exception ex)
            {
                return Fail(ex.Message, "保存失败");
            }
        }

        /// <summary>
        /// 添加 用户部门岗位
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="groupId"></param>
        /// <param name="positionId"></param>
        /// <param name="changeDefault">更改的是默认组织岗位的信息</param>
        /// <returns></returns>
        [HttpPost]
        [CloverAuthorize(FuncCode = Consts.Edit, FuncName = "添加用户部门岗位")]
        public ActionResult AddUserGroupPosition(string adminId, string groupId, string positionId, string roleId, bool changeDefault)
        {
            var usrcfgDao = ObjectFactory.GetInstance<UserConfigDAO>();
            try
            {
                //添加记录
                var posUserBLL = ObjectFactory.GetInstance<PositionUserBLL>();

                //if (posUserBLL.CheckUserHasGroupPosition(adminId, positionId, groupId))
                //{
                //    return Fail("记录已经存在,无需添加");
                //}
                if (ConvertHelper.IsInt32(groupId))
                {
                    int? vgroupId = int.Parse(groupId);
                    int? vroleId = null;
                    int posId = int.Parse(positionId);
                    if (!string.IsNullOrEmpty(roleId))
                    {
                        vroleId = int.Parse(roleId);
                    }
                    posUserBLL.InsertPositionUsers(int.Parse(positionId), vgroupId, vroleId, new[] { adminId });

                    //添加时检查是否有默认岗位及部门,不存在则需要添加
                    var defgroup = usrcfgDao.GetUserConfigs(adminId, AdminDAO.DefaultGroupTag);
                    if (changeDefault || defgroup.Exists(x => string.IsNullOrEmpty(x.ConfigValue)) || defgroup.Count == 0)
                        usrcfgDao.UpdateUserConfigs(adminId, AdminDAO.DefaultGroupTag, groupId);

                    var defpos = usrcfgDao.GetUserConfigs(adminId, AdminDAO.DefaultGroupPositionTag);
                    if (changeDefault || defpos.Exists(x => string.IsNullOrEmpty(x.ConfigValue)) || defpos.Count == 0)
                        usrcfgDao.UpdateUserConfigs(adminId, AdminDAO.DefaultGroupPositionTag, positionId);

                    //检查角色是否存在
                    var defrole = usrcfgDao.GetUserConfigs(adminId, AdminDAO.DefaultRoleTag);
                    if (changeDefault || defrole.Exists(x => string.IsNullOrEmpty(x.ConfigValue)) || defrole.Count == 0)
                        usrcfgDao.UpdateUserConfigs(adminId, AdminDAO.DefaultRoleTag, roleId);
                }
                else
                {
                    return Fail("无效的提交参数，请检查你的浏览器是否正常");
                }
                return Success();
            }
            catch (Exception ex)
            {
                return Fail(ex.Message, "删除失败");
            }
        }

        /// <summary>
        /// 删除 用户部门岗位
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="groupId"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>
        [HttpPost]
        [CloverAuthorize(FuncCode = Consts.Edit, FuncName = "删除用户部门岗位")]
        public ActionResult DeleteUserGroupPosition(string adminId, string groupId, string positionId, string roleId)
        {
            var usrcfgDao = ObjectFactory.GetInstance<UserConfigDAO>();
            try
            {
                //1.查找原有的配置信息
                var configs = usrcfgDao.GetUserConfigs(adminId,
                    string.Join("','", new string[3] { AdminDAO.DefaultGroupTag, AdminDAO.DefaultGroupPositionTag, AdminDAO.DefaultRoleTag }));

                //2.如果删除的内容包含在内，则置为空
                if (configs.Exists(x => x.ConfigValue == groupId && x.ConfigType == AdminDAO.DefaultGroupTag)) //变更用户的默认岗位及账号
                {
                    usrcfgDao.UpdateUserConfigs(adminId, AdminDAO.DefaultGroupTag, "");
                }
                if (configs.Exists(x => x.ConfigValue == positionId && x.ConfigType == AdminDAO.DefaultGroupPositionTag)) //变更用户的默认岗位及账号
                {
                    usrcfgDao.UpdateUserConfigs(adminId, AdminDAO.DefaultGroupPositionTag, "");
                }
                if (configs.Exists(x => x.ConfigValue == roleId && x.ConfigType == AdminDAO.DefaultRoleTag)) //变更用户的默认角色
                {
                    usrcfgDao.UpdateUserConfigs(adminId, AdminDAO.DefaultRoleTag, "");
                }
                //删除后需要默认另外一个部门岗位
                if (adminId.Equals(WebContext.CurrentUser.UniqueId))
                {
                    WebContext.CurrentUser.CurrGroupId = null;
                    WebContext.CurrentUser.CurrGroupName = string.Empty;
                    WebContext.CurrentUser.CurrPositionId = null;
                    WebContext.CurrentUser.CurrPositionName = string.Empty;
                    WebContext.CurrentUser.CurrRoleId = null;
                    WebContext.CurrentUser.CurrRoleName = string.Empty;
                }
                //删除记录
                var posUserBLL = ObjectFactory.GetInstance<PositionUserBLL>();

                if (ConvertHelper.IsInt32(groupId))
                {
                    int? vgroupId = int.Parse(groupId);
                    posUserBLL.DeletePositionUsers(int.Parse(positionId), vgroupId, new[] { adminId });
                }
                else
                {
                    return Fail("无效的提交参数，请检查你的浏览器是否正常");
                }
                return Success();
            }
            catch (Exception ex)
            {
                return Fail(ex.Message, "删除失败");
            }
        }
        #endregion
    }
}