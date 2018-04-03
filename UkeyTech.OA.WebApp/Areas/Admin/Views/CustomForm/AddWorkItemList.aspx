<%@ Page Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/AdminList.Master"
    Title="流程信息管理" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Register Namespace="RepeaterInMvc.Codes" TagPrefix="MVC" %>
<%@ Register Src="../Shared/Loading.ascx" TagName="Loading" TagPrefix="ld" %>
<%@ Register Src="../Shared/PopupWin.ascx" TagName="PopupWin" TagPrefix="uc2" %>
<%@ Register src="../Shared/ScriptBlock.ascx" tagname="ScriptBlock" tagprefix="uc1" %>
<%@ Register src="../Widget/AddWorkItemList.ascx" tagname="WorkItemList" tagprefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">

 
    <uc1:ScriptBlock ID="ScriptBlockA" runat="server" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   
    <div id="center" region="center" title="流程管理" style="width: auto;">
        <uc3:WorkItemList ID="WorkItemList1" runat="server" />
    </div>
    <uc2:PopupWin ID="PopupWin1" runat="server" />
   
</asp:Content>
