using System;
using System.Web;
using System.Web.Optimization;
using Clover.Core.Caching;
using Clover.Data;
using Clover.Permission.BLL;
using StructureMap;
using UkeyTech.OA.WebApp.Areas.Admin.Controllers;
using UkeyTech.WebFW.DAO;

namespace UkeyTech.OA.WebApp.Tools
{
    /// <summary>
    ///     Init 的摘要说明
    /// </summary>
    public class Init : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            context.Response.Write("StructureMap start init");
            //IOC注入
            ObjectFactory.Initialize(delegate(IInitializationExpression x)
            {
                x.AddConfigurationFromXmlFile(context.Server.MapPath("~") + "\\Config\\StructureMap.config");
                //使用默认StructureMap.config
                x.UseDefaultStructureMapConfigFile = false;
            });

            //字符串
            BaseDAO.OnInit();
            BaseDAO.ClearCachdQuery();

            context.Response.Write("StructureMap complete");

            //所有静态对象重新初始化
            var mbll = ObjectFactory.GetInstance<ModuleBLL>();
            mbll.InitTree();
            var gbll = ObjectFactory.GetInstance<GroupBLL>();
            gbll.InitTree();
            var rbll = ObjectFactory.GetInstance<RoleBLL>();
            rbll.InitTree();
            var dictbll = ObjectFactory.GetInstance<DictItemDAO>();
            dictbll.ClearCacheItems();

            WidgetController.InitCacheList(true);

            //自动化任务
            //var jobsmanager = ObjectFactory.GetInstance<QuartzJobs>();
            //if (jobsmanager.IsStop)
            //    jobsmanager.Start();

            //context.Response.Write("QuartzJobs init  complete");

            BundleTable.Bundles.Clear();

            BundleConfig.RegisterBundles(BundleTable.Bundles);
            try
            {
                var backer1 = ObjectFactory.GetInstance<ICacheBacker>();
                backer1.RemoveAll(string.Empty);
            }
            catch (Exception ex)
            {
                context.Response.Write("清空缓存失败:" + ex.Message);
            }

            context.Response.Write("Cache Remove complete");

            context.Response.Write("Initialize success!");

            context.Response.Write("目前连接池数" + BaseDAO.ManualConnectionPool.Count);
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}