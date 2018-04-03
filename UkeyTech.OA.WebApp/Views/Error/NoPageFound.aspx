<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/CustomError.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    你访问的资源不存在
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    你访问的资源(<%=Request.RawUrl %>)不存在,请联系系统管理员检查资源的正确性，
    <br />
    你还可以选择<a href="javascript:window.history.back(-1);">返回</a>或重新<a href="/Admin/Account/Login">登录</a>。
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

</asp:Content>
