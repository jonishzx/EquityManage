<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="errorHead" ContentPlaceHolderID="head" runat="server">
    <title>无权限访问</title>
    <style type="text/css">
        h1{font-size:25px;padding:10px;}
        h2{font-size:20px;}
        .currentuser{font-size:15px;padding:5px;}
        .redirection{font-size:15px;padding:5px;}
        .redirection a{color:#006EB1!important;  font-size: 15px !important;font-weight: bold;}
        #main{border: 1px solid #AABCCF;height: auto;margin: 15% 0 0 35%;width: 600px;}
    </style>
</asp:Content>
<asp:Content ID="errorContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        <img id="img1" src="~/Content/Images/stopaccess.gif" width="32" height="32" runat=server />
        抱歉，你无权限访问该功能 :
        <b><%=TempData["ErrorName"] %></b><span id="timeout"></span>.
    </h2>
    <div class="currentuser">
     
        <div>
            <%if (TempData["CurrentUser"] != "尚未登录")
              {%>
            你当前登录的用户身份为: <b>
                <%=TempData["CurrentUser"]%></b>.
             <%}
              else
              {%>
                由于你目前<%= TempData["CurrentUser"]  %>。
             <%} %>
         </div>
    </div>
    <div class="redirection">
        你可以选择
        <%if ((bool)TempData["IsLogin"])
        {%>
            <a href="~/Admin/Home" runat="server" target="_top">返回首页</a>,或者
        <%} %>
        <a id="A1"
            href="<%=Clover.Web.Core.Utility.ConvertAbsoulteUrl("~/Admin/Account/Login")%>?returnUrl=<%=TempData["ReturnUrl"] %>" target="_top">以其他用户身份登录</a>
    </div>
    <script type="text/javascript">
        var tip, sec = 5;

        function autoQuit(){
            sec --;
            tip.innerHTML = ', 浏览器将在' + sec + "秒后会自动返回到登录界面"
            if(sec == 0)
                window.parent.parent.location.href = "<%=Clover.Web.Core.Utility.ConvertAbsoulteUrl("~/Admin/Account/Login")%>?returnUrl=<%=TempData["ReturnUrl"] %>";
        }

        window.onload = function () {
            //if(<%=ViewData["AutoToLogin"] != null ? "true" : "fasle" %>){
                tip = document.getElementById("timeout");

                //setInterval(autoQuit, 1000);
            //}
        }
    </script>
</asp:Content>
