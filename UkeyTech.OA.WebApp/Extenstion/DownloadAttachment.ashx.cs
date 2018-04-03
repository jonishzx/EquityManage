using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Clover.Web.Core;
using StructureMap;
using UkeyTech.WebFW.DAO;
using System.IO;
using System.Web.SessionState;

namespace UkeyTech.OA.Web.Extenstion
{
    /// <summary>
    /// 附件下载下载
    /// </summary>
    public class DownloadAttachment : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            var iwebcontext = ObjectFactory.GetInstance<IWebContext>();

            if (iwebcontext.CurrentUser == null)
            {
                context.Response.Write("你尚未登录");
                return;
            }

            context.Response.ContentType = "text/plain";
            string attachId = context.Request.QueryString["attachId"] + "";

            if (string.IsNullOrEmpty(attachId)) //尝试访问附件持久化失败时
            {
                DownLoadReuqestFile(context);
                return;
            }
            else
            {
                DownLoadAttachment(context, attachId);
            }
        }

        /// <summary>
        /// 下载数据库的相关数据
        /// </summary>
        /// <param name="context"></param>
        /// <param name="attachId"></param>
        private static void DownLoadAttachment(HttpContext context, string attachId)
        {
            AttachmentDAO _attachmentdal = ObjectFactory.GetInstance<AttachmentDAO>();
            //使用数据库的附件类实现
            var doc = _attachmentdal.GetModel(int.Parse(attachId));
            if (doc == null)
                return;
            ///读取磁盘中的文件
            if (!string.IsNullOrEmpty(doc.FilePath))
            {

                var finfo = new FileInfo(context.Server.MapPath(doc.FilePath));
                string filename = doc.Title + finfo.Extension;

                string UserAgent = context.Request.ServerVariables["http_user_agent"].ToLower();
                if (UserAgent.IndexOf("firefox") == -1)
                    filename = HttpUtility.UrlEncode(filename, System.Text.Encoding.UTF8);

                context.Response.AddHeader("Content-Length", (finfo.Length).ToString());
                context.Response.ContentType = Utility.ConvertFileType(finfo.Extension);
                //下载时对文件名进行编码,否则中文会出现乱码.
                context.Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);

                context.Response.WriteFile(finfo.FullName);
                context.Response.End();
            }
        }

        private void DownLoadReuqestFile(HttpContext context)
        {
            var sourcepath = context.Request.QueryString["filePath"] ?? "";
            string filename = context.Request.QueryString["fileName"] ?? "";
            sourcepath = HttpUtility.UrlDecode(sourcepath);
            filename = HttpUtility.UrlDecode(filename);

            if (string.IsNullOrEmpty(sourcepath))
                context.Response.Write("无效的参数");

            //默认速度200 kb/s
            string psyfilepath = context.Server.MapPath((sourcepath.IndexOf("~/") >= 0 ? sourcepath : "~/" + sourcepath));
            if (System.IO.File.Exists(psyfilepath))
            {
                FileInfo finfo = new FileInfo(psyfilepath);
                filename = string.IsNullOrEmpty(filename) ? finfo.Name : filename;

                string UserAgent = context.Request.ServerVariables["http_user_agent"].ToLower();
                if (UserAgent.IndexOf("firefox") == -1)
                    filename = HttpUtility.UrlEncode(filename, System.Text.Encoding.UTF8);

                context.Response.AddHeader("Content-Length", (finfo.Length).ToString());
                context.Response.ContentType = Utility.ConvertFileType(finfo.Extension);
                //下载时对文件名进行编码,否则中文会出现乱码.
                context.Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);

                context.Response.TransmitFile(psyfilepath);
            }
            else
            {
                context.Response.Write("文件无效");
            }
            context.Response.End();
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

    }
}