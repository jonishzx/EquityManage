<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <meta http-equiv="Content-Language" content="UTF-8" />
    <link href="<%= Url.Content("~/Content/Admin/login.css") %>" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src='<%= Url.Content("~/scripts/iepngfix_tilebg.js") %>'></script>
    <!--[if lt IE 7]>
    <script type="text/javascript" src="<%=Url.Content("~/scripts/pngfix.js") %>"></script>
    <![endif]-->
    <!--<script type="text/javascript" src='<%= Url.Content("~/scripts/jquery.min.js") %>'></script>-->
    <!--<script type="text/javascript" src='<%= Url.Content("~/scripts/VCode.js") %>'></script>-->
    <title>
        <%=Clover.Config.WebSiteConfig.Config.WebAppName%>
    </title>
    <style media="screen" type="text/css">
        .pngfix, span
        {
            behavior: url(<%= Url.Content("~/scripts/iepngfix.htc") %>);
        }
    </style>
    <script type="text/javascript">
        var cookieflag = "<%=Clover.Config.WebSiteConfig.Config.SystemNo%>" + "_Admin";
        document.onkeydown = function (evt) {
            var evt = window.event ? window.event : evt;
            var targetId = (evt.target) ? evt.target.id : evt.srcElement.id;
            if (targetId != "UserId" && evt.keyCode == 13) {
                document.getElementById("btnSubmit").click();
            }
        }
        function loadValidCode() {
            //$('.imgVCode').attr("src", "../ajax/ValidCode.ashx?t=" + new Date().getTime());
        }
        function pageWidth() {return window.innerWidth != null? window.innerWidth: document.body != null? document.documentElement.clientWidth:null;}
        function pageHeight() {return window.innerHeight != null? window.innerHeight: document.body != null? document.documentElement.clientHeight:null;}
        function isFullscreen()
        {
          return pageWidth() >= screen.width && pageHeight() >= screen.height;
        }
        function fullscreen(navurl){          
           if(!isFullscreen()){
               var win = window.open(navurl,'','dependent=yes,fullscreen=yes,scrollbars=auto');
               window.opener = null;
               window.close();
           }
           else{
               window.location.href = navurl;
           }
        }
       
        <%=ViewData["Reidirection"]%>
    </script>
</head>
<body style="overflow: hidden;" scroll="no">
    <div id="LoginContainer">
        <img src="<%= Url.Content("~/Content/Images/Admin/login_bg_left.gif")%>" class="loginBgImage" />
        <div id="LoginMain">
            <div id="LoginLogo">
                <div id="LoginTitle">
                    <h3>
                        <%=Clover.Config.WebSiteConfig.Config.WebAppName%></h3>
                </div>
<%--                <span id="Version" class="pngfix"></span>--%>
            </div>
            <div id="LoginFormContainer">
                <%using (Html.BeginForm("Login", "Account", FormMethod.Post, new { @name = "_FORM", @class = "LoginForm" }))
                  {%>
                <p>
                    <label for="TestUserName">
                        <strong id="ForTestUserName">员工号:</strong></label>
                    <input id="UserId" name="UserId" class="TxtInput" autocomplete="off" maxlength="20" value="<%=ViewData["PUserId"] %>"
                        type="text" />
                    <br />
                    <%= Html.ValidationMessage("UserId")%>
                </p>
                <p>
                    <label for="TestUserPWD">
                        <strong id="ForTestUserPWD">密&nbsp;&nbsp;&nbsp;码:</strong></label>
                    <input id="Password" name="Password" class="TxtInput" autocomplete="off" type="password" maxlength="20" />
                    <br />
                    <%= Html.ValidationMessage("Password")%>
                </p>
                <p>
                    <label for="cbRemberme">
                        &nbsp;</label>
                    <%=Html.CheckBox("rememberMe", ViewData["rememberMe"]!=null ? (bool)ViewData["rememberMe"]:false)%>
                    <span id="ForRememberMe" style="margin: 10px 0pt 0pt;">记住用户</span>

                     <%-- <%=Html.CheckBox("IsFullScreen",  ViewData["IsFullScreen"]!=null ? (bool)ViewData["IsFullScreen"]:false)%>
                    <span id="IsFullScreen" style="margin: 10px 0pt 0pt; padding-left: 5px;">全屏打开</span>--%>
                </p>
                <p>
                    <input type="hidden" name="HttpRefer" value="" />
                    <button id="btnSubmit" type="submit" title="登录" class="LoginButton" >
                        <img src="<%=Url.Content("~/Content/Admin/Images/submit.gif") %>" />登录
                    </button>
                    
                    <input type="hidden" name='returnUrl' value='<%=ViewData["returnUrl"]%>' />
                </p>
                <%} %>
            </div>
        </div>
        <img src="<%= Url.Content("~/Content/Admin/Images/login_bg_right.gif")%>" class="loginBgImage" />
        <br class="clear" />
        <div id="shadow">
            <img src="<%= Url.Content("~/Content/Admin/Images/login_shadow_left.gif")%>" class="loginBgImage" />
            <div id="ShadowCenter">
                <center>
                    <br />
                    Copyright © 2011-<%=DateTime.Now.Year%>
                    <%=Clover.Config.WebSiteConfig.Config.Company%>
                    Lt. All rights reserved.
                </center>
                <center>
                    <br />
                </center>
            </div>
            <img src="<%= Url.Content("~/Content/Admin/Images/login_shadow_right.gif")%>" class="loginBgImage" />
        </div>
    </div>
</body>
</html>
