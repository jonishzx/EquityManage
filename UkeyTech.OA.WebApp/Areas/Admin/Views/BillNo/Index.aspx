x<%@ Page Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/AdminList.Master"
    Title="业务单据信息" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Register Src="../Shared/Loading.ascx" TagName="Loading" TagPrefix="ld" %>
<%@ Register Src="../Shared/PopupWin.ascx" TagName="PopupWin" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
    <script type="text/javascript" src="<%=Url.Content("~/ModuleJs/System/BillNo.js")%>"></script>
    <script type="text/javascript">
        var baseurl = '<%=Url.Action("","BillNo")%>';       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="top" region="north" title="单据号修复内容设置" style="height: 55px;">
       <div><label>主键:</label><input id="keyfield" value="STC_Id"/>
       <label>排序字段:</label><input id="sortfield" value="CreateTime"/>
       <label>单据格式:</label><input id="format" value="{document}{datetime}{Number}"/>
       <label>日期格式:</label><input id="datetimeformat" value="yyyy"/>
       </div>
    </div>
    <div id="center" region="center" title="单据规则列表" style="width: auto;">
        <table id="DataGrid">
        </table>
    </div>
    <div id="right" region="east" title="异常编号" style="width: 200px;">
        <table id="ExceptGrid">
        </table>
    </div>
    <uc2:PopupWin ID="PopupWin1" runat="server" />
</asp:Content>
