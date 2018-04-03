<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<UkeyTech.WebFW.Model.Widget>" %>
<%@ Import Namespace="Clover.Permission.Model" %>
<%@ Import Namespace="Clover.Web.Core" %>
<%@ Import Namespace="UkeyTech.OA.WebApp.Areas.Admin.Controllers" %>
<%@ Import Namespace=" StructureMap" %>
<%@ Import Namespace=" UkeyTech.WebFW.DAO" %>
<%@ Import Namespace=" UkeyTech.WebFW.Model" %>
   <script type="text/javascript">
       var currtitle = "", currlisturl = "";
       var progresstraceurl = '<%=Url.Action("ProgressTrace","WorkFlow")%>';
       $(function () {
           var addwilist = $(".addworkitemList").find(".thumb-wrap");
           $(addwilist).hover(
            function () { $(this).addClass("x-view-over"); },
            function () { $(this).removeClass("x-view-over"); })
            .find(".add").click(function () {
                //SetWinWithMaxSize($(this).attr("title"), $(this).attr("url"));
                SetWin($(document).width(), $(document).height(), $(this).attr("url"), $(this).attr("title"));
                var list = $(this).parent().find(".list");
                currtitle = $(list).attr("title");
                currlisturl = $(list).attr("url");
                SetBackFunc(AddSuccess);
            });

            $(addwilist).find(".list").click(function () {
               openListParentTab($(this).attr("title"), $(this).attr("url"));
           });
       });

       function AddSuccess() {
           setTimeout("MsgShow('系统提示','添加成功。');", 500);
           openListParentTab(currtitle, currlisturl);
       }

       function openParentTab() {
           window.parent.addTab("进度查询", null, progresstraceurl, "main");
       }
    </script>
<%
    var _webcontext = ObjectFactory.GetInstance<IWebContext>();
    FormDAO dal = ObjectFactory.GetInstance<FormDAO>();
    int rowscount = 0; //(null, PageSize, PageIndex, strWhere, desc, "ID", out rstcount)
    var result = dal.GetAllPaged(null,
        int.MaxValue, 1, @" Status = 1 and Visible = 1 and exists (
            select distinct cm.ModuleCode from dbo.CPM_Role cr
            join dbo.CPM_Role_User cru ON cr.RoleId = cru.RoleId
            join dbo.CPM_FuncPermission cfp ON cfp.RoleId = cr.RoleId and FunctionID = 1
            join dbo.CPM_Module cm ON cm.ModuleID = cfp.ModuleID and  cm.Status = 1 
            where cm.ModuleCode = FormCode and cru.UserId = '" + _webcontext.CurrentUser.UniqueId + "') ", true, " ViewOrd,ID ", out rowscount);
    
    var groups = new List<WorkItemsGroup>();
    var typelist = new List<string>();
 
    WorkItemsGroup group = null;
    foreach (var m in result)
    {
        if (!typelist.Contains(m.FormType))
        {
            group = new WorkItemsGroup();
            group.Id = group.Title = m.FormType;
            groups.Add(group);
            typelist.Add(m.FormType);
            var children = result.FindAll(x => x.FormType == group.Title);
            bool show = true;
            foreach (var c in children)
            {
                show = true;
                //角色判断
                if (!string.IsNullOrEmpty(c.LmtRoleIds))
                {
                    show = false;
                    var roleids = c.LmtRoleIds.Split(new char[]{','}, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var role in _webcontext.CurrentUser.Roles)
                    {
                        if (roleids.Contains(role.RoleID.ToString()))
                        {
                            show = true;
                            break;
                        }
                    }
                }
                //组织判断
                if (!string.IsNullOrEmpty(c.LmtGroupIds))
                {
                    show = false;
                    var groupids = c.LmtGroupIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    show = groupids.Contains(_webcontext.CurrentUser.CurrGroupId);
                }
                //岗位判断
                if (!string.IsNullOrEmpty(c.LmtPosIds))
                {
                    show = false;
                    var groupids = c.LmtPosIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    show = groupids.Contains(_webcontext.CurrentUser.CurrPositionId);
                }
                
                if(show)
                    group.Items.Add(c);
            }
        }
    }

    
    foreach (var g in groups)
    {%>
<div class="addworkitemList common-ct">
    <tpl for=".">
        <div>
    <a name="<%=g.Id%>"></a>
    <h2><div><%=g.Title%></div></h2>
        <dl>
    <tpl for="samples">
        <%  foreach (var it in g.Items)
            {
                var o = (Form)it;
                %>
                <div class="thumb-wrap">
                        <img src="<%= o.ImageUrl != null ? o.ImageUrl : Url.Content("~/Content/Images/noimg.gif")%>" title="<%=o.FormName %>" />
                    <div>
                        <H4><%=o.FormName %></H4>
                        <P><%=o.Descn %></P>
                        <p><a class="add" opid="<%=o.ID %>" title="添加<%=o.FormName %>"  url="<%= (o.ExternalFormUrl !=null ? o.ExternalFormUrl : "UserSubmitData") + "?formid=" + o.ID%>" href='javascript:void(0);'>添加</a> 
                        | <a class="list" title="<%=o.FormName %>列表" url="<%=(o.ExternalListUrl !=null ? o.ExternalListUrl : Url.Action("UserSubmitDataList","CustomForm")) + "?formid=" + o.ID%>" href='javascript:void(0)'>信息列表</a></p>
                    </div>
                </div>
                <%} %>
        </tpl>
    <div style="clear:left"></div>
    </dl>
    </div>
    </tpl>
</div>
<%}
%>