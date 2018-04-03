<%@ Page Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/AdminList.Master"
    Title="用户信息" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Register Src="../Shared/Loading.ascx" TagName="Loading" TagPrefix="ld" %>
<%@ Register Src="../Shared/PopupWin.ascx" TagName="PopupWin" TagPrefix="uc2" %>
<%@ Register src="GroupPositionEdit.ascx" tagname="GroupPositionEdit" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
    <script type="text/javascript">
        var baseurl = '<%=Url.Action("","Account")%>';
        var GetGroupTree = '<%=Url.Action("Group","Permission")%>' + "?type=GetGroupTree";
        var GroupPositionUrl = '<%=Url.Action("UserGroupPositionListWithAdminId","Account")%>';
        var GroupPositionPostUrl = '<%=Url.Action("SetDefaultUserGroupPositionWithAdminId","Account")%>';
    </script>
    <script type="text/javascript" src="<%=Url.Content("~/ModuleJs/Permission/Account.js")%>"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <form id="form1" runat="server">
    <div class="SearchDiv" region="north">
        用户账号：<input type="text" id="txtLoginId" style="width: 120px;" />
        用户名称：<input type="text" id="txtName" style="width: 120px;" />
        <a href="javascript:void(0);" onclick="queryData();" class="easyui-linkbutton" icon="icon-search">
            查询</a>
    </div>
    <div id="west" region="west" split="true" title="组织架构树" style="width: 200px;">
        <div id="dvAllGroup" class="tree-node" style="cursor: pointer;" onclick="searchAllGroup();">
            <span class="tree-indent"></span><span class="tree-icon tree-file"></span><span class="tree-title">
                查询全部</span></div>
        <ul id="treeGroup">
        </ul>
        
    </div>
    <div id="center" region="center" title="用户列表" style="width: auto;">
        <table id="DataGrid">
        </table>
    </div>
    <div region="east" title="岗位列表" style="width: 280px;">
        <table id="GroupPosition">
        </table>
    </div>
    <uc2:PopupWin ID="PopupWin1" runat="server" />
    </form>
    <uc1:GroupPositionEdit ID="GroupPositionEdit1" runat="server" />
</asp:Content>
