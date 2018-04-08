using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Clover.Config.Menu;
using Clover.Core.Collection;
using UkeyTech.OA.WebApp;
using UkeyTech.WebFW.DAO;
using Clover.Permission.Model;
using Clover.Permission.BLL;
using UkeyTech.WebFW.Model;

namespace Clover.Web.HTMLRender
{
    /// <summary>
    /// 数结构输出
    /// </summary>
    public class ModuleTreeRender
    {
        /*
         *  <li>
        <span>Folder</span>
        <ul>
            <li>
                <span>Sub Folder 1</span>
                <ul>
                    <li>
                        <span><a href="#">File 11</a></span>
                    </li>
                    <li>
                        <span>File 12</span>
                    </li>
                    <li>
                        <span>File 13</span>
                    </li>
                </ul>
            </li>
            <li>
                <span>File 2</span>
            </li>
            <li>
                <span>File 3</span>
            </li>
        </ul>
    </li>
    <li>
        <span>File21</span>
    </li>
         */
        /// <summary>
        /// 输出指定节点的子节点
        /// </summary>
        /// <param name="parentid"></param>
        /// <returns></returns>
        public static string RenderNodeS(Tree<Module> tree, string parentid)
        {
            StringBuilder sb = new StringBuilder(500);

            if (parentid != string.Empty)
            {
                rcRenderChildren(tree.FindChildren(parentid), sb, parentid, true);
            }
            else {
                rcRenderChildren(tree.FindRoot(),sb, "" ,true);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 输出指定节点的子节点
        /// </summary>
        /// <param name="parentid"></param>
        /// <returns></returns>
        public static string RenderNodeSWithPrivilege(Tree<Module> tree, string parentid)
        {
            StringBuilder sb = new StringBuilder(500);

            if (parentid != string.Empty)
            {
                rcRenderChildren(tree.FindChildren(parentid), sb, parentid, true, true);
            }
            else
            {
                rcRenderChildren(tree.FindRoot(), sb, "", true, true);
            }
            return sb.ToString();
        }

        public static string RenderReportSetting(List<ReportSetting> list, string url, string target)
        {
            if (list == null)
                return string.Empty;

            var groups = list.Select(x => x.Tag).Distinct().ToList();
            var sb = new StringBuilder();
            foreach (var @group in groups)
            {
                sb.AppendLine("<li>");
                sb.AppendLine("<span><a href=\"javascript:void(0)\">" + @group + "</a></span>");

                //nodes
                var sublist = list.Where(x => x.Tag == @group).ToList();
                sb.AppendLine("<ul>");
                foreach (var c in sublist)
                {
                    sb.AppendLine("<li>");
                    sb.AppendLine("<span><a href=\"" + 
                        (!string.IsNullOrEmpty(c.ViewAction)?c.ViewAction : url) + "?id=" + c.RecId +
                        "\" target=\"" + target + "\" >" + c.Title + "</a></span>");
                    sb.AppendLine("</li>");
                }
                sb.AppendLine("</ul>");
                //nodes

                sb.AppendLine("</li>");
            }
            return sb.ToString();
        }

        private static void rcRenderChildren(List<TreeNode<Module>> nodes, StringBuilder sb, string parentid, bool first)
        {
             rcRenderChildren(nodes, sb, parentid, first, false);
        }

        /// <summary>
        /// 检查是否显示在菜单的功能代码
        /// </summary>
        static readonly string[] CheckModuleMenuFuncCodes = new string[3] { "Browse", "Menu", "List" };
        private static void rcRenderChildren(List<TreeNode<Module>> nodes, StringBuilder sb, string parentid, bool first, bool checkprivilege)
        {
            if (first)
                sb.AppendLine("<ul class=\"easyui-tree tree\" animate=\"true\" dropable=\"false\" dnd=\"false\">");
            else
                sb.AppendLine("<ul>");

            foreach (TreeNode<Module> item in nodes)
            {
                List<TreeNode<Module>> children = item.getChildren();

                Module it = item.getNode();

                if (checkprivilege == false || children.Count > 0 || (checkprivilege && children.Count == 0 &&
                    Helper.CurrUserPermission.Exists(x => x.ModuleCode == it.ModuleCode
                                                          && CheckModuleMenuFuncCodes.Contains(x.FunctionCode))))
                    //验证是否去具有Broswer或是Menu权限才显示内容
                {
                    sb.AppendLine("<li>");
                    sb.AppendLine("<span><a href=\"" +
                                  (it != null && it.Target != null
                                       ? Clover.Web.Core.Utility.ConvertAbsoulteUrl(it.Target)
                                       : "") +
                                  "\" target=\"" + it.TargetFrame + "\" >"
                                  + it.Name + "</a></span>");
                }

                if (children.Count > 0)
                {
                    rcRenderChildren(children, sb, parentid, false, checkprivilege);
                }

                sb.AppendLine("</li>");
            }

            sb.AppendLine("</ul>");
        }
    }
}
