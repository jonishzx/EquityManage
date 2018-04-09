using System.Web.Mvc;
namespace UkeyTech.OA.WebApp.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                 "Admin_default",
                 "Admin/{controller}/{action}/{id}",
                 new { action = "Index", id = UrlParameter.Optional },
                 new string[] { "UkeyTech.OA.WebApp.Areas.Admin.Controllers" }
             );
         
            ////自定义表单类型
            //ViewEngines.Engines.Add(
            //    new WebFormViewEngine()
            //    {
            //        ViewLocationFormats = new string[] 
            //        {
            //            "~/Areas/Admin/Views/CustomForm/{1}/{0}.aspx",
            //             "~/Areas/Admin/Views/CustomForm/{1}/{0}.ascx"
            //        }
            //    });
        }
    }
}
