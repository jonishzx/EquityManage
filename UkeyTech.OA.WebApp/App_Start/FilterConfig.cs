using System.Web;
using System.Web.Mvc;
using UkeyTech.OA.FrameWork.MVCExntestion;

namespace UkeyTech.OA.WebApp
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new CustomExceptionAttribute(), 1);//自定义的验证特性
            filters.Add(new HandleErrorAttribute(), 2);
        }
    }
}