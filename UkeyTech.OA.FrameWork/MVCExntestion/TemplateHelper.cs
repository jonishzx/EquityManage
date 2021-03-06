﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Clover.Core;
namespace UkeyTech.OA.WebApp.Extenstion
{
    /// <summary>
    /// 模版工具
    /// </summary>
    public class TemplateHelper
    {
        private string _templatedir = "~/Template";
        /// <summary>
        /// 模板目录
        /// </summary>
        public string TemplateDir { get { return _templatedir; } set { _templatedir = value; } }

        /// <summary>
        /// 模板位置
        /// </summary>
        /// <param name="templateDir"> </param>
        public TemplateHelper(string templateDir)
        {
            _templatedir = templateDir;
        }

        /// <summary>
        /// 获取模版输出的内容
        /// </summary>
        /// <param name="templatename"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string LoadTemplateOutput(string templatename, Dictionary<string, object> parameters)
        {
            var dir = Clover.Core.IO.PathTool.getInstance().Map(_templatedir);
            if(System.IO.Directory.Exists(dir))
            {
                var filepath = System.IO.Path.Combine(dir, templatename + ".htm");
                string content = System.IO.File.ReadAllText(filepath);

                foreach (var key in parameters.Keys)
                {
                    content = content.Replace("{" + key + "}", parameters[key].ToString());
                }
                //一些具体的特殊参数
                content = content.Replace("{NOWDATE}", DateTime.Now.ToString("yyyy-MM-dd"));
                content = content.Replace("{NOWDATETIME}", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));

                return content;
            }

            return string.Empty;
        }
    }
}