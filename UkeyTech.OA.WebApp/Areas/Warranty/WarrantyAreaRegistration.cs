using System.Web.Mvc;

namespace UkeyTech.OA.WebApp.Areas.Warranty
{
    public class WarrantyAreaRegistration : AreaRegistration
    {
        public override string AreaName {
            get { return "Warranty"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Warranty_default",
                "Warranty/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
