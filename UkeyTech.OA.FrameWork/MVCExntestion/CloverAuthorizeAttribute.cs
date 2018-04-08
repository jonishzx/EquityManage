using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Clover.Permission;
using StructureMap;
using Clover.Web.Core;

namespace UkeyTech.OA.WebApp.Extenstion
{
    /// <summary>
    /// 自定义权限校验
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class CloverAuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {
        // Fields
      
        private readonly object _typeId = new object();

        private bool isAdmin = false;

        private string validateurl;
        // Methods
        protected virtual bool AuthorizeCore(HttpContextBase httpcontext)
        {
            if (httpcontext == null)
            {
                throw new ArgumentNullException("httpcontext");
            }

            if (isAdmin)
                return true;


            //存在相同的权限代码
            return bool.Parse(Helper.CheckFuncAccess(ModuleCode, FuncCode));          
        }

        private void CacheValidateHandler(HttpContext context, object data, ref HttpValidationStatus validationStatus)
        {
            validationStatus = this.OnCacheAuthorization(new HttpContextWrapper(context));
        }

        protected virtual void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var view = Helper.ForbiddenView(FuncName, filterContext.HttpContext.Request.Url.PathAndQuery);
       
            filterContext.Result = view;
        }

        public virtual void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

#if DEBUG
            /*
            IWebContext _webcontext = ObjectFactory.GetInstance<IWebContext>();


            Clover.Core.Domain.IAccount user = new UkeyTech.WebFW.Model.Admin();
            user.UniqueId = "90EC66D8-F9A3-40DC-B532-7E350BDF3169";
            user.UserName = "Mock测试管理员";
            user.AccountCode = "Admin";
            _webcontext.CurrentUser = user;
             */
#endif

            //定义验证url
            if (!string.IsNullOrEmpty(MappingToAction) && !string.IsNullOrEmpty(MappingToController))
                validateurl = "/" + filterContext.RouteData.Route.GetVirtualPath(filterContext.RequestContext,
                new System.Web.Routing.RouteValueDictionary(new { controller = MappingToController, action = MappingToAction })).VirtualPath;
            else
                validateurl = filterContext.HttpContext.Request.RawUrl;
                
            var controller = (UkeyTech.OA.WebApp.Extenstion.AdminBaseController)filterContext.Controller;

            //设置是否编辑状态
            if (FuncCode == Consts.Edit)
            {
                controller.IsEdit = true;
            }

            //控制器记录
            if (string.IsNullOrEmpty(ModuleCode))
                ModuleCode = filterContext.Controller.GetType().Name.Replace("Controller", "");

            Clover.Core.Domain.IAccount curruser =controller.WebContext.CurrentUser;
            List<Clover.Permission.Model.UserFuncPMResult> permission = controller.Permissions;

            isAdmin = curruser != null && curruser.UniqueId == Clover.Web.Core.SystemVar.AdminId;

            if (this.AuthorizeCore(filterContext.HttpContext))
            {
                HttpCachePolicyBase cache = filterContext.HttpContext.Response.Cache;
                cache.SetProxyMaxAge(new TimeSpan(0L));
                cache.AddValidationCallback(new HttpCacheValidateHandler(this.CacheValidateHandler), null);
            }
            else
            {
                //跳转自定义view
                this.HandleUnauthorizedRequest(filterContext);
            }
        }

        protected virtual HttpValidationStatus OnCacheAuthorization(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }
            if (!this.AuthorizeCore(httpContext))
            {
                return HttpValidationStatus.IgnoreThisRequest;
            }
            return HttpValidationStatus.Valid;
        }

       
        public override object TypeId
        {
            get
            {
                return this._typeId;
            }
        }


        /// <summary>
        /// 功能代码
        /// </summary>
        public string FuncCode
        {
            get;
            set;
        }

        /// <summary>
        /// 模块代码
        /// </summary>
        public string ModuleCode
        {
            get;
            set;
        }

        /// <summary>
        /// 功能名称
        /// </summary>
        public string FuncName
        {
            get;
            set;
        }

        /// <summary>
        /// 映射到的actoin名称
        /// </summary>
        public string MappingToAction
        {
            get;
            set;
        }

        /// <summary>
        /// 映射到的controller名称
        /// </summary>
        public string MappingToController
        {
            get;
            set;
        }
    }
}
