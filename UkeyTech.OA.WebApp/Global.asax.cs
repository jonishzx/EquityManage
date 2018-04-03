using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using StructureMap;

using Clover.Core.CodeTimer;
using StackExchange.Profiling;
using UkeyTech.OA.WebApp.Extenstion;

namespace UkeyTech.OA.WebApp
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            CodeTimer.Time("测试structuremap 速度",
                    new CodeTimer.ActionDelegate(InitAll));

        }

        private void InitAll()
        {
            //IOC注入
            ObjectFactory.Initialize(delegate (IInitializationExpression x)
            {
                x.AddConfigurationFromXmlFile(Server.MapPath("~") + "\\Config\\StructureMap.config");
                //使用默认StructureMap.config
                x.UseDefaultStructureMapConfigFile = false;
            });

            //字符串
            Clover.Data.BaseDAO.OnInit();

            //自动化任务
            //Clover.Schedules.QuartzJobs jobsmanager = ObjectFactory.GetInstance<Clover.Schedules.QuartzJobs>();
            //if (jobsmanager.IsStop)
            //    jobsmanager.Start();


            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ViewEngines.Engines.Clear();//先清理视图引擎
            MyWebFormViewEngine.Config();//注册多级目录扩展
            MyRazorViewEngine.Config();
        }

        protected void Application_BeginRequest()
        {
            //调试速度
            if (Request.IsLocal)
            {
                MiniProfiler.Start();
            }
        }

        protected void Application_EndRequest()
        {
            //调试速度
            if (Request.IsLocal)
            {
                MiniProfiler.Stop();
            }
        }
        protected void Application_End()
        {
            //  在应用程序关闭时运行的代码

            //更新当前最新的在线人数到数据库
            OnlineHandler handler = OnlineListFactory.Current();
            handler.UpdateOnlineStatus();

        }
    }
}