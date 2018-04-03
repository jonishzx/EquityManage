<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/CustomError.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    你无权限访问该资源不存在
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    你无权限访问该资源(<%=Request.RawUrl %>),你可以选择<a href="javascript:window.history.back(-1);">返回</a>或<a href="/Admin/Account/Login">登录</a>。
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

</asp:Content>
