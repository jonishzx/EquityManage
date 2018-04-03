<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/jquery-1.7.2.min.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/common.min.js")%>"></script>
    <script type="text/javascript">
        $(function () {

            if (typeof (window.top.closeSelectedTab) != "undefined") {
                var timeout = 5000;
                setTimeout(window.top.closeSelectedTab, timeout+1000);
                setInterval(function () {
                    $("#spTimeout").html(timeout / 1000 + "秒后自动关闭该页面.");
                    timeout -= 1000;
                }, 1000);
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>操作已完成,<span id="spTimeout">请稍候...</span></h2>
</asp:Content>


