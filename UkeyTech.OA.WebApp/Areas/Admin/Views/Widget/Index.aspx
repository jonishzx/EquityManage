<%@ Page Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/AdminList.Master"
    Title="部件信息设置" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Register Src="../Shared/Loading.ascx" TagName="Loading" TagPrefix="ld" %>
<%@ Register Src="../Shared/PopupWin.ascx" TagName="PopupWin" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
    <script type="text/javascript" src="<%=Url.Content("~/ModuleJs/System/Widget.js")%>"></script>
    <script type="text/javascript">
        var baseurl = '<%=Url.Action("","Widget")%>';
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="SearchDiv"  region="north">
        代码或名称：<input type="text" id="SearchText" style="width: 120px;" />
        <a href="javascript:void(0);" class="easyui-linkbutton" icon="icon-search" onclick="queryData();">
            查询</a>
    </div>
    <div id="center" region="center" title="部件列表" style="width: auto;">
        <table id="DataGrid">
        </table>
    </div>
    <uc2:PopupWin ID="PopupWin1" runat="server" />
</asp:Content>
