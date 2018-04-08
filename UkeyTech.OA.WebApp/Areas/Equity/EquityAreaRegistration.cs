using System.Web.Mvc;

namespace UkeyTech.OA.WebApp.Areas.Equity
{
    public class EquityAreaRegistration : AreaRegistration
    {
        public override string AreaName {
            get { return "Equity"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Equity_default",
                "Equity/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
