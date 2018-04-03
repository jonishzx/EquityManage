<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="errorHead" ContentPlaceHolderID="head" runat="server">
    <title>发生错误 : <%=TempData["ErrorName"]%></title>
</asp:Content>

<asp:Content ID="errorContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        抱歉，系统运行时发生错误 : <%=TempData["ErrorMessage"] %>.
    </h2>
</asp:Content>