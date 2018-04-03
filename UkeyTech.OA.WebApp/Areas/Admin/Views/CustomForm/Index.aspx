<%@ Page Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/AdminList.Master"
    Title="自定义表单管理" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Register Src="../Shared/Loading.ascx" TagName="Loading" TagPrefix="ld" %>
<%@ Register Src="../Shared/PopupWin.ascx" TagName="PopupWin" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
    <script type="text/javascript" src="<%=Url.Content("~/ModuleJs/CustomForm/CustomForm.js")%>"></script>
    <script type="text/javascript">
        var baseurl = '<%=Url.Action("","CustomForm")%>';
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="SearchDiv" region="north">
        代码或名称：<input type="text" id="SearchText" style="width: 120px;" />
        <a href="javascript:void(0);" class="easyui-linkbutton" icon="icon-search" onclick="queryData();">
            查询</a>
    </div>
    <div id="center" region="west"  style="width: 100%;">
        <table id="DataGrid">
        </table>
    </div>
   <%-- <div id="right" region="center" title="表单字段列表" style="width: 300px">
        <table id="FormColumnGrid">
        </table>
    </div>--%>
    <uc2:PopupWin ID="PopupWin1" runat="server" />    
</asp:Content>
