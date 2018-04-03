using System.Web;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Web.Optimization;

namespace UkeyTech.OA.WebApp
{
    public class BundleConfig
    {
        // 有关 Bundling 的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            /*
         

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // 使用 Modernizr 的开发版本进行开发和了解信息。然后，当你做好
            // 生产准备时，请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));
            */

            // 使用 Modernizr 的开发版本进行开发和了解信息。然后，当你做好
            // 生产准备时，请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            /*
            bundles.Add(new ScriptBundle("~/bundles/base").Include(
                     "~/Scripts/jquery-1.7.2.min.js",
                     "~/Scripts/EasyUI/jquery.easyui.min.js",
                     "~/scripts/EasyUI/locale/easyui-lang-zh_CN.js",
                     "~/Scripts/common.min.js"));
             
            */

            bundles.Add(new ScriptBundle("~/bundles/attachment").Include(                                                      
                    "~/Scripts/colorbox/jquery.colorbox-min.js",
                    "~/Scripts/colorbox/i18n/jquery.colorbox-zh-CN.js"));

            bundles.Add(new StyleImagePathBundle("~/styles/attachment").Include(
               "~/Scripts/colorbox/colorbox.css"
            ));           

            bundles.Add(new ScriptBundle("~/bundles/upload").Include(
                    "~/Scripts/jquery-1.7.2.min.js",     
                    "~/Scripts/jquery-ui.min.js",
                     "~/Scripts/plupload/plupload.full.min.js",                    
                     "~/Scripts/plupload/i18n/zh_CN.js"));

            bundles.Add(new StyleImagePathBundle("~/styles/upload").Include(
               "~/Scripts/plupload/jquery.ui.plupload/css/jquery.ui.plupload.css",
               "~/Content/jquery-ui.min.css"
               ));

            string currdefaultCSS = UkeyTech.OA.WebApp.Helper.GetDefaultStyleSheet();
            for(var i = 0; i< 3; i++){
                bundles.Add(new StyleImagePathBundle("~/styles/theme/skin_" + i.ToString()).Include(
               "~/Scripts/EasyUI/themes/skin_" + i.ToString() + "/easyui.css",
               currdefaultCSS,
               "~/Scripts/EasyUI/themes/icon.css"));
            }
         
            BundleTable.EnableOptimizations = true;
        }
    }

    public class StyleImagePathBundle : Bundle
    {
        public StyleImagePathBundle(string virtualPath)
            : base(virtualPath, new IBundleTransform[1]
      {
        (IBundleTransform) new CssMinify()
      })
        {
        }

        public StyleImagePathBundle(string virtualPath, string cdnPath)
            : base(virtualPath, cdnPath, new IBundleTransform[1]
      {
        (IBundleTransform) new CssMinify()
      })
        {
        }

        public new Bundle Include(params string[] virtualPaths)
        {
           
            // In production mode so CSS will be bundled. Correct image paths.
            var bundlePaths = new List<string>();
            var svr = HttpContext.Current.Server;
            foreach (var path in virtualPaths)
            {
                var pattern = new Regex(@"url\s*\(\s*([""']?)([^:)]+)\1\s*\)", RegexOptions.IgnoreCase);
                var contents = File.ReadAllText(svr.MapPath(path));
                if (!pattern.IsMatch(contents))
                {
                    bundlePaths.Add(path);
                    continue;
                }


                var bundlePath = (System.IO.Path.GetDirectoryName(path) ?? string.Empty).Replace(@"\", "/") + "/";
                var bundleUrlPath = VirtualPathUtility.ToAbsolute(bundlePath);
                var bundleFilePath = string.Format("{0}{1}.bundle{2}",
                                                   bundlePath,
                                                   System.IO.Path.GetFileNameWithoutExtension(path),
                                                   System.IO.Path.GetExtension(path));
                contents = pattern.Replace(contents, "url($1" + bundleUrlPath + "$2$1)");
                File.WriteAllText(svr.MapPath(bundleFilePath), contents);
                bundlePaths.Add(bundleFilePath);
            }
            base.Include(bundlePaths.ToArray());
            return this;
        }

    }
}